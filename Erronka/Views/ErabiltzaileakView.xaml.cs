using Erronka.Data;
using Erronka.Models;
using Erronka.Views;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Erronka.Views
{
    public partial class ErabiltzaileakView : UserControl
    {
        private readonly DatabaseContext _context = new DatabaseContext();

        public ErabiltzaileakView()
        {
            InitializeComponent();
            erabiltzaileakKargatu();
        }

        private void erabiltzaileakKargatu()
        {
            UsersGrid.ItemsSource = _context.Erabiltzaileak.ToList();
        }

        private void Button_Gehitu_Click(object sender, RoutedEventArgs e)
        {
            AddEditUserWindow win = new AddEditUserWindow();
            if (win.ShowDialog() == true)
                erabiltzaileakKargatu();
        }

        private void Button_Aldatu_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem is not Erabiltzailea u)
            {
                MessageBox.Show("Aukeratu erabiltzaile bat.");
                return;
            }

            AddEditUserWindow win = new AddEditUserWindow(u);

            if (win.ShowDialog() == true)
                erabiltzaileakKargatu();
        }

        private void Button_Ezabatu_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem is not Erabiltzailea u)
            {
                MessageBox.Show("Aukeratu erabiltzaile bat.");
                return;
            }

            if (MessageBox.Show("Ezabatu nahi duzu?", "Konfirmazioa",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _context.Erabiltzaileak.Remove(u);
                _context.SaveChanges();
                erabiltzaileakKargatu();
            }
        }
    }
}
