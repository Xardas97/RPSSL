using Microsoft.AspNetCore.Mvc;

using Mmicovic.RPSSL.API.Models;

namespace Mmicovic.RPSSL.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class GameplayController : ControllerBase
    {
        private readonly List<Shape> allShapes = [new(1, "rock"), new(2, "paper"), new(3, "scissors"),
                                                  new(4, "spock"), new(5, "lizard")];

        // GET: api/choices
        [HttpGet("choices")]
        public IEnumerable<Shape> GetChoices()
        {
            return allShapes;
        }

        // GET: api/choice
        [HttpGet("choice")]
        public Shape GetRandomChoice()
        {
            var randomNumberGenerator = new Random();
            var randomShape = randomNumberGenerator.Next(0, allShapes.Count);
            return allShapes[randomShape];
        }

        // POST api/play
        [HttpPost("play")]
        public GameRecord Post([FromBody] PlayCommand command)
        {
            var playerShape = command.ShapeId;
            var computerShape = GetRandomChoice().Id;
            var result = "win";

            return new GameRecord(result, playerShape, computerShape);
        }
    }
}
