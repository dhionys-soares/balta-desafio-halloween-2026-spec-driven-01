using TimeTracking.Application;
using TimeTracking.Domain;

namespace TimeTracking.Tests;

public class TimeRecordApplicationServiceTests
{
    [Fact]
    public void RegisterTimeRecord_WhenSequenceIsValid_ReturnsNextRecord()
    {
        var user = new User { Id = Guid.NewGuid(), UserName = "maria", Role = UserRole.Employee };
        var baseDate = new DateTime(2026, 10, 9, 8, 0, 0, DateTimeKind.Utc);
        var service = new TimeRecordApplicationService();

        var entry = service.RegisterTimeRecord(user, Array.Empty<TimeRecord>(), baseDate);
        var breakStart = service.RegisterTimeRecord(user, new[] { entry }, baseDate.AddHours(1));

        Assert.Equal(TimeRecordType.Entry, entry.RecordType);
        Assert.Equal(TimeRecordType.BreakStart, breakStart.RecordType);
    }

    [Fact]
    public void RegisterTimeRecord_WhenSequenceIsInvalid_Throws()
    {
        var user = new User { Id = Guid.NewGuid(), UserName = "maria", Role = UserRole.Employee };
        var service = new TimeRecordApplicationService();
        var invalidHistory = new[]
        {
            new TimeRecord { UserId = user.Id, RecordType = TimeRecordType.Entry, RecordedAt = new DateTime(2026, 10, 9, 8, 0, 0, DateTimeKind.Utc) },
            new TimeRecord { UserId = user.Id, RecordType = TimeRecordType.BreakStart, RecordedAt = new DateTime(2026, 10, 9, 9, 0, 0, DateTimeKind.Utc) }
        };

        Assert.Throws<InvalidOperationException>(() => service.RegisterTimeRecord(user, invalidHistory, new DateTime(2026, 10, 9, 9, 0, 0, DateTimeKind.Utc)));
    }
}

public class AbsenceApplicationServiceTests
{
    [Fact]
    public void RegisterAbsence_WhenDataIsValid_ReturnsAbsencePeriod()
    {
        var user = new User { Id = Guid.NewGuid(), UserName = "maria", Role = UserRole.Employee };
        var service = new AbsenceApplicationService();

        var absence = service.RegisterAbsence(user, new DateTime(2026, 10, 11, 9, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 11, 11, 0, 0, DateTimeKind.Utc), "Consulta médica");

        Assert.Equal(user.Id, absence.UserId);
        Assert.Equal("Consulta médica", absence.Reason);
    }
}

public class WorkdaySummaryServiceTests
{
    [Fact]
    public void GetSummary_WhenJourneyIsCompleted_ReturnsWorkedHours()
    {
        var userId = Guid.NewGuid();
        var records = new[]
        {
            new TimeRecord { UserId = userId, RecordType = TimeRecordType.Entry, RecordedAt = new DateTime(2026, 10, 9, 8, 0, 0, DateTimeKind.Utc) },
            new TimeRecord { UserId = userId, RecordType = TimeRecordType.BreakStart, RecordedAt = new DateTime(2026, 10, 9, 12, 0, 0, DateTimeKind.Utc) },
            new TimeRecord { UserId = userId, RecordType = TimeRecordType.BreakEnd, RecordedAt = new DateTime(2026, 10, 9, 13, 0, 0, DateTimeKind.Utc) },
            new TimeRecord { UserId = userId, RecordType = TimeRecordType.Exit, RecordedAt = new DateTime(2026, 10, 9, 17, 0, 0, DateTimeKind.Utc) }
        };

        var service = new WorkdaySummaryService();
        var summary = service.GetSummary(records);

        Assert.True(summary.IsCompleted);
        Assert.Equal(TimeSpan.FromHours(8), summary.WorkedHours);
    }

    [Fact]
    public void GetSummary_WhenJourneyIsIncomplete_ReturnsZeroHours()
    {
        var userId = Guid.NewGuid();
        var records = new[]
        {
            new TimeRecord { UserId = userId, RecordType = TimeRecordType.Entry, RecordedAt = new DateTime(2026, 10, 9, 8, 0, 0, DateTimeKind.Utc) },
            new TimeRecord { UserId = userId, RecordType = TimeRecordType.BreakStart, RecordedAt = new DateTime(2026, 10, 9, 12, 0, 0, DateTimeKind.Utc) }
        };

        var service = new WorkdaySummaryService();
        var summary = service.GetSummary(records);

        Assert.False(summary.IsCompleted);
        Assert.Equal(TimeSpan.Zero, summary.WorkedHours);
    }
}
