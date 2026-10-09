using TimeTracking.Application;
using TimeTracking.Domain;

namespace TimeTracking.Api;

public sealed record TimeRecordRequest(DateTime RecordedAt);

public sealed record TimeRecordResponse(Guid Id, Guid UserId, string RecordType, DateTime RecordedAt);

public sealed record AbsenceRequest(DateTime StartAt, DateTime EndAt, string Reason);

public sealed record AbsenceResponse(Guid Id, Guid UserId, DateTime StartAt, DateTime EndAt, string Reason);

public static class TimeRecordEndpoints
{
    public static TimeRecordResponse MapResponse(TimeRecord timeRecord)
    {
        ArgumentNullException.ThrowIfNull(timeRecord);

        return new TimeRecordResponse(
            timeRecord.Id,
            timeRecord.UserId,
            timeRecord.RecordType.ToString(),
            timeRecord.RecordedAt);
    }

    public static IResult Create(TimeRecordRequest request)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "demo-user",
            Email = "demo@local",
            Role = UserRole.Employee
        };

        var service = new TimeRecordApplicationService();
        var record = service.RegisterTimeRecord(user, Array.Empty<TimeRecord>(), request.RecordedAt);

        return Results.Created($"/api/time-records/{record.Id}", MapResponse(record));
    }
}

public static class AbsenceEndpoints
{
    public static AbsenceResponse MapResponse(AbsencePeriod absencePeriod)
    {
        ArgumentNullException.ThrowIfNull(absencePeriod);

        return new AbsenceResponse(
            absencePeriod.Id,
            absencePeriod.UserId,
            absencePeriod.StartAt,
            absencePeriod.EndAt,
            absencePeriod.Reason);
    }

    public static IResult Create(AbsenceRequest request)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "demo-user",
            Email = "demo@local",
            Role = UserRole.Employee
        };

        var service = new AbsenceApplicationService();
        var absence = service.RegisterAbsence(user, request.StartAt, request.EndAt, request.Reason);

        return Results.Created($"/api/absences/{absence.Id}", MapResponse(absence));
    }
}
