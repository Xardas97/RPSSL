using Microsoft.Extensions.Logging;

using Moq;
using Xunit;

using Mmicovic.RPSSL.Service.Models;

namespace Mmicovic.RPSSL.Service.Tests
{
    public class GameResultCalculatorTests
    {
        private readonly IShapeProvider shapeProvider;
        private readonly IGameResultCalculator gameResultCalculator;

        public GameResultCalculatorTests()
        {
            shapeProvider = new ShapeProvider();

            var calculatorLogger = new Mock<ILogger<GameResultCalculator>>();
            gameResultCalculator = new GameResultCalculator(calculatorLogger.Object);
        }

        [Theory]
        [InlineData("Rock", "Rock", Result.Tie)]
        [InlineData("Rock", "Paper", Result.Lose)]
        [InlineData("Rock", "Scissors", Result.Win)]
        [InlineData("Rock", "Spock", Result.Lose)]
        [InlineData("Rock", "Lizard", Result.Win)]
        [InlineData("Paper", "Rock", Result.Win)]
        [InlineData("Paper", "Paper", Result.Tie)]
        [InlineData("Paper", "Scissors", Result.Lose)]
        [InlineData("Paper", "Spock", Result.Win)]
        [InlineData("Paper", "Lizard", Result.Lose)]
        [InlineData("Scissors", "Rock", Result.Lose)]
        [InlineData("Scissors", "Paper", Result.Win)]
        [InlineData("Scissors", "Scissors", Result.Tie)]
        [InlineData("Scissors", "Spock", Result.Lose)]
        [InlineData("Scissors", "Lizard", Result.Win)]
        [InlineData("Spock", "Rock", Result.Win)]
        [InlineData("Spock", "Paper", Result.Lose)]
        [InlineData("Spock", "Scissors", Result.Win)]
        [InlineData("Spock", "Spock", Result.Tie)]
        [InlineData("Spock", "Lizard", Result.Lose)]
        [InlineData("Lizard", "Rock", Result.Lose)]
        [InlineData("Lizard", "Paper", Result.Win)]
        [InlineData("Lizard", "Scissors", Result.Lose)]
        [InlineData("Lizard", "Spock", Result.Win)]
        [InlineData("Lizard", "Lizard", Result.Tie)]
        public void Calculator_correctly_calculates_game_result(string player1Shape, string player2Shape,
                                                                Result expectedResult)
        {
            // Setup
            var allShapes = shapeProvider.GetAllShapes();
            var player1ShapeId = GetShapeId(allShapes, player1Shape);
            var player2ShapeId = GetShapeId(allShapes, player2Shape);

            // Execution
            var result = gameResultCalculator.Calculate(player1ShapeId, player2ShapeId);

            // Verification
            Assert.Equal(expectedResult, result);
        }

        private int GetShapeId(IEnumerable<Shape> allShapes, string shapeName)
        {
            return allShapes.First(s => string.Equals(s.Name, shapeName, StringComparison.OrdinalIgnoreCase)).Id;
        }
    }
}
