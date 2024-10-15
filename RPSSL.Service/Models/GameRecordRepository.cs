using Microsoft.EntityFrameworkCore;

namespace Mmicovic.RPSSL.Service.Models
{
    public interface IGameRecordRepository
    {
        Task DeleteGameRecords();
        Task SaveGameRecord(GameRecord gameRecord);
        Task<IEnumerable<GameRecord>> GetGameRecords(int? take, CancellationToken ct);
    }

    public class GameRecordRepository(GameRecordContext gameRecordContext) : IGameRecordRepository
    {
        private readonly GameRecordContext gameRecordContext = gameRecordContext;

        public async Task<IEnumerable<GameRecord>> GetGameRecords(int? take, CancellationToken ct)
        {
            if (!take.HasValue)
                return await gameRecordContext.GameRecords.ToListAsync(ct);

            // Take latest {take} records. The newer once have a higher ID, so sort by ID in desc order
            // This is not the best solution, replace with {AddedDate} field.
            return await gameRecordContext.GameRecords
                                .OrderByDescending(r => r.Id)
                                .Take(take.Value)
                                .ToListAsync(ct);
        }

        public async Task SaveGameRecord(GameRecord gameRecord)
        {
            // We don't want to interrupt saving to the DB, so we don't use a cancellation token
            gameRecordContext.GameRecords.Add(gameRecord);
            await gameRecordContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task DeleteGameRecords()
        {
            // There is no RemoveAll method. We could wipe the whole DB with a direct SQL query
            // but do it like this instead so that it can be easily expanded with a filter.
            foreach (var gameRecord in gameRecordContext.GameRecords)
            {
                gameRecordContext.GameRecords.Remove(gameRecord);
            }

            // We don't want to interrupt saving to the DB, so we don't use a cancellation token
            await gameRecordContext.SaveChangesAsync(CancellationToken.None);
        }
    }
}
