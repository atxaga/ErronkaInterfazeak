using Erronka.Data;
using Erronka.Models;
using System.Windows;

namespace Erronka.Views
{
    public partial class AddEditProduktuaWindow : Window
    {
        private readonly DatabaseContext _context = new DatabaseContext();
        private Produktua _produktua;

        public AddEditProduktuaWindow(Produktua produktua = null)
        {
            InitializeComponent();
            _produktua = produktua;

            if (_produktua != null)
            {
                TxtIzena.Text = produktua.izena;
                TxtKategoria.Text = produktua.kategoria;
                TxtPrezioa.Text = produktua.prezioa.ToString();
                TxtStock.Text = produktua.stock.ToString();
            }
        }

        private void Gorde_Click(object sender, RoutedEventArgs e)
        {
            if (_produktua == null)
            {
                _produktua = new Produktua();
                _context.Produktuak.Add(_produktua);
            }

            _produktua.izena = TxtIzena.Text;
            _produktua.kategoria = TxtKategoria.Text;

            if (decimal.TryParse(TxtPrezioa.Text, out decimal prezio))
                _produktua.prezioa = prezio;

            if (int.TryParse(TxtStock.Text, out int stock))
                _produktua.stock = stock;

            _context.SaveChanges();
            DialogResult = true;
            Close();
        }
    }
}
