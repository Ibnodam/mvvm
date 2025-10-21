using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Nothing.Model;
using Sample.Model;
using Microsoft.EntityFrameworkCore;

namespace Nothing.ViewModel
{
    public class CommissionActivityReportViewModel : BaseClass
    {
        private readonly CouncyContext _context;

        public ObservableCollection<CommissionActivityInfo> ActivityReport { get; set; } = new();

        private DateTime _startDate = DateTime.Now.AddMonths(-1);
        public DateTime StartDate
        {
            get => _startDate;
            set { _startDate = value; OnPropertyChanged(); }
        }

        private DateTime _endDate = DateTime.Now;
        public DateTime EndDate
        {
            get => _endDate;
            set { _endDate = value; OnPropertyChanged(); }
        }

        public CommissionActivityReportViewModel()
        {
            _context = new CouncyContext();
        }

        public class CommissionActivityInfo : BaseClass
        {
            public string CommissionName { get; set; }
            public string ChairmanName { get; set; }
            public int MembersCount { get; set; }
            public int MeetingsCount { get; set; }
            public double MeetingsPerMonth => MeetingsCount > 0 ? (double)MeetingsCount / 3 : 0; // Примерно за 3 месяца
        }

        // Команда генерации отчета
        private RelayCommand _generateReportCommand;
        public ICommand GenerateReportCommand => _generateReportCommand ??= new RelayCommand(obj =>
        {
            if (StartDate > EndDate)
            {
                MessageBox.Show("Дата начала не может быть позже даты окончания!");
                return;
            }

            GenerateActivityReport();
        });

        private void GenerateActivityReport()
        {
            ActivityReport.Clear();

            // Получаем все комиссии с их заседаниями и председателями
            var commissions = _context.Commissions
                .Include(c => c.CommissionMeetings)
                .Include(c => c.CommissionChairs)
                .ThenInclude(ch => ch.Member)
                .Include(c => c.CommissionMemberships)
                .AsNoTracking()
                .ToList();

            foreach (var commission in commissions)
            {
                var meetingsInPeriod = commission.CommissionMeetings
                    .Count(m => m.MeetingDateTime >= StartDate && m.MeetingDateTime <= EndDate);

                var chairman = commission.CommissionChairs
                    .FirstOrDefault()?.Member?.Name ?? "—";

                var activeMembers = commission.CommissionMemberships
                    .Count(m => m.StartDate <= DateTime.Now &&
                               (m.EndDate == null || m.EndDate >= DateTime.Now));

                var activityInfo = new CommissionActivityInfo
                {
                    CommissionName = commission.Name,
                    ChairmanName = chairman,
                    MembersCount = activeMembers,
                    MeetingsCount = meetingsInPeriod
                };

                ActivityReport.Add(activityInfo);
            }

            // Сортируем по количеству заседаний (по убыванию)
            var sortedReport = ActivityReport.OrderByDescending(c => c.MeetingsCount).ToList();
            ActivityReport.Clear();
            foreach (var item in sortedReport)
            {
                ActivityReport.Add(item);
            }

            MessageBox.Show($"Отчет сгенерирован!\nПериод: {StartDate:dd.MM.yyyy} - {EndDate:dd.MM.yyyy}\n" +
                           $"Всего комиссий: {commissions.Count}");
        }

        // Команда закрытия
        private RelayCommand _closeCommand;
        public ICommand CloseCommand => _closeCommand ??= new RelayCommand(obj =>
        {
            if (obj is Window window) window.Close();
        });
    }
}