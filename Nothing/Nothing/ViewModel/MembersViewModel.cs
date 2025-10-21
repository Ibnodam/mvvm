using Nothing.Model;
using Sample.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Nothing.View;
using Microsoft.EntityFrameworkCore;

namespace Nothing.ViewModel
{
    public class MembersViewModel : BaseClass
    {
        private readonly CouncyContext _context;

        public ObservableCollection<DumaMember> Members { get; set; } = new();

        private DumaMember selectedMember;
        public DumaMember SelectedMember
        {
            get => selectedMember;
            set { selectedMember = value; OnPropertyChanged(); }
        }

        public MembersViewModel()
        {
            _context = new CouncyContext();
            LoadMembers();
        }

        //private void LoadMembers()
        //{
        //    Members.Clear();
        //    foreach (var m in _context.DumaMembers.ToList())
        //        Members.Add(m);
        //}



        private void LoadMembers()
        {
            Members.Clear();
            var members = _context.DumaMembers
                                  .Include(m => m.CommissionMemberships)
                                      .ThenInclude(cm => cm.Commission)
                                  .ToList();
            foreach (var m in members)
                Members.Add(m);
        }



        // Команды CRUD
        private RelayCommand addCommand;
        public ICommand AddCommand => addCommand ??= new RelayCommand(obj =>
        {
            var vm = new MemberEditWindowViewModel();
            var view = new MemberEditWindow { DataContext = vm };
            if (view.ShowDialog() == true)
            {
                _context.DumaMembers.Add(vm.Member);
                _context.SaveChanges();
                LoadMembers();
            }
        });

        private RelayCommand editCommand;
        public ICommand EditCommand => editCommand ??= new RelayCommand(obj =>
        {
            if (SelectedMember == null) return;

            var vm = new MemberEditWindowViewModel(SelectedMember);
            var view = new MemberEditWindow { DataContext = vm };
            if (view.ShowDialog() == true)
            {
                _context.SaveChanges();
                LoadMembers();
            }
        });

        private RelayCommand deleteCommand;
        public ICommand DeleteCommand => deleteCommand ??= new RelayCommand(obj =>
        {
            if (SelectedMember == null) return;

            if (MessageBox.Show($"Удалить {SelectedMember.Name}?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _context.DumaMembers.Remove(SelectedMember);
                _context.SaveChanges();
                LoadMembers();
            }
        });
    }
}
