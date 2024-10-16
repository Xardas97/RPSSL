
namespace Mmicovic.RPSSL.Service.Models
{
    public enum Result { Win, Lose, Tie }

    public class GameRecord(string? user, Result? result, int? playerChoice, int? computerChoice)
    {
        public GameRecord(int playerChoice) : this(null, null, playerChoice, null) { }

        public long? Id { get; set; }
        public string? User { get; set; } = user;
        public Result? Result { get; set; } = result;
        public int? PlayerChoice { get; set; } = playerChoice;
        public int? ComputerChoice { get; set; } = computerChoice;
    }
}
