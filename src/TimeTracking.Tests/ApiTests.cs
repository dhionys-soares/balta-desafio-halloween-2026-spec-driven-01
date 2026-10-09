using TimeTracking.Api;
using TimeTracking.Domain;

namespace TimeTracking.Tests;

public class ApiTests
{
    [Fact]
    public void TimeRecordEndpoints_MapResponse_ReturnsExpectedPayload()
    {
        var record = new TimeRecord
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            RecordType = TimeRecordType.Entry,
            RecordedAt = new DateTime(2026, 10, 9, 8, 0, 0, DateTimeKind.Utc)
        };

        var response = TimeRecordEndpoints.MapResponse(record);

        Assert.Equal(record.Id, response.Id);
        Assert.Equal("Entry", response.RecordType);
    }

    [Fact]
    public void AbsenceEndpoints_MapResponse_ReturnsExpectedPayload()
    {
        var absence = new AbsencePeriod
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            StartAt = new DateTime(2026, 10, 10, 9, 0, 0, DateTimeKind.Utc),
            EndAt = new DateTime(2026, 10, 10, 11, 0, 0, DateTimeKind.Utc),
            Reason = "Acompanhamento familiar"
        };

        var response = AbsenceEndpoints.MapResponse(absence);

        Assert.Equal(absence.Id, response.Id);
        Assert.Equal("Acompanhamento familiar", response.Reason);
    }

    [Fact]
    public void TimeRecordEndpoints_Create_ReturnsCreatedResult()
    {
        var request = new TimeRecordRequest(new DateTime(2026, 10, 9, 8, 0, 0, DateTimeKind.Utc));

        var result = TimeRecordEndpoints.Create(request);

        Assert.NotNull(result);
    }
}
