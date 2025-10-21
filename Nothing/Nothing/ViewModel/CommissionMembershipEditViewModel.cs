using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nothing.Model;
using Sample.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Nothing.ViewModel
{
    public class CommissionMembershipEditViewModel : BaseClass
    {
        private readonly CouncyContext _context;

        public ObservableCollection<Commission> Commissions { get; set; } = new();
        public ObservableCollection<DumaMember> Members { get; set; } = new();

        public CommissionMembership Membership { get; set; }

        public Commission SelectedCommission
        {
            get => Membership.Commission;
            set { Membership.Commission = value; OnPropertyChanged(); }
        }

        public DumaMember SelectedMember
        {
            get => Membership.Member;
            set { Membership.Member = value; OnPropertyChanged(); }
        }

        public DateTime StartDate
        {
            get => Membership.StartDate;
            set { Membership.StartDate = value; OnPropertyChanged(); }
        }

        public DateTime? EndDate
        {
            get => Membership.EndDate;
            set { Membership.EndDate = value; OnPropertyChanged(); }
        }

        public CommissionMembershipEditViewModel(CommissionMembership membership = null)
        {
            _context = new CouncyContext();
            Membership = membership ?? new CommissionMembership { StartDate = DateTime.Today };

            foreach (var c in _context.Commissions.ToList())
                Commissions.Add(c);
            foreach (var m in _context.DumaMembers.ToList())
                Members.Add(m);
        }

        private RelayCommand saveCommand;
        public ICommand SaveCommand => saveCommand ??= new RelayCommand(obj =>
        {
            if (SelectedCommission == null || SelectedMember == null)
            {
                MessageBox.Show("Выберите комиссию и члена Думы!");
                return;
            }

            if (!_context.CommissionMemberships.Contains(Membership))
                _context.CommissionMemberships.Add(Membership);

            _context.SaveChanges();
            if (obj is Window window) window.DialogResult = true;
        });

        private RelayCommand cancelCommand;
        public ICommand CancelCommand => cancelCommand ??= new RelayCommand(obj =>
        {
            if (obj is Window window) window.DialogResult = false;
        });
    }
}
