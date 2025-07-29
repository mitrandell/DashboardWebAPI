using DashboardWebAPI.DataTransferObjects;
using DashboardWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DashboardWebAPI.Data
{
    public class DAL : IDAL
    {
        private readonly ApplicationDbContext _db;
        
        public DAL(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> AddTaskDataAsync(List<TaskData> taskData)
        {

            var uniqueTaskKeys = taskData.Select(x => x.TaskNumber).ToList();

            var existingTaskData = await _db.TaskSet.Where(x => uniqueTaskKeys.Contains(x.TaskNumber))
                .Select(x => x.TaskNumber)
                .ToListAsync();

            var newTaskData = taskData.Where(x => !existingTaskData.Contains(x.TaskNumber)).ToList();

            if (newTaskData.Any())
            {
                await _db.TaskSet.AddRangeAsync(newTaskData);
                await _db.SaveChangesAsync();
            }

            return true;
        }

        public async Task<List<TaskDataDTO>> GetTaskDataAsync(string date)
        {
            DateTime dateTime = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

            return await _db.TaskSet.Where(x => x.StartTaskDate > dateTime)
                .OrderBy(x => x.StartTaskDate)
                .Select(x => new TaskDataDTO
                {
                    TaskNumber = x.TaskNumber,
                    Status = x.Status,
                    InitiatorName = x.InitiatorName,
                    Title = x.Title,
                    ExecutorName = x.ExecutorName,
                    SystemSectionName = x.SystemSectionName,
                    ExecutedTime = x.ExecutedTime,
                    StartTaskDate = x.StartTaskDate.ToString("yyyy-MM-dd"),
                    EndTaskDate = x.EndTaskDate == null ? " " : x.EndTaskDate.Value.ToString("yyyy-MM-dd")
                }).ToListAsync();
        }
        public async Task<bool> AddCriticalTaskDataAsync(CriticalTask taskData)
        {
            await _db.CriticalTaskSet.AddAsync(taskData);
            await _db.SaveChangesAsync();

            return true;
        }
        public async Task<bool> EditCriticalTaskDataAsync(CriticalTask taskData)
        {
            _db.Attach(taskData);
            _db.Entry(taskData).State = EntityState.Modified;

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<List<CriticalTask>> GetCriticalTaskDataAsync()
        {
            return await _db.CriticalTaskSet.Where(e => e.ActionStatus == "В процессе").OrderBy(x => x.StartDate).ToListAsync();
        }

        public async Task<List<DeveloperTask>> GetDeveloperTaskDataAsync()
        {
            return await _db.DeveloperTaskSet.Where(e => e.ActionStatus == "В процессе").OrderBy(x => x.StartDate).ToListAsync();
        }

        public async Task<bool> AddDeveloperTaskDataAsync(DeveloperTask taskData)
        {
            await _db.DeveloperTaskSet.AddAsync(taskData);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditDeveloperTaskDataAsync(DeveloperTask taskData)
        {
            _db.Attach(taskData);
            _db.Entry(taskData).State = EntityState.Modified;

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<List<BussinessDay>> GetBussinessDaysAsync(int id)
        {
            var dayType = await _db.BussinessDayTypeSet.Where(type => type.Id == id).FirstOrDefaultAsync();

            return await _db.BussinessDaySet.Where(day => day.Type == dayType).ToListAsync();
        }

        public async Task<List<BussinessDayDTO>> GetBussinessDaysForMonthAsync(int year, int month)
        {
            var bussinessDays = await _db.BussinessDaySet.Where(x => x.Date.Year == year && x.Date.Month == month)
                .Order()
                .Select(x => new BussinessDayDTO
                {
                    Id = x.Id,
                    Date = x.Date.ToString("dd/MM/yyyy"),
                    TypeId = x.Type.Id
                })
                .ToListAsync();

            return bussinessDays;
        }

        public async Task<bool> EditBussinessDayAsync(EditBussinessDayDTO day)
        {
            var bussinessDay = await _db.BussinessDaySet.FirstOrDefaultAsync(x => x.Id == day.Id);
            if (bussinessDay == null)
            {
                return false;
            }

            var dayType = await _db.BussinessDayTypeSet.FirstOrDefaultAsync(x => x.Id == day.TypeId);
            if (dayType == null)
            {
                return false;
            }

            bussinessDay.Type = dayType;

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<List<int>> GetBussinessDaysYearsAsync()
        {
            var years = await _db.BussinessDaySet.Select(date => date.Date.Year)
                .Distinct()
                .ToListAsync();

            return years;
        }

        public async Task<User> GetUserAsync(UserLoginDTO userCredentials)
        { 
            return await _db.UserSet.FirstOrDefaultAsync(x => x.Login.Equals(userCredentials.Login));
        }

        public async Task AddNoteAsync(Note note)
        {
            await _db.NoteSet.AddAsync(note);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Note>> GetNotesAsync()
        {
            return await _db.NoteSet.ToListAsync();
        }

        public async Task EditNoteAsync(Note note)
        {
            _db.NoteSet.Attach(note);
            _db.Entry(note).State = EntityState.Modified;

            await _db.SaveChangesAsync();
        }

        public async Task<bool> DeleteNoteAsync(long id)
        {
            var note = await _db.NoteSet.FirstOrDefaultAsync(x => x.Id == id);
            if(note == null)
            {
                return false;
            }

            _db.Remove(note);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
