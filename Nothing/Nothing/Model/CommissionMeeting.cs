using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nothing.Model;

public partial class CommissionMeeting : BaseClass
{
    [Key]
    public int MeetingId { get; set; }

    private int commissionId;
    public int CommissionId
    {
        get => commissionId;
        set { commissionId = value; OnPropertyChanged(); }
    }

    private DateTime meetingDateTime;
    public DateTime MeetingDateTime
    {
        get => meetingDateTime;
        set { meetingDateTime = value; OnPropertyChanged(); }
    }

    private string location;
    public string Location
    {
        get => location;
        set { location = value; OnPropertyChanged(); }
    }

    public virtual Commission Commission { get; set; } = null!;
    public virtual ICollection<MeetingAttendee> MeetingAttendees { get; set; } = new List<MeetingAttendee>();
}