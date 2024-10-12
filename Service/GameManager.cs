using Mmicovic.RPSSL.Service.Models;

namespace Mmicovic.RPSSL.Service
{

    public interface IGameManager
    {
        Shape GetRandomShape();
        IEnumerable<Shape> GetAllShapes();

        GameRecord PlayAgainstComputer(int playerShape);
    }

    /* This class implements methods used for accesing the valid hand shapes and playing the game.
     * It can generate random shapes both by request and for the CPU player of a game.
     * For number generation the insecure basic .NET pseudonumber generator is temporarily used.
     * The algorithm for deciding the winner of a game is described in more detailed in the project documentation. */
    public class GameManager : IGameManager
    {
        private const int SHAPE_MIN = 1;
        private const int SHAPE_MAX = 5;

        private readonly List<Shape> allShapes = [new(1, "rock"), new(2, "paper"), new(3, "scissors"),
                                                  new(4, "spock"), new(5, "lizard")];


        public IEnumerable<Shape> GetAllShapes()
        {
            return allShapes;
        }

        public Shape GetRandomShape()
        {
            var randomNumberGenerator = new Random();
            var randomShape = randomNumberGenerator.Next(0, allShapes.Count);
            return allShapes[randomShape];
        }

        public GameRecord PlayAgainstComputer(int playerShape)
        {
            // Verify that the player picked a correct IDs
            VerifyShapeIdRange(playerShape);

            // Choose a random shape for the Computer
            var computerShape = GetRandomShape().Id;

            var result = CalculateGameResult(playerShape, computerShape);
            return new GameRecord(result, playerShape, computerShape);
        }

        private static Result CalculateGameResult(int player1Shape, int player2Shape)
        {
            // If both players picked the same ID, it's a tie
            if (player1Shape == player2Shape)
                return Result.Tie;

            // Calculate the difference between shape IDs with modulo 5
            var comparison = (player2Shape - player1Shape + SHAPE_MAX) % SHAPE_MAX;

            // Player 1 wins against IDs that are bigger by even numbers, loses otherwise
            // Refer to documentation for detailed explanation
            if (comparison % 2 == 0)
                return Result.Win;
            else
                return Result.Lose;
        }

        private static void VerifyShapeIdRange(int shapeId)
        {
            if (shapeId < SHAPE_MIN || shapeId > SHAPE_MAX)
            {
                throw new ArgumentOutOfRangeException("Shape ID is out of the offered range");
            }
        }
    }
}
