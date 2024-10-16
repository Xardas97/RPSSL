using Microsoft.EntityFrameworkCore;

namespace Mmicovic.RPSSL.Service.Models
{
    public interface IGameRecordRepository
    {
        Task DeleteGameRecords(string user);
        Task<bool> DeleteGameRecord(string user, long id, CancellationToken ct);

        Task SaveGameRecord(GameRecord gameRecord);

        Task<IEnumerable<GameRecord>> GetGameRecords(string user, int? take, CancellationToken ct);
    }

    public class GameRecordRepository(GameRecordContext gameRecordContext) : IGameRecordRepository
    {
        private readonly GameRecordContext gameRecordContext = gameRecordContext;

        public async Task<IEnumerable<GameRecord>> GetGameRecords(string user, int? take, CancellationToken ct)
        {
            if (!take.HasValue)
                return await gameRecordContext.GameRecords.ToListAsync(ct);

            // Only include records made for this user.
            // Take latest {take} records. The newer once have a higher ID, so sort by ID in desc order.
            // This is not the best solution, replace with {AddedDate} field.
            return await gameRecordContext.GameRecords
                                .Where(r => string.Equals(r.User, user))
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

        public async Task<bool> DeleteGameRecord(string user, long id, CancellationToken ct)
        {
            // Only delete if the record exists and was made for this user
            var gameRecord = await gameRecordContext.GameRecords.FindAsync(id, ct);
            if (gameRecord is null || !string.Equals(gameRecord.User, user))
                return false;

            // We don't want to interrupt saving to the DB, so we don't use a cancellation token
            gameRecordContext.GameRecords.Remove(gameRecord);
            await gameRecordContext.SaveChangesAsync(CancellationToken.None);
            return true;
        }

        public async Task DeleteGameRecords(string user)
        {
            // Remove all records made for this user
            foreach (var gameRecord in gameRecordContext.GameRecords.Where(r => string.Equals(r.User, user)))
            {
                gameRecordContext.GameRecords.Remove(gameRecord);
            }

            // We don't want to interrupt saving to the DB, so we don't use a cancellation token
            await gameRecordContext.SaveChangesAsync(CancellationToken.None);
        }
    }
}
