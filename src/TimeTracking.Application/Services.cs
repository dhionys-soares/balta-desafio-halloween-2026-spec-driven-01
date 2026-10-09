using TimeTracking.Domain;

namespace TimeTracking.Application;

public sealed class TimeRecordApplicationService
{
    public TimeRecord RegisterTimeRecord(User user, IEnumerable<TimeRecord> existingRecords, DateTime recordedAt)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(existingRecords);

        var ordered = existingRecords
            .OrderBy(record => record.RecordedAt)
            .ToList();

        var nextExpectedType = TimeRecordSequenceValidator.GetNextExpectedType(ordered);
        var newRecord = new TimeRecord
        {
            UserId = user.Id,
            RecordType = nextExpectedType,
            RecordedAt = recordedAt,
            CreatedAt = DateTime.UtcNow
        };

        TimeRecordSequenceValidator.ValidateNextRecord(ordered, newRecord);

        return newRecord;
    }
}

public sealed class AbsenceApplicationService
{
    public AbsencePeriod RegisterAbsence(User user, DateTime startAt, DateTime endAt, string reason)
    {
        ArgumentNullException.ThrowIfNull(user);

        var absence = new AbsencePeriod
        {
            UserId = user.Id,
            StartAt = startAt,
            EndAt = endAt,
            Reason = reason,
            CreatedAt = DateTime.UtcNow
        };

        AbsencePeriodValidator.Validate(absence);

        return absence;
    }
}

public sealed class WorkdaySummaryService
{
    public WorkdaySummary GetSummary(IEnumerable<TimeRecord> records)
    {
        var ordered = records
            .OrderBy(record => record.RecordedAt)
            .ToList();

        if (!TimeRecordSequenceValidator.IsJourneyComplete(ordered))
        {
            return new WorkdaySummary(false, TimeSpan.Zero);
        }

        return new WorkdaySummary(true, WorkdayCalculator.CalculateWorkedHours(ordered));
    }
}

public sealed record WorkdaySummary(bool IsCompleted, TimeSpan WorkedHours);
