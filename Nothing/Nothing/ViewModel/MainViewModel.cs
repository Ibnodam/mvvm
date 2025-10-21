using System.Windows;
using System.Windows.Input;
using Nothing.Model;
using Nothing.View;
using Sample.Model;

namespace Nothing.ViewModel
{
    public class MainViewModel : BaseClass
    {
        private RelayCommand _openCommissionActivityReportCommand; //Работает
        public ICommand OpenCommissionActivityReportCommand => _openCommissionActivityReportCommand ??= new RelayCommand(obj =>
        {
            var window = new CommissionActivityReportView
            {
                DataContext = new CommissionActivityReportViewModel()
            };
            window.ShowDialog();
        });

        private RelayCommand _openAttendanceReportCommand; //Работает
        public ICommand OpenAttendanceReportCommand => _openAttendanceReportCommand ??= new RelayCommand(obj =>
        {
            var window = new AttendanceReportView
            {
                DataContext = new AttendanceReportViewModel()
            };
            window.ShowDialog();
        });



        private RelayCommand _openCommissionsManagementCommand; //Работает
        public ICommand OpenCommissionsManagementCommand => _openCommissionsManagementCommand ??= new RelayCommand(obj =>
        {
            var window = new CommissionsManagementView
            {
                DataContext = new CommissionsManagementViewModel()
            };
            window.ShowDialog();
        });

        private RelayCommand _openCommissionsReportCommand; //Работает
        public ICommand OpenCommissionsReportCommand => _openCommissionsReportCommand ??= new RelayCommand(obj =>
        {
            var window = new CommissionsReportView
            {
                DataContext = new CommissionsReportViewModel()
            };
            window.ShowDialog();
        });

        private RelayCommand _openCommissionMeetingsCommand; //Работает
        public ICommand OpenCommissionMeetingsCommand => _openCommissionMeetingsCommand ??= new RelayCommand(obj =>
        {
            var window = new CommissionMeetingsView
            {
                DataContext = new CommissionMeetingsViewModel()
            };
            window.ShowDialog();
        });



        private string _currentUser;
        public string CurrentUser
        {
            get => _currentUser;
            set { _currentUser = value; OnPropertyChanged(); }
        }

        //private RelayCommand openMeetingsCommand;
        //public ICommand OpenMeetingsCommand => openMeetingsCommand ??= new RelayCommand(obj =>
        //{
        //    var window = new MeetingsWindow { DataContext = new MeetingsViewModel() };
        //    window.ShowDialog();
        //});

        private RelayCommand openMembersCommand; //Работает

        public ICommand OpenMembersCommand => openMembersCommand ??= new RelayCommand(obj =>
        {
            var window = new MembersWindow();
            window.ShowDialog();
        });

        private RelayCommand openQueryMembershipsCommand; //Работает
        public ICommand OpenQueryMembershipsCommand => openQueryMembershipsCommand ??= new RelayCommand(obj =>
        {
            new CommissionMembershipWindow().ShowDialog();
        });

        //private RelayCommand openChairsCommand;
        //public ICommand OpenChairsCommand => openChairsCommand ??= new RelayCommand(obj =>
        //{
        //    MessageBox.Show("Здесь будет окно Председателей комиссий");
        //});


        //private RelayCommand openCommissionsCommand;
        //public ICommand OpenCommissionsCommand => openCommissionsCommand ??= new RelayCommand(obj =>
        //{
        //    var viewModel = new CommissionsViewModel();
        //    var window = new CommissionsWindow
        //    {
        //        DataContext = viewModel
        //    };
        //    window.ShowDialog();
        //});

        //private RelayCommand openMembershipsCommand;
        //public ICommand OpenMembershipsCommand => openMembershipsCommand ??= new RelayCommand(obj =>
        //{
        //    MessageBox.Show("Здесь будет окно Членства в комиссии");
        //});

        //private RelayCommand openQueryMembersWithCommissionsCommand;
        //public ICommand OpenQueryMembersWithCommissionsCommand => openQueryMembersWithCommissionsCommand ??= new RelayCommand(obj =>
        //{
        //    new MembersWindow().ShowDialog();
        //});

        //private RelayCommand openAttendeesCommand;
        //public ICommand OpenAttendeesCommand => openAttendeesCommand ??= new RelayCommand(obj =>
        //{
        //    var window = new AttendeesWindow
        //    {
        //        DataContext = new AttendeesViewModel()
        //    };
        //    window.ShowDialog();
        //});

        //private RelayCommand openCommissionReportCommand;
        //public ICommand OpenCommissionReportCommand => openCommissionReportCommand ??= new RelayCommand(obj =>
        //{
        //    var window = new CommissionReportWindow
        //    {
        //        DataContext = new CommissionReportViewModel()
        //    };
        //    window.ShowDialog();
        //});
    }
}
