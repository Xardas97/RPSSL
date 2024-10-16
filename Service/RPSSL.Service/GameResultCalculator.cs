using Microsoft.Extensions.Logging;

using Mmicovic.RPSSL.Service.Models;

namespace Mmicovic.RPSSL.Service
{
    public interface IGameResultCalculator
    {
        Result Calculate(int player1Shape, int player2Shape);
    }

    /* This calculator calculates the result for the first player of a two player game of RPSSL.
     * It uses an algorithm which is specifically tailored to the current setup of Shape IDs
     * as described in the ShapeProvider.cs. Any changes to the ID distribution or shape count
     * would necessitate changes to this algorithm.
     * The justification for the algorithm is described in more detailed in the project documentation.*/
    public class GameResultCalculator(ILogger<GameResultCalculator> logger) : IGameResultCalculator
    {
        private readonly ILogger<GameResultCalculator> logger = logger;

        public Result Calculate(int player1Shape, int player2Shape)
        {
            // If both players picked the same ID, it's a tie
            if (player1Shape == player2Shape)
                return Result.Tie;

            // Calculate the difference between shape IDs with modulo 5
            var comparison = (player2Shape - player1Shape + 5) % 5;
            logger.LogDebug($"Shape comparison post-modulo: {comparison}");

            // Player 1 wins against IDs that are bigger by even numbers, loses otherwise
            // Refer to documentation for detailed explanation
            if (comparison % 2 == 0)
                return Result.Win;
            else
                return Result.Lose;
        }
    }
}
