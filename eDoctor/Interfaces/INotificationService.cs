using eDoctor.Models.Dtos.Notification;
using eDoctor.Models.Dtos.Notification.Queries;
using eDoctor.Results;

namespace eDoctor.Interfaces;

public interface INotificationService
{
    Task SendAsync(SendQueryDto dto);
    Task<MyDto> GetMyAsync(MyQueryDto dto);
    Task<Result> ReadAsync(ReadQueryDto dto);
}
