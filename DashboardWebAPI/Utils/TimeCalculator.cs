using DashboardWebAPI.Models;

namespace DashboardWebAPI.Utils
{
    public class TimeCalculator
    {
        public static TimeSpan CalculatingTaskCompletionTime(DateTime startTaskDate, DateTime endTaskDate, 
            List<BussinessDay> weekends)
        {
            var startBussinessHours = DateTime.Parse("08:00:00").TimeOfDay;
            var endBussinessHours = DateTime.Parse("17:00:00").TimeOfDay;

            var weekendDates = weekends.Select(x => x.Date).ToList();  

            TimeSpan totalHoursTaskCompletion = TimeSpan.Zero;

            if(endTaskDate == null)
            {
                return totalHoursTaskCompletion;
            }

            SetDateTimeForTasksNotInBussinessHoursInterval(ref startTaskDate, ref endTaskDate, startBussinessHours, endBussinessHours);

            while (weekendDates.Contains(startTaskDate.Date))
            {
                startTaskDate = IncreaseDateTime(startTaskDate.Year, startTaskDate.Month, startTaskDate.Day);
            }
            
            if(startTaskDate.Date > endTaskDate.Date && weekendDates.Contains(endTaskDate.Date))
            {
                while (weekendDates.Contains(endTaskDate.Date))
                {
                    endTaskDate = IncreaseDateTime(endTaskDate.Year, endTaskDate.Month, endTaskDate.Day);
                }
            }

            if (startTaskDate.Date < endTaskDate.Date && weekendDates.Contains(endTaskDate.Date))
            {
                while (weekendDates.Contains(endTaskDate.Date))
                {
                    endTaskDate = DecreaseDateTime(endTaskDate.Year, endTaskDate.Month, endTaskDate.Day);
                }
            }

            if (startTaskDate.Date == endTaskDate.Date)
            {
                totalHoursTaskCompletion += endTaskDate - startTaskDate;

                return totalHoursTaskCompletion;
            }
            
            if (startTaskDate.Date < endTaskDate.Date)
            {
                while (endTaskDate > startTaskDate)
                {
                    if (weekendDates.Contains(endTaskDate.Date) && endTaskDate.Date > startTaskDate.Date)
                    {
                        endTaskDate = DecreaseDateTime(endTaskDate.Year, endTaskDate.Month, endTaskDate.Day);
                    }

                    if (endTaskDate.TimeOfDay >= startBussinessHours && endTaskDate.TimeOfDay <= endBussinessHours
                        && !weekendDates.Contains(endTaskDate.Date))
                    {
                        if (endTaskDate.Date > startTaskDate.Date)
                        {
                            totalHoursTaskCompletion += endTaskDate.TimeOfDay - startBussinessHours;

                            endTaskDate = DecreaseDateTime(endTaskDate.Year, endTaskDate.Month, endTaskDate.Day);
                        }

                        if (endTaskDate.Date == startTaskDate.Date)
                        {
                            totalHoursTaskCompletion += endTaskDate - startTaskDate;

                            endTaskDate = DecreaseDateTime(endTaskDate.Year, endTaskDate.Month, endTaskDate.Day);
                        }
                    }
                }
            }

            return totalHoursTaskCompletion;
        }

        private static DateTime DecreaseDateTime(int year, int month, int day)
        {
            int newYear = month == 1 && day == 1 ? year - 1 : year;
            int newMonth = day == 1 && month > 1 ? month - 1 : month;
            if(day == 1 && month == 1)
            {
                newMonth = 12;
            }
            
            int newDay = day == 1 ? DateTime.DaysInMonth(newYear, newMonth) : day - 1;

            return new DateTime(newYear, newMonth, newDay, 17, 0, 0);
        }

        private static DateTime IncreaseDateTime(int year, int month, int day)
        {
            int newYear = month == 12 && day == DateTime.DaysInMonth(year, month) ? year + 1 : year;
            int newMonth = month < 12 && day == DateTime.DaysInMonth(year, month) ? month + 1 : month;
            if (month == 12 && day == DateTime.DaysInMonth(year, month))
            {
                newMonth = 1;
            }

            int newDay = day == DateTime.DaysInMonth(year, month) ? 1 : day + 1;

            return new DateTime(newYear, newMonth, newDay, 8, 0, 0);
        }

        private static DateTime SetDateTime(int year, int month, int day, int hour)
        {
            return new DateTime(year, month, day, hour, 0, 0);
        }

        private static void SetDateTimeForTasksNotInBussinessHoursInterval(ref DateTime startTaskDate, ref DateTime endTaskDate, TimeSpan startBussinessHours, TimeSpan endBussinessHours)
        {
            if (startTaskDate.TimeOfDay < startBussinessHours)
            {
                startTaskDate = SetDateTime(startTaskDate.Year, startTaskDate.Month, startTaskDate.Day, startBussinessHours.Hours);
            }

            if (startTaskDate.TimeOfDay > endBussinessHours)
            {
                startTaskDate = IncreaseDateTime(startTaskDate.Year, startTaskDate.Month, startTaskDate.Day);
            }

            if (endTaskDate.TimeOfDay > endBussinessHours)
            {
                endTaskDate = SetDateTime(endTaskDate.Year, endTaskDate.Month, endTaskDate.Day, endBussinessHours.Hours);
            }

            if (endTaskDate.TimeOfDay < startBussinessHours)
            {
                endTaskDate = DecreaseDateTime(endTaskDate.Year, endTaskDate.Month, endTaskDate.Day);
            }
        }
    }
}
