using Nothing.ViewModel;
using System.Windows;
using System.Windows.Controls;


namespace Nothing.View
{
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
            DataContext = new AuthViewModel();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is AuthViewModel vm)
            {
                vm.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}

//using Nothing.ViewModel;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

//namespace Nothing.View;
//public partial class AuthWindow : Window
//{
//    public AuthWindow()
//    {
//        InitializeComponent();
//        DataContext = new AuthViewModel();
//    }

//    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
//    {
//        if (DataContext is AuthViewModel vm)
//        {
//            vm.Password = ((PasswordBox)sender).Password;
//        }
//    }
//}

//namespace Nothing.View
//{
//    /// <summary>
//    /// Логика взаимодействия для AuthWindow.xaml
//    /// </summary>
//    public partial class AuthWindow : Window
//    {
//        public AuthWindow()
//        {
//            InitializeComponent();
//        }

//        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
//        {
//            if (DataContext is AuthViewModel vm)
//            {
//                vm.Password = ((PasswordBox)sender).Password;
//            }
//        }
//    }
//}
