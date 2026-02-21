using eDoctor.Enums;

namespace eDoctor.Helpers.ExtensionMethods;

public static class ScheduleStatusHelper
{
    public static string ConvertToString(this ScheduleStatus scheduleStatus)
    {
        return scheduleStatus switch
        {
            ScheduleStatus.CREATED => "Created",
            ScheduleStatus.ORDERED => "Ordered",
            ScheduleStatus.ONGOING => "Ongoing",
            ScheduleStatus.COMPLETED => "Completed",
            ScheduleStatus.CANCELLED => "Cancelled",
            _ => "None"
        };
    }
}
