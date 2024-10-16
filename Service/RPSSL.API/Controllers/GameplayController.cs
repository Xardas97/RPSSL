using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

using AutoMapper;

using Mmicovic.RPSSL.Service;
using Mmicovic.RPSSL.API.Models;
using Mmicovic.RPSSL.API.Validators;
using Mmicovic.RPSSL.API.Exceptions;
using Mmicovic.RPSSL.API.Initialization;
using Mmicovic.RPSSL.Service.Models;

namespace Mmicovic.RPSSL.API.Controllers
{
    [Route("api")]
    [Authorize]
    [EnableCors(CorsSetup.CORS_POLICY_TEST_UI)]
    [ApiController]
    /* This controller holds a collection of related endpoints all used for
     * accesing the valid hand shapes and playing the game.
     * It works by relaying the requests to the GameManager class of it's Service.
     * The returned objects are converted to the API's object set, which has support
     * for JSON serialization as per desired API formats. */
    public class GameplayController(IGameManager gameManager, IMapper mapper, ILogger<GameplayController> logger) : ControllerBase
    {
        private readonly IMapper mapper = mapper;
        private readonly IGameManager gameManager = gameManager;
        private readonly ILogger<GameplayController> logger = logger;

        // GET: api/choices
        [HttpGet("choices")]
        public IEnumerable<ShapeDTO> GetChoices()
        {
            logger.LogDebug("Received request for all choices");
            var shapes = gameManager.GetAllShapes();
            return mapper.Map<IEnumerable<ShapeDTO>>(shapes);
        }

        // GET: api/choice
        [HttpGet("choice")]
        public async Task<ShapeDTO> GetRandomChoice(CancellationToken ct)
        {
            logger.LogDebug("Received request for a random choice");

            var randomShape = await gameManager.GetRandomShape(ct);

            logger.LogDebug($"Random choice generated: {randomShape.Name}");
            return mapper.Map<ShapeDTO>(randomShape);
        }

        // GET api/play
        [HttpGet("play")]
        public async Task<IEnumerable<GameRecordDTO>> GetGameRecords([FromQuery] int? take, CancellationToken ct)
        {
            var user = User.Identity!.Name!;
            logger.LogDebug($"Received request from {user} for saved game records, take: {take}");

            var records = await gameManager.GetGameRecords(user, take, ct);

            logger.LogDebug($"Returning {records.Count()} records");
            return mapper.Map<IEnumerable<GameRecordDTO>>(records);
        }

        // POST api/play
        [HttpPost("play")]
        public async Task<GameRecordDTO> PostGameRecord([FromBody] GameRecordDTO command, CancellationToken ct)
        {
            var user = User.Identity!.Name!;
            logger.LogDebug($"Received request for a new game for user {user} with shape: {command.PlayerChoice}");
            new PostGameRecordValidator(gameManager.IsValidShapeId).Validate(command);

            var newGameRecord = mapper.Map<GameRecord>(command);
            newGameRecord.User = user;

            var record = await gameManager.Play(newGameRecord, ct);

            logger.LogDebug($"A game has been played with shape {record.PlayerChoice} against {record.ComputerChoice}, " +
                            $"Result: {record.Result}");
            return mapper.Map<GameRecordDTO>(record);
        }

        // DELETE api/play/{id}
        [HttpDelete("play/{id}")]
        public async Task DeleteGameRecord([FromRoute] string id, CancellationToken ct)
        {
            var user = User.Identity!.Name!;
            logger.LogDebug($"Received request from {user} to delete a game records: {id}");
            new DeleteGameRecordValidator().Validate(id);

            var deleted = await gameManager.DeleteGameRecords(user, long.Parse(id), ct);
            if (!deleted)
                throw new HttpNotFoundException("Game record not found");
        }

        // DELETE api/play
        [HttpDelete("play")]
        public async Task DeleteGameRecords(CancellationToken ct)
        {
            var user = User.Identity!.Name!;
            logger.LogDebug($"Received request from {user} to delete all game records");

            await gameManager.DeleteGameRecords(user, null, ct);
        }
    }
}
