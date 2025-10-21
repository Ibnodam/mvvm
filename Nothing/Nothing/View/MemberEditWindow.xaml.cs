using Nothing.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Nothing.View
{
    public partial class MemberEditWindow : Window
    {
        public MemberEditWindow()
        {
            InitializeComponent();
        }

        private void PhoneTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Разрешаем только: цифры 0-9, +, -, (, )
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c) && c != '+' && c != '-' && c != '(' && c != ')')
                {
                    e.Handled = true;
                    return;
                }
            }
            e.Handled = false;
        }
    }
}
