using Microsoft.Extensions.Logging;

using Mmicovic.RPSSL.Service.Models;

namespace Mmicovic.RPSSL.Service
{

    public interface IGameManager
    {
        Task<Shape> GetRandomShape();
        IEnumerable<Shape> GetAllShapes();

        Task<GameRecord> PlayAgainstComputer(int playerShape);
    }

    /* This class implements methods used for accesing the valid hand shapes and playing the game.
     * It can generate random shapes both by request and for the CPU player of a game.
     * For number generation an external pseudorandom generator i used.
     * The algorithm for deciding the winner of a game is described in more detailed in the project documentation. */
    public class GameManager(IRandomGenerator randomGenerator, ILogger<GameManager> logger) : IGameManager
    {
        public const int SHAPE_MIN = 1;
        public const int SHAPE_MAX = 5;

        private readonly IRandomGenerator randomGenerator = randomGenerator;
        private readonly ILogger<GameManager> logger = logger;

        private readonly List<Shape> allShapes = [new(1, "rock"), new(2, "paper"), new(3, "scissors"),
                                                  new(4, "spock"), new(5, "lizard")];


        public IEnumerable<Shape> GetAllShapes()
        {
            return allShapes;
        }

        public async Task<Shape> GetRandomShape()
        {
            var randomShape = await randomGenerator.Next(0, allShapes.Count);
            return allShapes[randomShape];
        }

        public async Task<GameRecord> PlayAgainstComputer(int playerShape)
        {
            logger.LogInformation($"CPU game enganged with shape: {playerShape}");

            // Verify that the player picked a correct IDs
            VerifyShapeIdRange(playerShape);

            // Choose a random shape for the Computer
            var computerShape = (await GetRandomShape()).Id;
            logger.LogInformation($"CPU chose shape: {computerShape}");

            var result = CalculateGameResult(playerShape, computerShape);
            logger.LogInformation($"Calculated game result: {result}");

            return new GameRecord(result, playerShape, computerShape);
        }

        private Result CalculateGameResult(int player1Shape, int player2Shape)
        {
            // If both players picked the same ID, it's a tie
            if (player1Shape == player2Shape)
                return Result.Tie;

            // Calculate the difference between shape IDs with modulo 5
            var comparison = (player2Shape - player1Shape + SHAPE_MAX) % SHAPE_MAX;
            logger.LogDebug($"Shape comparison post-modulo: {comparison}");

            // Player 1 wins against IDs that are bigger by even numbers, loses otherwise
            // Refer to documentation for detailed explanation
            if (comparison % 2 == 0)
                return Result.Win;
            else
                return Result.Lose;
        }

        private void VerifyShapeIdRange(int shapeId)
        {
            if (shapeId < SHAPE_MIN || shapeId > SHAPE_MAX)
            {
                logger.LogWarning($"Unsupported shape ID inputted: {shapeId}");
                throw new ArgumentOutOfRangeException("Chosen shape does not exist");
            }
        }
    }
}
