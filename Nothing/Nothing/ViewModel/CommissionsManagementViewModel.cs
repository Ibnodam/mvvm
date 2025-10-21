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
    public class CommissionsManagementViewModel : BaseClass
    {
        private readonly CouncyContext _context;

        public ObservableCollection<Commission> Commissions { get; set; } = new();
        public ObservableCollection<DumaMember> AvailableMembers { get; set; } = new();

        private Commission _selectedCommission;
        public Commission SelectedCommission
        {
            get => _selectedCommission;
            set
            {
                _selectedCommission = value;
                OnPropertyChanged();
                if (value != null)
                {
                    LoadChairmanForSelectedCommission();
                }
            }
        }

        private DumaMember _selectedChairman;
        public DumaMember SelectedChairman
        {
            get => _selectedChairman;
            set { _selectedChairman = value; OnPropertyChanged(); }
        }

        public CommissionsManagementViewModel()
        {
            _context = new CouncyContext();
            LoadCommissions();
            LoadAvailableMembers();
        }

        private void LoadCommissions()
        {
            Commissions.Clear();
            var commissions = _context.Commissions
                .Include(c => c.CommissionChairs)
                .ThenInclude(ch => ch.Member)
                .AsNoTracking()
                .ToList();

            foreach (var commission in commissions)
            {
                Commissions.Add(commission);
            }
        }

        private void LoadAvailableMembers()
        {
            AvailableMembers.Clear();
            foreach (var member in _context.DumaMembers.AsNoTracking().ToList())
            {
                AvailableMembers.Add(member);
            }
        }

        private void LoadChairmanForSelectedCommission()
        {
            if (SelectedCommission?.CommissionId > 0)
            {
                var chair = _context.CommissionChairs
                    .Include(ch => ch.Member)
                    .AsNoTracking()
                    .FirstOrDefault(ch => ch.CommissionId == SelectedCommission.CommissionId);

                SelectedChairman = chair?.Member;
            }
            else
            {
                SelectedChairman = null;
            }
        }

        private RelayCommand _addCommissionCommand;
        public ICommand AddCommissionCommand => _addCommissionCommand ??= new RelayCommand(obj =>
        {
            var newCommission = new Commission
            {
                Name = "Новая комиссия",
                Profile = "Описание комиссии"
            };

            Commissions.Add(newCommission);
            SelectedCommission = newCommission;
            SelectedChairman = null;
        });

        private RelayCommand _saveCommissionCommand;
        public ICommand SaveCommissionCommand => _saveCommissionCommand ??= new RelayCommand(obj =>
        {
            if (SelectedCommission == null)
            {
                MessageBox.Show("Выберите комиссию для сохранения!");
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedCommission.Name))
            {
                MessageBox.Show("Введите название комиссии!");
                return;
            }

            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // Определяем, новая это комиссия или существующая
                        bool isNewCommission = SelectedCommission.CommissionId == 0;

                        if (isNewCommission)
                        {

                            _context.Commissions.Add(SelectedCommission);
                            _context.SaveChanges();
                        }
                        else
                        {
 
                            var existingCommission = _context.Commissions
                                .FirstOrDefault(c => c.CommissionId == SelectedCommission.CommissionId);

                            if (existingCommission != null)
                            {
                                existingCommission.Name = SelectedCommission.Name;
                                existingCommission.Profile = SelectedCommission.Profile;
                            }
                        }

                        if (SelectedChairman != null)
                        {
                            var existingChair = _context.CommissionChairs
                                .FirstOrDefault(ch => ch.CommissionId == SelectedCommission.CommissionId);

                            if (existingChair != null)
                            {

                                existingChair.MemberId = SelectedChairman.MemberId;
                                existingChair.StartDate = DateTime.Now;
                                existingChair.EndDate = null; 
                            }
                            else
                            {
                                var newChair = new CommissionChair
                                {
                                    CommissionId = SelectedCommission.CommissionId,
                                    MemberId = SelectedChairman.MemberId,
                                    StartDate = DateTime.Now
                                };
                                _context.CommissionChairs.Add(newChair);
                            }
                        }
                        else
                        {
  
                            var existingChair = _context.CommissionChairs
                                .FirstOrDefault(ch => ch.CommissionId == SelectedCommission.CommissionId);

                            if (existingChair != null)
                            {
                                _context.CommissionChairs.Remove(existingChair);
                            }
                        }

                        _context.SaveChanges();
                        transaction.Commit();

                        LoadCommissions();
                        MessageBox.Show("Комиссия сохранена успешно!");
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}\n\nДетали: {ex.InnerException?.Message}");
            }
        });


        private RelayCommand _deleteCommissionCommand;
        public ICommand DeleteCommissionCommand => _deleteCommissionCommand ??= new RelayCommand(obj =>
        {
            if (SelectedCommission == null) return;


            var hasMembers = _context.CommissionMemberships
                .Any(cm => cm.CommissionId == SelectedCommission.CommissionId);
            var hasMeetings = _context.CommissionMeetings
                .Any(cm => cm.CommissionId == SelectedCommission.CommissionId);

            if (hasMembers || hasMeetings)
            {
                MessageBox.Show("Невозможно удалить комиссию, так как с ней связаны члены или заседания!\nСначала удалите все связанные записи.");
                return;
            }

            var result = MessageBox.Show(
                $"Вы уверены, что хотите удалить комиссию '{SelectedCommission.Name}'?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                try
                {

                    var chairs = _context.CommissionChairs
                        .Where(ch => ch.CommissionId == SelectedCommission.CommissionId)
                        .ToList();
                    _context.CommissionChairs.RemoveRange(chairs);

 
                    _context.Commissions.Remove(SelectedCommission);
                    _context.SaveChanges();

                    Commissions.Remove(SelectedCommission);
                    MessageBox.Show("Комиссия удалена успешно!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}");
                }
            }
        });


        private RelayCommand _closeCommand;
        public ICommand CloseCommand => _closeCommand ??= new RelayCommand(obj =>
        {
            if (obj is Window window) window.Close();
        });
    }
}