using Microsoft.EntityFrameworkCore;
using Nothing.Model;
using System.Collections.ObjectModel;

namespace Nothing.ViewModel
{
    public class CommissionReportViewModel : BaseClass
    {
        private readonly CouncyContext _context;

        public ObservableCollection<CommissionReportItem> Commissions { get; set; } = new();

        public CommissionReportViewModel()
        {
            _context = new CouncyContext();
            LoadReport();
        }

        private void LoadReport()
        {
            Commissions.Clear();

            var commissions = _context.Commissions
                .Include(c => c.CommissionChairs)
                    .ThenInclude(ch => ch.Member)
                .Include(c => c.CommissionMemberships)
                    .ThenInclude(m => m.Member)
                .ToList();

            foreach (var c in commissions)
            {
                var chairman = c.CommissionChairs.FirstOrDefault()?.Member?.Name ?? "—";
                var members = c.CommissionMemberships.Select(cm => cm.Member?.Name ?? "—");

                Commissions.Add(new CommissionReportItem
                {
                    Name = c.Name,
                    Profile = c.Profile,
                    ChairmanName = chairman,
                    MembersList = string.Join(", ", members)
                });
            }
        }
    }

    public class CommissionReportItem
    {
        public string Name { get; set; } = "";
        public string Profile { get; set; } = "";
        public string ChairmanName { get; set; } = "";
        public string MembersList { get; set; } = "";
    }
}
