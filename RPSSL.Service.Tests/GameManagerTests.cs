using Microsoft.Extensions.Logging;

using Moq;
using Xunit;

using Mmicovic.RPSSL.Service.Models;

namespace Mmicovic.RPSSL.Service.Tests
{
    public class GameManagerTests
    {
        private const int MAX_SHAPE_ID = 10;
        private const int PLAYER_SHAPE_ID = 4;
        private const int GENERATED_SHAPE_ID = 7;
        private const string GENERATED_SHAPE_NAME = "GeneratedShape";
        private static readonly Shape GENERATED_SHAPE = new(GENERATED_SHAPE_ID, GENERATED_SHAPE_NAME);

        private readonly Mock<IShapeProvider> shapeProvider;
        private readonly Mock<IRandomGenerator> randomGenerator;
        private readonly Mock<IGameResultCalculator> gameResultCalculator;

        private readonly IGameManager gameManager;

        public GameManagerTests()
        {
            shapeProvider = new Mock<IShapeProvider>();
            randomGenerator = new Mock<IRandomGenerator>();
            gameResultCalculator = new Mock<IGameResultCalculator>();

            var logger = new Mock<ILogger<GameManager>>();
            gameManager = new GameManager(randomGenerator.Object, gameResultCalculator.Object,
                                          shapeProvider.Object, logger.Object);
        }

        [Theory]
        [InlineData(Result.Tie)]
        [InlineData(Result.Lose)]
        [InlineData(Result.Win)]
        public async Task PlayAgainstComputer_correctly_relays_data_between_dependencies(Result expectedResult)
        {
            // Setup
            var ct = CancellationToken.None;

            shapeProvider.Setup(sp => sp.GetMaxShapeId()).Returns(MAX_SHAPE_ID);
            shapeProvider.Setup(sp => sp.GetShape(GENERATED_SHAPE_ID)).Returns(GENERATED_SHAPE);
            shapeProvider.Setup(sp => sp.IsValidShapeId(PLAYER_SHAPE_ID)).Returns(true);
            randomGenerator.Setup(rg => rg.Next(1, MAX_SHAPE_ID + 1, ct))
                           .Returns(Task.FromResult(GENERATED_SHAPE_ID));
            gameResultCalculator.Setup(grc => grc.Calculate(PLAYER_SHAPE_ID, GENERATED_SHAPE_ID))
                                .Returns(expectedResult);

            // Execution
            var gameRecord = await gameManager.PlayAgainstComputer(PLAYER_SHAPE_ID, ct);

            // Verification
            shapeProvider.Verify(sp => sp.IsValidShapeId(PLAYER_SHAPE_ID), Times.Once);
            randomGenerator.Verify(rg => rg.Next(1, MAX_SHAPE_ID + 1, ct), Times.Once);
            gameResultCalculator.Verify(grc => grc.Calculate(PLAYER_SHAPE_ID, GENERATED_SHAPE_ID), Times.Once);

            Assert.Equal(expectedResult, gameRecord.Result);
            Assert.Equal(PLAYER_SHAPE_ID, gameRecord.Player1Choice);
            Assert.Equal(GENERATED_SHAPE_ID, gameRecord.Player2Choice);
        }

        [Fact]
        public async Task PlayAgainstComputer_raises_exception_for_invalid_shape_id()
        {
            // Setup
            var ct = CancellationToken.None;
            shapeProvider.Setup(sp => sp.IsValidShapeId(PLAYER_SHAPE_ID)).Returns(false);

            // Execution
            var execute = async () => await gameManager.PlayAgainstComputer(PLAYER_SHAPE_ID, ct);

            // Verification
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(execute);
        }
    }
}
