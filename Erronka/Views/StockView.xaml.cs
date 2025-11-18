using Erronka.Data;
using Erronka.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Erronka.Views
{
    public partial class StockView : UserControl
    {
        private readonly DatabaseContext _context;

        public StockView()
        {
            InitializeComponent();
            _context = new DatabaseContext();
            CargarProductos();
        }

        private void CargarProductos()
        {
            StockGrid.ItemsSource = _context.Produktuak.ToList();
        }

        private void StockGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (StockGrid.SelectedItem is not Produktua p)
                return;

            var main = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            if (main == null) return;

            int resultado = main.Kalkulatu(p.stock.ToString());
            int nuevoStock = resultado;

            p.stock = nuevoStock;

            _context.SaveChanges();

            CargarProductos();
        }

        private void Button_Gehitu_Click(object sender, RoutedEventArgs e)
        {
            AddEditProduktuaWindow win = new AddEditProduktuaWindow();
            if (win.ShowDialog() == true)
                CargarProductos();
        }

        private void Button_Aldatu_Click(object sender, RoutedEventArgs e)
        {
            if (StockGrid.SelectedItem is not Produktua p)
            {
                MessageBox.Show("Aukeratu produktu bat.");
                return;
            }

            AddEditProduktuaWindow win = new AddEditProduktuaWindow(p);
            if (win.ShowDialog() == true)
                CargarProductos();
        }

        private void Button_Ezabatu_Click(object sender, RoutedEventArgs e)
        {
            if (StockGrid.SelectedItem is not Produktua p)
            {
                MessageBox.Show("Aukeratu produktu bat.");
                return;
            }

            if (MessageBox.Show("Ziur zaude ezabatu nahi duzula?",
                "Konfirmazioa", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _context.Produktuak.Remove(p);
                _context.SaveChanges();
                CargarProductos();
            }
        }

    }
}
