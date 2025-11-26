using StudentAllowanceTracker.Client.DTOs;

namespace StudentAllowanceTracker.Client.Services.Interfaces
{
    public interface IHistoryService
    {
        Task<bool> AddHistory(HistoryDTO historyDTO);
        Task<List<HistoryDTO>?> GetHistories(string? type = null);
        Task<HistoryDTO?> GetHistoryById(Guid id);
        Task<bool> DeleteHistory(Guid id);
    }
}
