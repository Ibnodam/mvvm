using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nothing.Model;

public partial class Commission : BaseClass
{
    [Key]
    public int CommissionId { get; set; }

    private string name;
    public string Name
    {
        get => name;
        set { name = value; OnPropertyChanged(); }
    }

    private string profile;
    public string Profile
    {
        get => profile;
        set { profile = value; OnPropertyChanged(); }
    }

    public virtual ICollection<CommissionChair> CommissionChairs { get; set; } = new List<CommissionChair>();
    public virtual ICollection<CommissionMeeting> CommissionMeetings { get; set; } = new List<CommissionMeeting>();
    public virtual ICollection<CommissionMembership> CommissionMemberships { get; set; } = new List<CommissionMembership>();

    [NotMapped]
    public string ChairmanName => CommissionChairs.FirstOrDefault()?.Member?.Name ?? "—";

    [NotMapped]
    public string MembersList => CommissionMemberships.Any()
        ? string.Join(", ", CommissionMemberships.Select(m => m.Member?.Name ?? "—"))
        : "—";
}