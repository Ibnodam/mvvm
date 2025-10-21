using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Nothing.Model;
using Sample.Model;
using Microsoft.EntityFrameworkCore;

namespace Nothing.ViewModel
{
    public class CommissionsReportViewModel : BaseClass
    {
        private readonly CouncyContext _context;

        public ObservableCollection<Commission> Commissions { get; set; } = new();
        public ObservableCollection<CommissionMembership> CurrentMembers { get; set; } = new();

        private Commission _selectedCommission;
        public Commission SelectedCommission
        {
            get => _selectedCommission;
            set
            {
                _selectedCommission = value;
                OnPropertyChanged();
                LoadCommissionDetails();
            }
        }

        public CommissionsReportViewModel()
        {
            _context = new CouncyContext();
            LoadCommissions();
        }

        private void LoadCommissions()
        {
            Commissions.Clear();

            // Загружаем комиссии с председателями и членами
            var commissions = _context.Commissions
                .Include(c => c.CommissionChairs)
                .ThenInclude(ch => ch.Member)
                .Include(c => c.CommissionMemberships)
                .ThenInclude(m => m.Member)
                .ToList();

            foreach (var commission in commissions)
            {
                Commissions.Add(commission);
            }

            if (Commissions.Any())
                SelectedCommission = Commissions.First();
        }

        private void LoadCommissionDetails()
        {
            CurrentMembers.Clear();

            if (SelectedCommission?.CommissionMemberships != null)
            {
                foreach (var member in SelectedCommission.CommissionMemberships)
                {
                    CurrentMembers.Add(member);
                }
            }
        }

        // Команда обновления данных
        private RelayCommand _refreshCommand;
        public ICommand RefreshCommand => _refreshCommand ??= new RelayCommand(obj =>
        {
            LoadCommissions();
        });

        // Команда закрытия
        private RelayCommand _closeCommand;
        public ICommand CloseCommand => _closeCommand ??= new RelayCommand(obj =>
        {
            if (obj is Window window) window.Close();
        });
    }
}