
namespace Mmicovic.RPSSL.Service.Models
{
    public enum Result { Win, Lose, Tie }

    public class GameRecord(Result result, int player1Choice, int player2Choice)
    {
        public Result Result { get; init; } = result;
        public int Player1Choice { get; init; } = player1Choice;
        public int Player2Choice { get; init; } = player2Choice;
    }
}
