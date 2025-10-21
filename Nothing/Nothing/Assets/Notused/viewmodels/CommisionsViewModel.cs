using Nothing.Model;
using Nothing.View;
using Sample.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Nothing.ViewModel
{
    public class CommissionsViewModel : BaseClass
    {
        private readonly CouncyContext _context;

        public ObservableCollection<Commission> Commissions { get; set; } = new();

        private Commission selectedCommission;
        public Commission SelectedCommission
        {
            get => selectedCommission;
            set { selectedCommission = value; OnPropertyChanged(); }
        }

        public CommissionsViewModel()
        {
            _context = new CouncyContext();
            LoadCommissions();
        }

        private void LoadCommissions()
        {
            Commissions.Clear();
            foreach (var c in _context.Commissions.ToList())
                Commissions.Add(c);
        }

        // --- Команды ---
        private RelayCommand addCommand;
        public ICommand AddCommand => addCommand ??= new RelayCommand(obj =>
        {
            SelectedCommission = new Commission(); // новая комиссия
            OpenEditWindow();
        });

        private RelayCommand editCommand;
        public ICommand EditCommand => editCommand ??= new RelayCommand(obj =>
        {
            if (SelectedCommission == null) return;
            OpenEditWindow();
        });

        private RelayCommand deleteCommand;
        public ICommand DeleteCommand => deleteCommand ??= new RelayCommand(obj =>
        {
            if (SelectedCommission == null) return;

            if (MessageBox.Show($"Удалить комиссию {SelectedCommission.Name}?",
                                "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _context.Commissions.Remove(SelectedCommission);
                _context.SaveChanges();
                LoadCommissions();
            }
        });

        // --- Метод открытия окна редактирования ---
        private void OpenEditWindow()
        {
            var editWindow = new CommissionEditWindow { DataContext = this };
            if (editWindow.ShowDialog() == true)
            {
                if (!_context.Commissions.Contains(SelectedCommission))
                    _context.Commissions.Add(SelectedCommission);

                _context.SaveChanges();
                LoadCommissions();
            }
        }

        // --- Команды кнопок в окне редактирования ---
        private RelayCommand saveCommand;
        public ICommand SaveCommand => saveCommand ??= new RelayCommand(obj =>
        {
            if (obj is Window w)
            {
                w.DialogResult = true;
                w.Close();
            }
        });

        private RelayCommand cancelCommand;
        public ICommand CancelCommand => cancelCommand ??= new RelayCommand(obj =>
        {
            if (obj is Window w)
            {
                w.DialogResult = false;
                w.Close();
            }
        });
    }
}
