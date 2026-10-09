namespace TimeTracking.Domain;

public enum UserRole
{
    Employee = 1,
    Administrator = 2
}

public sealed class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Employee;
}

public enum TimeRecordType
{
    Entry = 1,
    BreakStart = 2,
    BreakEnd = 3,
    Exit = 4
}

public sealed class TimeRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public TimeRecordType RecordType { get; set; }
    public DateTime RecordedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public sealed class AbsencePeriod
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public static class TimeRecordSequenceValidator
{
    public static bool IsValidTransition(TimeRecordType? previousType, TimeRecordType nextType)
    {
        if (previousType is null)
        {
            return nextType == TimeRecordType.Entry;
        }

        return previousType switch
        {
            TimeRecordType.Entry => nextType == TimeRecordType.BreakStart,
            TimeRecordType.BreakStart => nextType == TimeRecordType.BreakEnd,
            TimeRecordType.BreakEnd => nextType == TimeRecordType.Exit,
            TimeRecordType.Exit => false,
            _ => false
        };
    }

    public static TimeRecordType GetNextExpectedType(IEnumerable<TimeRecord> records)
    {
        var ordered = records
            .OrderBy(record => record.RecordedAt)
            .ToList();

        if (ordered.Count == 0)
        {
            return TimeRecordType.Entry;
        }

        var lastType = ordered[^1].RecordType;

        return lastType switch
        {
            TimeRecordType.Entry => TimeRecordType.BreakStart,
            TimeRecordType.BreakStart => TimeRecordType.BreakEnd,
            TimeRecordType.BreakEnd => TimeRecordType.Exit,
            TimeRecordType.Exit => throw new InvalidOperationException("A jornada já foi concluída."),
            _ => throw new InvalidOperationException("Tipo de marcação inválido.")
        };
    }

    public static void ValidateNextRecord(IReadOnlyCollection<TimeRecord> existingRecords, TimeRecord newRecord)
    {
        ArgumentNullException.ThrowIfNull(existingRecords);
        ArgumentNullException.ThrowIfNull(newRecord);

        if (existingRecords.Count == 0 && newRecord.RecordType != TimeRecordType.Entry)
        {
            throw new InvalidOperationException("A primeira marcação da jornada deve ser a entrada.");
        }

        if (existingRecords.Count > 0)
        {
            var lastRecord = existingRecords
                .OrderBy(record => record.RecordedAt)
                .Last();

            if (newRecord.RecordedAt <= lastRecord.RecordedAt)
            {
                throw new InvalidOperationException("A nova marcação deve ocorrer após a última marcação registrada.");
            }

            var expectedType = GetNextExpectedType(existingRecords);
            if (expectedType != newRecord.RecordType)
            {
                throw new InvalidOperationException("A sequência de marcações não está correta.");
            }
        }
    }

    public static bool IsJourneyComplete(IEnumerable<TimeRecord> records)
    {
        var ordered = records
            .OrderBy(record => record.RecordedAt)
            .ToList();

        if (ordered.Count != 4)
        {
            return false;
        }

        var expectedSequence = new[]
        {
            TimeRecordType.Entry,
            TimeRecordType.BreakStart,
            TimeRecordType.BreakEnd,
            TimeRecordType.Exit
        };

        return ordered.Select(record => record.RecordType)
            .SequenceEqual(expectedSequence);
    }
}

public static class AbsencePeriodValidator
{
    public static void Validate(AbsencePeriod absencePeriod)
    {
        ArgumentNullException.ThrowIfNull(absencePeriod);

        if (absencePeriod.UserId == Guid.Empty)
        {
            throw new InvalidOperationException("O usuário associado ao período de ausência é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(absencePeriod.Reason))
        {
            throw new InvalidOperationException("O motivo da ausência é obrigatório.");
        }

        if (absencePeriod.EndAt <= absencePeriod.StartAt)
        {
            throw new InvalidOperationException("O término da ausência deve ser posterior ao início.");
        }
    }
}

public static class WorkdayCalculator
{
    public static TimeSpan CalculateWorkedHours(IEnumerable<TimeRecord> records)
    {
        var ordered = records
            .OrderBy(record => record.RecordedAt)
            .ToList();

        if (ordered.Count != 4 || !TimeRecordSequenceValidator.IsJourneyComplete(ordered))
        {
            return TimeSpan.Zero;
        }

        var entry = ordered.Single(record => record.RecordType == TimeRecordType.Entry).RecordedAt;
        var breakStart = ordered.Single(record => record.RecordType == TimeRecordType.BreakStart).RecordedAt;
        var breakEnd = ordered.Single(record => record.RecordType == TimeRecordType.BreakEnd).RecordedAt;
        var exit = ordered.Single(record => record.RecordType == TimeRecordType.Exit).RecordedAt;

        return exit - entry - (breakEnd - breakStart);
    }
}
