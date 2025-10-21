using Nothing.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Sample.Model;
using Nothing.View;



namespace Nothing.ViewModel
{
    public class AttendeesViewModel : BaseClass
    {
        private readonly CouncyContext _context;

        public ObservableCollection<MeetingAttendee> Attendees { get; set; } = new();

        private MeetingAttendee selectedAttendee;
        public MeetingAttendee SelectedAttendee
        {
            get => selectedAttendee;
            set { selectedAttendee = value; OnPropertyChanged(); }
        }

        public AttendeesViewModel()
        {
            _context = new CouncyContext();
            LoadAttendees();
        }

        private void LoadAttendees()
        {
            Attendees.Clear();
            foreach (var a in _context.MeetingAttendees
                                       .ToList()) // при необходимости Include для Member и Meeting
                Attendees.Add(a);
        }

        // --- Команды CRUD ---
        private RelayCommand addCommand;
        public ICommand AddCommand => addCommand ??= new RelayCommand(obj =>
        {
            var vm = new AttendeeEditWindowViewModel(); // новый объект
            var view = new AttendeeEditWindow { DataContext = vm };
            if (view.ShowDialog() == true)
            {
                _context.MeetingAttendees.Add(vm.Attendee);
                _context.SaveChanges();
                LoadAttendees();
            }
        });

        private RelayCommand editCommand;
        public ICommand EditCommand => editCommand ??= new RelayCommand(obj =>
        {
            if (SelectedAttendee == null) return;

            var vm = new AttendeeEditWindowViewModel(SelectedAttendee);
            var view = new AttendeeEditWindow { DataContext = vm };
            if (view.ShowDialog() == true)
            {
                _context.SaveChanges();
                LoadAttendees();
            }
        });

        private RelayCommand deleteCommand;
        public ICommand DeleteCommand => deleteCommand ??= new RelayCommand(obj =>
        {
            if (SelectedAttendee == null) return;

            if (MessageBox.Show($"Удалить участника {SelectedAttendee.Member.Name}?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _context.MeetingAttendees.Remove(SelectedAttendee);
                _context.SaveChanges();
                LoadAttendees();
            }
        });
    }
}

