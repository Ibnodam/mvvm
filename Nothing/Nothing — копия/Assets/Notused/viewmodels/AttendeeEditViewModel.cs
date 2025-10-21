using Nothing.Model;
using Sample.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
namespace Nothing.ViewModel
{
    public class AttendeeEditWindowViewModel : BaseClass
    {
        private readonly CouncyContext _context = new();

        public MeetingAttendee Attendee { get; set; }

        public ObservableCollection<DumaMember> Members { get; set; }

        public DumaMember SelectedMember
        {
            get => Attendee.Member;
            set
            {
                Attendee.Member = value;
                Attendee.MemberId = value?.MemberId ?? 0;
                OnPropertyChanged();
            }
        }

        // ✅ Конструктор для редактирования существующего участника
        public AttendeeEditWindowViewModel(MeetingAttendee attendee)
        {
            Attendee = attendee;
            Members = new ObservableCollection<DumaMember>(_context.DumaMembers.ToList());
        }

        // ✅ Конструктор для добавления нового участника
        public AttendeeEditWindowViewModel()
        {
            Attendee = new MeetingAttendee();
            Members = new ObservableCollection<DumaMember>(_context.DumaMembers.ToList());
        }

        public ICommand SaveCommand => new RelayCommand(obj =>
        {
            if (SelectedMember == null)
            {
                MessageBox.Show("Выберите участника!");
                return;
            }

            ((Window)obj).DialogResult = true;
        });

        public ICommand CancelCommand => new RelayCommand(obj =>
        {
            ((Window)obj).DialogResult = false;
        });
    }
}
