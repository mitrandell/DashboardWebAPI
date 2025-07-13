using DashboardWebAPI.DataTransferObjects;
using DashboardWebAPI.Models;

namespace DashboardWebAPI.Data
{
    public interface IDAL
    {
        public Task<bool> AddTaskDataAsync(List<TaskData> taskData);
        public Task<List<TaskDataDTO>> GetTaskDataAsync(long date);
        public Task<List<BussinessDay>> GetBussinessDaysAsync(int id);
        public Task<bool> AddCriticalTaskDataAsync(CriticalTask taskData);
        public Task<bool> EditCriticalTaskDataAsync(CriticalTask taskData);
        public Task<List<CriticalTask>> GetCriticalTaskDataAsync();
        public Task<List<DeveloperTask>> GetDeveloperTaskDataAsync();
        public Task<bool> AddDeveloperTaskDataAsync(DeveloperTask taskData);
        public Task<bool> EditDeveloperTaskDataAsync(DeveloperTask taskData);
        public Task<List<BussinessDayDTO>> GetBussinessDaysForMonthAsync(int year, int month);
        public Task<bool> EditBussinessDayAsync(EditBussinessDayDTO day);
        public Task<List<int>> GetBussinessDaysYearsAsync();
        public Task<User> GetUserAsync(UserLoginDTO userCredentials);
    }
}
