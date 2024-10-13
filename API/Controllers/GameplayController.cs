using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

using Mmicovic.RPSSL.Service;
using Mmicovic.RPSSL.API.Models;
using Mmicovic.RPSSL.API.Validators;
using Mmicovic.RPSSL.API.Exceptions;

namespace Mmicovic.RPSSL.API.Controllers
{
    [Route("api")]
    [EnableCors(CorsSetup.CORS_POLICY_TEST_UI)]
    [ApiController]
    /* This controller holds a collection of related endpoints all used for
     * accesing the valid hand shapes and playing the game.
     * It works by relaying the requests to the GameManager class of it's Service.
     * The returned objects are converted to the API's object set, which has support
     * for JSON serialization as per desired API formats. */
    public class GameplayController(IGameManager gameManager, ILogger<GameplayController> logger) : ControllerBase
    {
        private readonly IGameManager gameManager = gameManager;
        private readonly ILogger<GameplayController> logger = logger;

        // GET: api/choices
        [HttpGet("choices")]
        public IEnumerable<Shape> GetChoices()
        {
            logger.LogDebug("Received request for all choices");
            return gameManager.GetAllShapes().Select(s => new Shape(s));
        }

        // GET: api/choice
        [HttpGet("choice")]
        public async Task<Shape> GetRandomChoice()
        {
            logger.LogDebug("Received request for a random choice");

            var randomShape = await gameManager.GetRandomShape();
            logger.LogDebug($"Random choice generated: {randomShape.Name}");

            return new Shape(randomShape);
        }

        // POST api/play
        [HttpPost("play")]
        public async Task<GameRecord> PostNewComputerGame([FromBody] PlayCommand command)
        {
            logger.LogDebug($"Received request for a new CPU game with player shape: {command.ShapeId}");
            new PostNewComputerGameValidator().Validate(command);

            var record = await gameManager.PlayAgainstComputer(command.ShapeId!.Value);
            logger.LogDebug($"A CPU game has been played with shape {record.Player1Choice} against {record.Player2Choice}, " +
                            $"Result: {record.Result}");

            return new GameRecord(record);
        }
    }
}
