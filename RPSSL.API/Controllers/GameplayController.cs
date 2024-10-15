using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

using Mmicovic.RPSSL.Service;
using Mmicovic.RPSSL.API.Models;
using Mmicovic.RPSSL.API.Validators;

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
        public async Task<Shape> GetRandomChoice(CancellationToken ct)
        {
            logger.LogDebug("Received request for a random choice");

            var randomShape = await gameManager.GetRandomShape(ct);
            logger.LogDebug($"Random choice generated: {randomShape.Name}");

            return new Shape(randomShape);
        }

        // GET api/play
        [HttpGet("play")]
        public async Task<IEnumerable<GameRecord>> GetGameRecords([FromQuery] int? take, CancellationToken ct)
        {
            logger.LogDebug($"Received request for saved game records, take: {take}");

            var records = await gameManager.GetGameRecords(take, ct);
            logger.LogDebug($"Returning {records.Count()} records");

            return records.Select(r => new GameRecord(r));
        }

        // POST api/play
        [HttpPost("play")]
        public async Task<GameRecord> PostGameRecord([FromBody] GameRecord command, CancellationToken ct)
        {
            logger.LogDebug($"Received request for a new game with player shape: {command.PlayerChoice}");
            new PostGameRecordValidator(gameManager.IsValidShapeId).Validate(command);

            var record = await gameManager.Play(command.ToServiceObject(), ct);
            logger.LogDebug($"A game has been played with shape {record.PlayerChoice} against {record.ComputerChoice}, " +
                            $"Result: {record.Result}");

            return new GameRecord(record);
        }

        // DELETE api/play
        [HttpDelete("play")]
        public async Task DeleteGameRecords()
        {
            logger.LogDebug($"Received request to delete game records");
            await gameManager.DeleteGameRecords();
        }
    }
}
