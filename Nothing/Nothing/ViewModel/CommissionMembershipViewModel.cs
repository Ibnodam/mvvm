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
using Nothing.View;

namespace Nothing.ViewModel
{
    public class CommissionMembershipViewModel : BaseClass
    {
        private readonly CouncyContext _context;

        public ObservableCollection<CommissionMembership> Memberships { get; set; } = new();

        private CommissionMembership selectedMembership;
        public CommissionMembership SelectedMembership
        {
            get => selectedMembership;
            set { selectedMembership = value; OnPropertyChanged(); }
        }

        // Для ComboBox
        public ObservableCollection<DumaMember> Members { get; set; } = new();
        public ObservableCollection<Commission> Commissions { get; set; } = new();

        public CommissionMembershipViewModel()
        {
            _context = new CouncyContext();

            Members = new ObservableCollection<DumaMember>(_context.DumaMembers.ToList());
            Commissions = new ObservableCollection<Commission>(_context.Commissions.ToList());

            LoadMemberships();
        }

        private void LoadMemberships()
        {
            Memberships.Clear();
            foreach (var m in _context.CommissionMemberships
                                      .ToList())
            {
                m.Member = _context.DumaMembers.FirstOrDefault(x => x.MemberId == m.MemberId)!;
                m.Commission = _context.Commissions.FirstOrDefault(x => x.CommissionId == m.CommissionId)!;
                Memberships.Add(m);
            }
        }

        // CRUD
        private RelayCommand addCommand;
        public ICommand AddCommand => addCommand ??= new RelayCommand(obj =>
        {
            SelectedMembership = new CommissionMembership
            {
                StartDate = DateTime.Today
            };
            OpenEditWindow();
        });

        private RelayCommand editCommand;
        public ICommand EditCommand => editCommand ??= new RelayCommand(obj =>
        {
            if (SelectedMembership == null) return;
            OpenEditWindow();
        });

        private RelayCommand deleteCommand;
        public ICommand DeleteCommand => deleteCommand ??= new RelayCommand(obj =>
        {
            if (SelectedMembership == null) return;
            if (MessageBox.Show($"Удалить запись?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _context.CommissionMemberships.Remove(SelectedMembership);
                _context.SaveChanges();
                LoadMemberships();
            }
        });




        private void OpenEditWindow()
        {
            var vm = new CommissionMembershipEditViewModel(SelectedMembership);
            var window = new CommissionMembershipEditWindow { DataContext = vm };

            if (window.ShowDialog() == true)
            {
                // Проверяем, новая ли запись
                if (!_context.CommissionMemberships.Any(cm => cm.ComisMemId == vm.Membership.ComisMemId))
                {
                    vm.Membership.Commission = _context.Commissions.FirstOrDefault(c => c.CommissionId == vm.SelectedCommission.CommissionId)!;
                    vm.Membership.Member = _context.DumaMembers.FirstOrDefault(m => m.MemberId == vm.SelectedMember.MemberId)!;

                    _context.CommissionMemberships.Add(vm.Membership);
                }

                _context.SaveChanges();
                LoadMemberships();
            }
        }





        //private void OpenEditWindow()
        //{
        //    var vm = new CommissionMembershipEditViewModel(SelectedMembership);
        //    var window = new CommissionMembershipEditWindow { DataContext = vm };

        //    if (window.ShowDialog() == true)
        //    {
        //        // Проверяем, новая ли запись
        //        if (!_context.CommissionMemberships.Any(cm => cm.ComisMemId == vm.Membership.ComisMemId))
        //        {
        //            // Присваиваем существующие навигационные объекты через ID
        //            vm.Membership.Commission = _context.Commissions.FirstOrDefault(c => c.CommissionId == vm.SelectedCommission.CommissionId)!;
        //            vm.Membership.Member = _context.DumaMembers.FirstOrDefault(m => m.MemberId == vm.SelectedMember.MemberId)!;

        //            _context.CommissionMemberships.Add(vm.Membership);
        //        }

        //        _context.SaveChanges();
        //        LoadMemberships();
        //    }
        //}




        //private void OpenEditWindow()
        //{
        //    var window = new CommissionMembershipEditWindow { DataContext = this };
        //    if (window.ShowDialog() == true)
        //    {
        //        if (!_context.CommissionMemberships.Contains(SelectedMembership))
        //            _context.CommissionMemberships.Add(SelectedMembership);

        //        _context.SaveChanges();
        //        LoadMemberships();
        //    }
        //}

        private RelayCommand saveCommand;
        public ICommand SaveCommand => saveCommand ??= new RelayCommand(obj =>
        {
            if (obj is Window w)
                w.DialogResult = true;
        });

        private RelayCommand cancelCommand;
        public ICommand CancelCommand => cancelCommand ??= new RelayCommand(obj =>
        {
            if (obj is Window w)
                w.DialogResult = false;
        });
    }
}

