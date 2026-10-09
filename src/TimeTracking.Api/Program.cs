using TimeTracking.Api;
using TimeTracking.Domain;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => Results.Ok(new { status = "ok" }));
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapPost("/api/time-records", (TimeRecordRequest request) => TimeRecordEndpoints.Create(request));
app.MapGet("/api/time-records", () => Results.Ok(new[]
{
    new TimeRecordResponse(Guid.NewGuid(), Guid.NewGuid(), TimeRecordType.Entry.ToString(), DateTime.UtcNow)
}));

app.MapPost("/api/absences", (AbsenceRequest request) => AbsenceEndpoints.Create(request));
app.MapGet("/api/absences", () => Results.Ok(new[]
{
    new AbsenceResponse(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow.AddHours(2), "Consulta médica")
}));

app.Run();
