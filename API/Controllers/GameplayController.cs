using Microsoft.AspNetCore.Mvc;

using Mmicovic.RPSSL.Service;
using Mmicovic.RPSSL.API.Models;

namespace Mmicovic.RPSSL.API.Controllers
{
    [Route("api")]
    [ApiController]
    /* This controller holds a collection of related endpoints all used for
     * accesing the valid hand shapes and playing the game.
     * It works by relaying the requests to the GameManager class of it's Service.
     * The returned objects are converted to the API's object set, which has support
     * for JSON serialization as per desired API formats. */
    public class GameplayController(ILogger<GameplayController> logger) : ControllerBase
    {
        private readonly IGameManager gameManager = new GameManager();
        private readonly ILogger<GameplayController> logger = logger;

        // GET: api/choices
        [HttpGet("choices")]
        public IEnumerable<Shape> GetChoices()
        {
            return gameManager.GetAllShapes().Select(s => new Shape(s));
        }

        // GET: api/choice
        [HttpGet("choice")]
        public Shape GetRandomChoice()
        {
            var randomShape = gameManager.GetRandomShape();
            return new Shape(randomShape);
        }

        // POST api/play
        [HttpPost("play")]
        public GameRecord Post([FromBody] PlayCommand command)
        {
            var record = gameManager.PlayAgainstComputer(command.ShapeId);

            logger.LogDebug($"A game has been played with shape {command.ShapeId}, result: {record.Result}");
            return new GameRecord(record);
        }
    }
}
