using System.Windows;
using System.Windows.Input;
using Nothing.Model;
using Sample.Model;

namespace Nothing.ViewModel
{
    public class MemberEditWindowViewModel : BaseClass
    {
        public DumaMember Member { get; set; }

        public MemberEditWindowViewModel()
        {
            Member = new DumaMember();
        }

        public MemberEditWindowViewModel(DumaMember member)
        {
            Member = member;
        }

        private RelayCommand saveCommand;
        public ICommand SaveCommand => saveCommand ??= new RelayCommand(obj =>
        {
            if (obj is Window window)
            {
                window.DialogResult = true;
                window.Close();
            }
        });

        private RelayCommand cancelCommand;
        public ICommand CancelCommand => cancelCommand ??= new RelayCommand(obj =>
        {
            if (obj is Window window)
            {
                window.DialogResult = false;
                window.Close();
            }
        });
    }
}
