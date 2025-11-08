using StudentAllowanceTracker.Client.DTOs;

namespace StudentAllowanceTracker.Client.Services.Interfaces
{
    public interface IGoalService
    {
        Task<bool> AddGoal(GoalDTO goalDTO);
        Task<GoalDTO?> UpdateGoal(GoalDTO goal);
        Task<List<GoalDTO>?> GetGoalsByUser();
        Task DeleteGoal(Guid goalID);
    }
}
