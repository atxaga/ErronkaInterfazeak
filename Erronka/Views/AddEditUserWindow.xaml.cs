using Erronka.Data;
using Erronka.Models;
using System.Windows;
using System.Windows.Controls;

namespace Erronka.Views
{
    public partial class AddEditUserWindow : Window
    {
        private readonly DatabaseContext _context = new DatabaseContext();
        private Erabiltzailea _user;

        public AddEditUserWindow(Erabiltzailea user = null)
        {
            InitializeComponent();
            _user = user;

            if (_user != null)
            {
                TxtIzena.Text = _user.Izena;
                TxtPasahitza.Password = _user.Pasahitza;
                CbRola.Text = _user.Rola;
            }
        }

        private void Gorde_Click(object sender, RoutedEventArgs e)
        {
            if (_user == null)
            {
                _user = new Erabiltzailea();
                _context.Erabiltzaileak.Add(_user);
            }

            _user.Izena = TxtIzena.Text;
            _user.Pasahitza = TxtPasahitza.Password;
            _user.Rola = (CbRola.SelectedItem as ComboBoxItem)?.Content.ToString();

            _context.SaveChanges();

            DialogResult = true;
            Close();
        }
    }
}
