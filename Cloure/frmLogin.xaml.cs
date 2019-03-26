using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Cloure
{
    /// <summary>
    /// Lógica de interacción para frmLogin.xaml
    /// </summary>
    public partial class frmLogin : Window
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {

            App.cloureLib.AddParameter("topic", "login");
            App.cloureLib.AddParameter("user", txtUser.Text);
            App.cloureLib.AddParameter("pass", txtPass.Password);

            string res = await App.cloureLib.ExecuteCommand();

            MessageBox.Show(res);
        }
    }
}
