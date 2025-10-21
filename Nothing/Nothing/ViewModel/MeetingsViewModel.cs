using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nothing.Model;
using Sample.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Nothing.View;
using Nothing.Model;
using Nothing.View;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Nothing.ViewModel
{
    public class MeetingsViewModel : BaseClass
    {
        private readonly CouncyContext _context = new();

        public ObservableCollection<CommissionMeeting> Meetings { get; set; }
        private CommissionMeeting selectedMeeting;
        public CommissionMeeting SelectedMeeting
        {
            get => selectedMeeting;
            set { selectedMeeting = value; OnPropertyChanged(); }
        }

        public MeetingAttendee SelectedAttendee { get; set; }

        public MeetingsViewModel()
        {
            LoadMeetings();
        }

        private void LoadMeetings()
        {
            Meetings = new ObservableCollection<CommissionMeeting>(
                _context.CommissionMeetings
                        .OrderBy(m => m.MeetingDateTime)
                        .ToList());
            OnPropertyChanged(nameof(Meetings));
        }

        // --- Команды ---
        public ICommand AddMeetingCommand => new RelayCommand(_ => OpenMeetingEditWindow(new CommissionMeeting()));
        public ICommand EditMeetingCommand => new RelayCommand(_ =>
        {
            if (SelectedMeeting != null)
                OpenMeetingEditWindow(SelectedMeeting);
        });
        public ICommand DeleteMeetingCommand => new RelayCommand(_ =>
        {
            if (SelectedMeeting != null)
            {
                if (MessageBox.Show("Удалить митинг?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _context.CommissionMeetings.Remove(SelectedMeeting);
                    _context.SaveChanges();
                    LoadMeetings();
                }
            }
        });

        public ICommand AddAttendeeCommand => new RelayCommand(_ =>
        {
            if (SelectedMeeting == null) return;
            var attendee = new MeetingAttendee { MeetingId = SelectedMeeting.MeetingId };
            var vm = new AttendeeEditWindowViewModel(attendee);
            var window = new AttendeeEditWindow { DataContext = vm };
            if (window.ShowDialog() == true)
            {
                _context.MeetingAttendees.Add(vm.Attendee);
                _context.SaveChanges();
                LoadMeetings();
            }
        });

        public ICommand DeleteAttendeeCommand => new RelayCommand(_ =>
        {
            if (SelectedAttendee != null)
            {
                _context.MeetingAttendees.Remove(SelectedAttendee);
                _context.SaveChanges();
                LoadMeetings();
            }
        });

        // --- Вспомогательные методы ---
        private void OpenMeetingEditWindow(CommissionMeeting meeting)
        {
            var vm = new MeetingEditWindowViewModel(meeting);
            var window = new MeetingEditWindow { DataContext = vm };

            if (window.ShowDialog() == true)
            {
                if (!_context.CommissionMeetings.Any(x => x.MeetingId == vm.Meeting.MeetingId))
                    _context.CommissionMeetings.Add(vm.Meeting);

                _context.SaveChanges();
                LoadMeetings();
            }
        }
    }
}
