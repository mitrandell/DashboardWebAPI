using DashboardWebAPI.DataTransferObjects;
using DashboardWebAPI.Models;

namespace DashboardWebAPI.Data
{
    public interface IDAL
    {
        //tasks
        public Task<List<TaskDataDTO>> GetTaskDataAsync(string date);
        public Task<bool> AddTaskDataAsync(List<TaskData> taskData);
        public Task<List<CriticalTask>> GetCriticalTaskDataAsync();
        public Task<bool> AddCriticalTaskDataAsync(CriticalTask taskData);
        public Task<bool> EditCriticalTaskDataAsync(CriticalTask taskData);
        public Task<List<DeveloperTask>> GetDeveloperTaskDataAsync();
        public Task<bool> AddDeveloperTaskDataAsync(DeveloperTask taskData);
        public Task<bool> EditDeveloperTaskDataAsync(DeveloperTask taskData);

        //bussiness days
        public Task<List<BussinessDay>> GetBussinessDaysAsync(int id);
        public Task<List<BussinessDayDTO>> GetBussinessDaysForMonthAsync(int year, int month);
        public Task<bool> EditBussinessDayAsync(EditBussinessDayDTO day);
        public Task<List<int>> GetBussinessDaysYearsAsync();

        //acount
        public Task<User> GetUserAsync(UserLoginDTO userCredentials);

        //notes
        public Task AddNoteAsync(Note note);
        public Task<List<Note>> GetNotesAsync();
        public Task EditNoteAsync(Note note);
        public Task<bool> DeleteNoteAsync(long id);

        //script notes
        public Task AddScriptNoteAsync(ScriptNote scriptNote);
        public Task<List<ScriptNote>> GetScriptNotesAsync();
        public Task EditScriptNotesAsync(ScriptNote scriptNote);
        public Task DeleteScriptNoteAsync(long id);
    }
}
