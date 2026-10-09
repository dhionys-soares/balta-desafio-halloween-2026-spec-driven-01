using TimeTracking.Domain;

namespace TimeTracking.Tests;

public class TimeRecordSequenceValidatorTests
{
    [Fact]
    public void GetNextExpectedType_WhenNoRecords_ReturnsEntry()
    {
        var result = TimeRecordSequenceValidator.GetNextExpectedType(Array.Empty<TimeRecord>());

        Assert.Equal(TimeRecordType.Entry, result);
    }

    [Fact]
    public void ValidateNextRecord_WhenSequenceIsValid_DoesNotThrow()
    {
        var existing = new[]
        {
            new TimeRecord { RecordType = TimeRecordType.Entry, RecordedAt = new DateTime(2026, 10, 9, 8, 0, 0, DateTimeKind.Utc) }
        };

        var next = new TimeRecord
        {
            RecordType = TimeRecordType.BreakStart,
            RecordedAt = new DateTime(2026, 10, 9, 9, 0, 0, DateTimeKind.Utc)
        };

        var exception = Record.Exception(() => TimeRecordSequenceValidator.ValidateNextRecord(existing, next));

        Assert.Null(exception);
    }

    [Fact]
    public void ValidateNextRecord_WhenSequenceIsInvalid_ThrowsInvalidOperationException()
    {
        var existing = new[]
        {
            new TimeRecord { RecordType = TimeRecordType.Entry, RecordedAt = new DateTime(2026, 10, 9, 8, 0, 0, DateTimeKind.Utc) }
        };

        var next = new TimeRecord
        {
            RecordType = TimeRecordType.Entry,
            RecordedAt = new DateTime(2026, 10, 9, 8, 30, 0, DateTimeKind.Utc)
        };

        Assert.Throws<InvalidOperationException>(() => TimeRecordSequenceValidator.ValidateNextRecord(existing, next));
    }
}

public class AbsencePeriodValidatorTests
{
    [Fact]
    public void Validate_WhenEndDateIsBeforeOrEqualToStart_Throws()
    {
        var absence = new AbsencePeriod
        {
            UserId = Guid.NewGuid(),
            StartAt = new DateTime(2026, 10, 10, 9, 0, 0, DateTimeKind.Utc),
            EndAt = new DateTime(2026, 10, 10, 9, 0, 0, DateTimeKind.Utc),
            Reason = "Consulta médica"
        };

        Assert.Throws<InvalidOperationException>(() => AbsencePeriodValidator.Validate(absence));
    }
}
