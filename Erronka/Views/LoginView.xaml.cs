using Erronka.Data;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Erronka.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;
            string password = PasswordBox.Text;

            using (var db = new DatabaseContext())
            {
                var user = db.Erabiltzaileak
                    .FirstOrDefault(u => u.izena == username && u.pasahitza == password);

                if (user != null)
                {
                    MessageBox.Show($"Ongi etorri, {user.izena}!", "Login arrakastatsua",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    ErreserbakWindow erreserbak = new ErreserbakWindow(user.id);

                    MainWindow main = new MainWindow(user.id, user.rola);
                    main.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Erabiltzailea edo pasahitza okerra da.",
                        "Errorea", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}

