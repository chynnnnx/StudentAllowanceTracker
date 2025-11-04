using StudentAllowanceTracker.Client.DTOs;

namespace StudentAllowanceTracker.Client.Services.Interfaces
{
    public interface IAllowanceService
    {
        Task<bool> AddAllowance(AllowanceDTO allowanceDTO);
        Task<List<AllowanceDTO>?> GetAllowanceByUser();
        Task<AllowanceDTO?> UpdateAllowance(AllowanceDTO allowance);
        Task DeleteAllowance(Guid allowanceID);
    }
}
