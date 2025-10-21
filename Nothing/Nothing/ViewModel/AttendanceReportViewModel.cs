using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class AttendanceReportViewModel : BaseClass
    {
        private readonly CouncyContext _context;

        public ObservableCollection<Commission> Commissions { get; set; } = new();
        public ObservableCollection<MemberAttendanceInfo> AttendanceReport { get; set; } = new();

        private Commission _selectedCommission;
        public Commission SelectedCommission
        {
            get => _selectedCommission;
            set { _selectedCommission = value; OnPropertyChanged(); }
        }

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

        public AttendanceReportViewModel()
        {
            _context = new CouncyContext();
            LoadCommissions();
        }

        private void LoadCommissions()
        {
            Commissions.Clear();
            foreach (var commission in _context.Commissions.AsNoTracking().ToList())
            {
                Commissions.Add(commission);
            }

            if (Commissions.Any())
                SelectedCommission = Commissions.First();
        }

        public class MemberAttendanceInfo : BaseClass
        {
            public string MemberName { get; set; }
            public int TotalMeetings { get; set; }
            public int AttendedMeetings { get; set; }
            public int MissedMeetings => TotalMeetings - AttendedMeetings;
            public double AttendanceRate => TotalMeetings > 0 ? (double)AttendedMeetings / TotalMeetings * 100 : 0;
        }

        // Генерации отчета
        private RelayCommand _generateReportCommand;
        public ICommand GenerateReportCommand => _generateReportCommand ??= new RelayCommand(obj =>
        {
            if (SelectedCommission == null)
            {
                MessageBox.Show("Выберите комиссию!");
                return;
            }

            if (StartDate > EndDate)
            {
                MessageBox.Show("Дата начала не может быть позже даты окончания!");
                return;
            }

            GenerateAttendanceReport();
        });

        private void GenerateAttendanceReport()
        {
            AttendanceReport.Clear();

            var meetingsInPeriod = _context.CommissionMeetings
                .Where(m => m.CommissionId == SelectedCommission.CommissionId &&
                           m.MeetingDateTime >= StartDate &&
                           m.MeetingDateTime <= EndDate)
                .AsNoTracking()
                .ToList();

            if (!meetingsInPeriod.Any())
            {
                MessageBox.Show("За указанный период заседаний не найдено!");
                return;
            }
            var commissionMembers = _context.CommissionMemberships
                .Where(cm => cm.CommissionId == SelectedCommission.CommissionId &&
                            cm.StartDate <= EndDate &&
                            (cm.EndDate == null || cm.EndDate >= StartDate))
                .Include(cm => cm.Member)
                .AsNoTracking()
                .ToList();

            foreach (var member in commissionMembers)
            {
                var attendedMeetings = _context.MeetingAttendees
                    .Where(ma => ma.MemberId == member.MemberId &&
                                meetingsInPeriod.Select(m => m.MeetingId).Contains(ma.MeetingId))
                    .AsNoTracking()
                    .Count();

                var memberInfo = new MemberAttendanceInfo
                {
                    MemberName = member.Member.Name,
                    TotalMeetings = meetingsInPeriod.Count,
                    AttendedMeetings = attendedMeetings
                };

                AttendanceReport.Add(memberInfo);
            }

            var sortedReport = AttendanceReport.OrderByDescending(m => m.MissedMeetings).ToList();
            AttendanceReport.Clear();
            foreach (var item in sortedReport)
            {
                AttendanceReport.Add(item);
            }

            MessageBox.Show($"Отчет сгенерирован!\nПериод: {StartDate:dd.MM.yyyy} - {EndDate:dd.MM.yyyy}\n" +
                           $"Комиссия: {SelectedCommission.Name}\n" +
                           $"Всего заседаний: {meetingsInPeriod.Count}");
        }

        // Команда закрытия
        private RelayCommand _closeCommand;
        public ICommand CloseCommand => _closeCommand ??= new RelayCommand(obj =>
        {
            if (obj is Window window) window.Close();
        });
    }
}