using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nothing.Model;

public partial class CommissionChair : BaseClass
{
    [Key]
    public int ChairsId { get; set; }

    private int commissionId;
    public int CommissionId
    {
        get => commissionId;
        set { commissionId = value; OnPropertyChanged(); }
    }

    private int memberId;
    public int MemberId
    {
        get => memberId;
        set { memberId = value; OnPropertyChanged(); }
    }

    private DateTime startDate;
    public DateTime StartDate
    {
        get => startDate;
        set { startDate = value; OnPropertyChanged(); }
    }

    private DateTime? endDate;
    public DateTime? EndDate
    {
        get => endDate;
        set { endDate = value; OnPropertyChanged(); }
    }

    public virtual Commission Commission { get; set; } = null!;
    public virtual DumaMember Member { get; set; } = null!;
}