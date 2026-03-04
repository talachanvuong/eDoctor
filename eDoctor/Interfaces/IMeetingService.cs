using eDoctor.Models.Dtos.Meeting.Queries;
using eDoctor.Results;

namespace eDoctor.Interfaces;

public interface IMeetingService
{
    Task<Result> JoinAsync(RoomQueryDto dto);
}
