using Microsoft.Extensions.Logging;

using Mmicovic.RPSSL.Service.Models;

namespace Mmicovic.RPSSL.Service
{

    public interface IGameManager
    {
        Task<Shape> GetRandomShape(CancellationToken ct);
        IEnumerable<Shape> GetAllShapes();

        bool IsValidShapeId(int shapeId);

        Task<GameRecord> Play(GameRecord gameRecord, CancellationToken ct);

        Task<bool> DeleteGameRecords(string user, long? id, CancellationToken ct);
        Task<IEnumerable<GameRecord>> GetGameRecords(string user, int? take, CancellationToken ct);
    }

    /* This class implements methods used for accesing the valid hand shapes and playing the game.
     * It does that as a fascade which provides a single point of access to and simplifies the usage
     * of underlying services (RandomGenerator, ShapeProvider, GameResultCalculator, GameRecordRepository)
     * It can generate random shapes both by request and for the CPU player of a game.
     * For number generation an external pseudorandom generator is used. */
    public class GameManager(IRandomGenerator randomGenerator, IGameResultCalculator gameResultCalculator,
                             IShapeProvider shapeProvider, IGameRecordRepository gameRecordRepository,
                             ILogger<GameManager> logger) : IGameManager
    {
        private readonly ILogger<GameManager> logger = logger;
        private readonly IShapeProvider shapeProvider = shapeProvider;
        private readonly IRandomGenerator randomGenerator = randomGenerator;
        private readonly IGameResultCalculator gameResultCalculator = gameResultCalculator;
        private readonly IGameRecordRepository gameRecordRepository = gameRecordRepository;


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

        public async Task<GameRecord> Play(GameRecord gameRecord, CancellationToken ct)
        {
            logger.LogInformation($"New game enganged with shape: {gameRecord.PlayerChoice}");

            // Verify that the user was inputted and that the player picked a correct ID
            VerifyUserAndShapeId(gameRecord.User, gameRecord.PlayerChoice);

            // Choose a random shape for the Computer
            var computerShape = (await GetRandomShape(ct)).Id;
            logger.LogInformation($"CPU chose shape: {computerShape}");

            // Calculate the result of the game
            var result = gameResultCalculator.Calculate(gameRecord.PlayerChoice!.Value, computerShape);
            logger.LogInformation($"Calculated game result: {result}");

            // Fill new game record fields
            gameRecord.ComputerChoice = computerShape;
            gameRecord.Result = result;

            // Save game record to the database
            await gameRecordRepository.SaveGameRecord(gameRecord);

            return gameRecord;
        }

        private void VerifyUserAndShapeId(string? user, int? playerShape)
        {
            if (user is null)
            {
                logger.LogWarning($"User not inputted");
                throw new ArgumentException("User is a mandatory field");
            }

            if (playerShape is null || !IsValidShapeId(playerShape.Value))
            {
                logger.LogWarning($"Unsupported shape ID inputted: {playerShape}");
                throw new ArgumentOutOfRangeException("Chosen shape does not exist");
            }
        }

        public async Task<IEnumerable<GameRecord>> GetGameRecords(string user, int? take, CancellationToken ct)
        {
            return await gameRecordRepository.GetGameRecords(user, take, ct);
        }

        public async Task<bool> DeleteGameRecords(string user, long? id, CancellationToken ct)
        {
            if (id.HasValue)
            {
                return await gameRecordRepository.DeleteGameRecord(user, id.Value, ct);
            }
            else
            {
                await gameRecordRepository.DeleteGameRecords(user);
                return true;
            }
        }
    }
}
