using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Nothing.Model;
using Sample.Model;

namespace Nothing.ViewModel
{
    public class CommissionMeetingsViewModel : BaseClass
    {
        private readonly CouncyContext _context;

        public ObservableCollection<CommissionMeeting> Meetings { get; set; } = new();
        public ObservableCollection<Commission> Commissions { get; set; } = new();
        public ObservableCollection<DumaMember> AllMembers { get; set; } = new();
        public ObservableCollection<MeetingAttendee> CurrentAttendees { get; set; } = new();

        private CommissionMeeting _selectedMeeting;
        public CommissionMeeting SelectedMeeting
        {
            get => _selectedMeeting;
            set
            {
                _selectedMeeting = value;
                OnPropertyChanged();
                LoadAttendees();
            }
        }

        private DumaMember _selectedMemberToAdd;
        public DumaMember SelectedMemberToAdd
        {
            get => _selectedMemberToAdd;
            set { _selectedMemberToAdd = value; OnPropertyChanged(); }
        }

        private MeetingAttendee _selectedAttendee;
        public MeetingAttendee SelectedAttendee
        {
            get => _selectedAttendee;
            set { _selectedAttendee = value; OnPropertyChanged(); }
        }

        public CommissionMeetingsViewModel()
        {
            _context = new CouncyContext();
            LoadData();
        }

        private void LoadData()
        {
            Meetings.Clear();
            Commissions.Clear();
            AllMembers.Clear();

            foreach (var meeting in _context.CommissionMeetings
                .Include(m => m.Commission)
                .Include(m => m.MeetingAttendees)
                .ThenInclude(a => a.Member)
                .ToList())
            {
                Meetings.Add(meeting);
            }

            foreach (var commission in _context.Commissions.ToList())
                Commissions.Add(commission);

            foreach (var member in _context.DumaMembers.ToList())
                AllMembers.Add(member);
        }

        private void LoadAttendees()
        {
            CurrentAttendees.Clear();
            if (SelectedMeeting?.MeetingAttendees != null)
            {
                foreach (var attendee in SelectedMeeting.MeetingAttendees)
                    CurrentAttendees.Add(attendee);
            }
        }

        private RelayCommand _addMeetingCommand;
        public ICommand AddMeetingCommand => _addMeetingCommand ??= new RelayCommand(obj =>
        {
            var newMeeting = new CommissionMeeting
            {
                MeetingDateTime = DateTime.Now,
                Location = "Зал заседаний"
            };
            Meetings.Add(newMeeting);
            SelectedMeeting = newMeeting;
        });

        private RelayCommand _saveMeetingCommand;
        public ICommand SaveMeetingCommand => _saveMeetingCommand ??= new RelayCommand(obj =>
        {
            if (SelectedMeeting == null) return;

            if (!_context.CommissionMeetings.Contains(SelectedMeeting))
                _context.CommissionMeetings.Add(SelectedMeeting);

            _context.SaveChanges();
            MessageBox.Show("Заседание сохранено!");
        });

        private RelayCommand _deleteMeetingCommand;
        public ICommand DeleteMeetingCommand => _deleteMeetingCommand ??= new RelayCommand(obj =>
        {
            if (SelectedMeeting == null) return;

            _context.CommissionMeetings.Remove(SelectedMeeting);
            Meetings.Remove(SelectedMeeting);
            _context.SaveChanges();
        });

        // Команды для управления участниками
        private RelayCommand _addAttendeeCommand;
        public ICommand AddAttendeeCommand => _addAttendeeCommand ??= new RelayCommand(obj =>
        {
            if (SelectedMeeting == null || SelectedMemberToAdd == null)
            {
                MessageBox.Show("Выберите заседание и участника!");
                return;
            }

            var attendee = new MeetingAttendee
            {
                MeetingId = SelectedMeeting.MeetingId,
                MemberId = SelectedMemberToAdd.MemberId,
                Meeting = SelectedMeeting,
                Member = SelectedMemberToAdd
            };

            SelectedMeeting.MeetingAttendees.Add(attendee);
            CurrentAttendees.Add(attendee);

            if (SelectedMeeting.MeetingId > 0) // Если заседание уже сохранено в БД
            {
                _context.MeetingAttendees.Add(attendee);
                _context.SaveChanges();
            }
        });

        private RelayCommand _removeAttendeeCommand;
        public ICommand RemoveAttendeeCommand => _removeAttendeeCommand ??= new RelayCommand(obj =>
        {
            if (SelectedAttendee == null) return;

            SelectedMeeting.MeetingAttendees.Remove(SelectedAttendee);
            CurrentAttendees.Remove(SelectedAttendee);

            if (SelectedAttendee.AttendeesId > 0) // Если участник уже сохранен в БД
            {
                _context.MeetingAttendees.Remove(SelectedAttendee);
                _context.SaveChanges();
            }
        });
    }
}