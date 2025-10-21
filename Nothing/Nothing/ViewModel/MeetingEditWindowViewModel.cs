using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nothing.Model;
using Sample.Model;
using System.Collections.ObjectModel;
using System.Linq;

using Nothing.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using Nothing.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Nothing.ViewModel
{
    public class MeetingEditWindowViewModel : BaseClass
    {
        private readonly CouncyContext _context = new();
        public CommissionMeeting Meeting { get; set; }

        public ObservableCollection<Commission> Commissions { get; set; }
        public Commission SelectedCommission
        {
            get => Meeting.Commission;
            set
            {
                Meeting.Commission = value;
                Meeting.CommissionId = value.CommissionId;
                OnPropertyChanged();
            }
        }

        public MeetingEditWindowViewModel(CommissionMeeting meeting)
        {
            Meeting = meeting;
            Commissions = new ObservableCollection<Commission>(_context.Commissions.ToList());
        }

        public ICommand SaveCommand => new RelayCommand(obj =>
        {
            if (SelectedCommission == null)
            {
                MessageBox.Show("Выберите комиссию!");
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
