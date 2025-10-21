using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nothing.Model;

public partial class MeetingAttendee : BaseClass
{
    [Key]
    public int AttendeesId { get; set; }

    private int meetingId;
    public int MeetingId
    {
        get => meetingId;
        set { meetingId = value; OnPropertyChanged(); }
    }

    private int memberId;
    public int MemberId
    {
        get => memberId;
        set { memberId = value; OnPropertyChanged(); }
    }

    public int? RoleId { get; set; }

    public virtual CommissionMeeting Meeting { get; set; } = null!;
    public virtual DumaMember Member { get; set; } = null!;
}