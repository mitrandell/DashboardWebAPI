using ClosedXML.Excel;
using DashboardWebAPI.Data;
using DashboardWebAPI.DataTransferObjects;
using DashboardWebAPI.Models;
using DashboardWebAPI.Utils;
using DotNetEnv;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

Env.Load("./environments.env");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING") ??
        throw new InvalidOperationException("Connection string 'postgresql' not found.")));

builder.Services.AddScoped<IDAL, DAL>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin()
    .WithOrigins("http://localhost:4200", "http://127.0.0.1:4200", "http://46.0.24.72:4200", "http://46.0.24.72:4200")
    .AllowAnyMethod()
    .AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var culture = new CultureInfo("ru-RU");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;
Thread.CurrentThread.CurrentCulture = culture;
Thread.CurrentThread.CurrentUICulture = culture;

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.UseCors("CorsPolicy");

app.MapPost("/api/upload", async (IFormFile file, IDAL dal) =>
{
    try
    {
        if (!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            return Results.BadRequest("Поддерживаются только файлы с расширением .xlsx");
        }

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);

        using var workBook = new XLWorkbook(memoryStream);

        var workSheet = workBook.Worksheet(1);

        var headerRow = workSheet.FirstRowUsed();
        var expectedHeaders = new List<string>
            {
                "ID",
                "Статус",
                "Инициатор запроса",
                "Тема",
                "Создано (когда)",
                "Участник",
                "Завершено"
            };

        for (int i = 0; i < expectedHeaders.Count; i++)
        {
            if (headerRow.Cell(i + 1).GetValue<string>() != expectedHeaders[i])
            {
                return Results.BadRequest($"Неверный заголовок в столбце {i + 1}. Ожидается: {expectedHeaders[i]}");
            }
        }


        var weekends = await dal.GetBussinessDaysAsync(2);
        if (weekends.Count == 0)
        {
            throw new ApplicationException("Не заполнен график рабочих дней");
        }

        var data = workSheet.RowsUsed()
            .Skip(1)
            .Select(row => new TaskData
            {
                TaskNumber = row.Cell(1).GetValue<int>(),
                Status = row.Cell(2).GetValue<string>(),
                InitiatorName = row.Cell(3).GetValue<string>(),
                Title = row.Cell(4).GetValue<string>(),
                StartTaskDate = DateTime.Parse(row.Cell(5).GetValue<string>().Split(", ")[1].Split("MSK")[0]),
                ExecutorName = row.Cell(6).GetValue<string>(),
                SystemSectionName = row.Cell(4).GetValue<string>().Contains('.')
                    ? row.Cell(4).GetValue<string>().Split('.')[0]
                    : "НЕ УКАЗАН РАЗДЕЛ СИСТЕМЫ",
                EndTaskDate = !string.IsNullOrEmpty(row.Cell(7).GetValue<string>())
                    ? DateTime.Parse(row.Cell(7).GetValue<string>().Split(", ")[1].Split("MSK")[0])
                    : null,
                ExecutedTime = !string.IsNullOrEmpty(row.Cell(7).GetValue<string>())
                    ? TimeCalculator.CalculatingTaskCompletionTime(DateTime.Parse(row.Cell(5).GetValue<string>().Split(", ")[1].Split("MSK")[0]),
                    DateTime.Parse(row.Cell(7).GetValue<string>().Split(", ")[1].Split("MSK")[0]), weekends)
                    : null
            }).ToList();

        await dal.AddTaskDataAsync(data);

        return Results.Ok();
    }
    catch (Exception ex)
    {

        return Results.BadRequest(ex.Message);
    }
})
.Accepts<IFormFile>("multipart/form-data")
.DisableAntiforgery();

app.MapGet("/api/GetTasks/{date}", async (long date, IDAL dal) =>
{
    try
    {
        var tasks = await dal.GetTaskDataAsync(date);
        return Results.Ok(tasks);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPost("/api/AddCriticalTask", async ([FromBody] CriticalTask task, IDAL dal) =>
{
    try
    {
        if (task.TaskNumber > double.MaxValue)
        {
            return Results.BadRequest("Число слишком большое");
        }
        var tasks = await dal.AddCriticalTaskDataAsync(task);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/api/EditCriticalTask", async ([FromBody] CriticalTask task, IDAL dal) =>
{
    try
    {
        var tasks = await dal.EditCriticalTaskDataAsync(task);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/api/GetCriticalTasks", async (IDAL dal) =>
{
    try
    {
        var tasks = await dal.GetCriticalTaskDataAsync();

        return Results.Ok(tasks);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/api/GetDeveloperTasks", async (IDAL dal) =>
{
    try
    {
        var tasks = await dal.GetDeveloperTaskDataAsync();

        return Results.Ok(tasks);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPost("/api/AddDeveloperTask", async ([FromBody] DeveloperTask task, IDAL dal) =>
{
    try
    {
        var tasks = await dal.AddDeveloperTaskDataAsync(task);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/api/EditDeveloperTask", async ([FromBody] DeveloperTask task, IDAL dal) =>
{
    try
    {
        var tasks = await dal.EditDeveloperTaskDataAsync(task);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/api/GetBussinessDays/year={year}&month={month}", async (int year, int month, IDAL dal) =>
{
    try
    {
        var bussinessDays = await dal.GetBussinessDaysForMonthAsync(year, month);
        return Results.Ok(bussinessDays);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/api/GetBussinessDaysYears", async (IDAL dal) =>
{
    try
    {
        var years = await dal.GetBussinessDaysYearsAsync();
        return Results.Ok(years);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/api/EditBussinessDay", async ([FromBody] EditBussinessDayDTO day, IDAL dal) =>
{
    try
    {
        var result = await dal.EditBussinessDayAsync(day);
        if (!result)
        {
            return Results.BadRequest("Рабочий день не найден");
        }

        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();
