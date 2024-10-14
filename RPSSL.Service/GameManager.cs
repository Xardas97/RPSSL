using Microsoft.Extensions.Logging;

using Mmicovic.RPSSL.Service.Models;

namespace Mmicovic.RPSSL.Service
{

    public interface IGameManager
    {
        Task<Shape> GetRandomShape(CancellationToken ct);
        IEnumerable<Shape> GetAllShapes();

        bool IsValidShapeId(int shapeId);

        Task<GameRecord> PlayAgainstComputer(int playerShape, CancellationToken ct);
    }

    /* This class implements methods used for accesing the valid hand shapes and playing the game.
     * It does that as a fascade which provides a single point of access to and simplifies the usage
     * of underlying services (RandomGenerator, ShapeProvider, GameResultCalculator)
     * It can generate random shapes both by request and for the CPU player of a game.
     * For number generation an external pseudorandom generator is used. */
    public class GameManager(IRandomGenerator randomGenerator, IGameResultCalculator gameResultCalculator,
                             IShapeProvider shapeProvider, ILogger<GameManager> logger) : IGameManager
    {
        private readonly ILogger<GameManager> logger = logger;
        private readonly IShapeProvider shapeProvider = shapeProvider;
        private readonly IRandomGenerator randomGenerator = randomGenerator;
        private readonly IGameResultCalculator gameResultCalculator = gameResultCalculator;


        public IEnumerable<Shape> GetAllShapes()
        {
            return shapeProvider.GetAllShapes();
        }

        public async Task<Shape> GetRandomShape(CancellationToken ct)
        {
            var randomShapeId = await randomGenerator.Next(1, shapeProvider.GetMaxShapeId() + 1, ct);
            return shapeProvider.GetShape(randomShapeId);
        }

        public bool IsValidShapeId(int shapeId)
        {
            return shapeProvider.IsValidShapeId(shapeId);
        }

        public async Task<GameRecord> PlayAgainstComputer(int playerShape, CancellationToken ct)
        {
            logger.LogInformation($"CPU game enganged with shape: {playerShape}");

            // Verify that the player picked a correct IDs
            VerifyShapeIdRange(playerShape);

            // Choose a random shape for the Computer
            var computerShape = (await GetRandomShape(ct)).Id;
            logger.LogInformation($"CPU chose shape: {computerShape}");

            // Calculate the result of the game
            var result = gameResultCalculator.Calculate(playerShape, computerShape);
            logger.LogInformation($"Calculated game result: {result}");

            // Create a game record
            return new GameRecord(result, playerShape, computerShape);
        }

        private void VerifyShapeIdRange(int shapeId)
        {
            if (!shapeProvider.IsValidShapeId(shapeId))
            {
                logger.LogWarning($"Unsupported shape ID inputted: {shapeId}");
                throw new ArgumentOutOfRangeException("Chosen shape does not exist");
            }
        }
    }
}
