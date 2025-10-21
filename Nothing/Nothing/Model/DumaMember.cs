using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http.Headers;

namespace Nothing.Model;

public partial class DumaMember : BaseClass
{
    [Key]
    public int MemberId { get; set; }

    private string name;
    public string Name
    {
        get => name;
        set { name = value; OnPropertyChanged(); }
    }

    private string homePhone;
    public string HomePhone
    {
        get => homePhone;
        set { homePhone = value; OnPropertyChanged(); }
    }

    private string workPhone;
    public string WorkPhone
    {
        get => workPhone;
        set { workPhone = value; OnPropertyChanged(); }
    }

    private string address;
    public string Address
    {
        get => address;
        set { address = value; OnPropertyChanged(); }
    }

    public virtual ICollection<CommissionChair> CommissionChairs { get; set; } = new List<CommissionChair>();
    public virtual ICollection<CommissionMembership> CommissionMemberships { get; set; } = new List<CommissionMembership>();
    public virtual ICollection<MeetingAttendee> MeetingAttendees { get; set; } = new List<MeetingAttendee>();

    [NotMapped]
    public string CommissionsList
    {
        get
        {
            if (CommissionMemberships != null && CommissionMemberships.Any())
                return string.Join(", ", CommissionMemberships.Select(cm => cm.Commission?.Name ?? "—"));
            return "—";
        }
    }

}
