using Erronka.Data;
using Erronka.Models;
using Erronka.Views;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Erronka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int userId;
        private ObservableCollection<Ticket> ticketItems = new ObservableCollection<Ticket>();
        private readonly DatabaseContext _context = new DatabaseContext();
        private ObservableCollection<Produktua> _produktuak = new ObservableCollection<Produktua>();
        private TicketView ticketView;


        public MainWindow(int loggedUserId)
        {
            InitializeComponent();
            userId = loggedUserId;
            
            ticketView = new TicketView();
            MainContent.Content = ticketView;
            CategoryList.SelectionChanged += CategoryList_SelectionChanged;
        }

        private void Button_Erreserbak_Click(object sender, RoutedEventArgs e)
        {
            ErreserbakWindow win = new ErreserbakWindow(userId);
            win.Show();
        }

        private void Button_Stock_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.StockView();
        }

        private void Button_Erabiltzaileak_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.ErabiltzaileakView();
        }

        private void Button_Ticket_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.TicketView();
        }

        private void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (CategoryList.SelectedItem is ListBoxItem item)
            {
                string kategoria = item.Content.ToString();
                ProduktuaKargatu(kategoria);
            }
        }

        private void ProduktuaKargatu(string kategoria)
        {
            var productos = _context.Produktuak
                .Where(p => p.kategoria == kategoria)
                .Select(p => new
                {
                    Name = p.izena,
                    Price = p.prezioa,
                    ProductId = p.id
                })
                .ToList();

            ProductList.ItemsSource = productos;
        }

        private void Produktua_Click(object sender, MouseButtonEventArgs e)
        {
            dynamic p = ((FrameworkElement)sender).DataContext;

            var main = (MainWindow)Application.Current.Windows
              .OfType<MainWindow>()
              .FirstOrDefault();

            if (main.ticketView != null)
            {
                main.ticketView.AddProduct(
                    p.Name,        
                    p.Price,        
                    p.ProductId    
                );
            }
        }

        private void Zenbakia_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button btn)) return;
            if (!int.TryParse(btn.Content.ToString(), out int number)) return;

            Emaitza.Text += number.ToString();
        }

        private void KantitateaAldatu(object sender, RoutedEventArgs e)
        {

            var number = int.Parse(Emaitza.Text);
            var main = Application.Current.Windows
                          .OfType<MainWindow>()
                          .FirstOrDefault();

            if (main != null && main.ticketView != null)
            {
                var selected = main.ticketView.OrderItemsGrid.SelectedItem as Ticket;
                if (selected != null)
                {
                    int nuevaCantidad = number;
                    main.ticketView.AldatuKantitatea(nuevaCantidad);
                }
            }

        }

        private void Button_CE_Click(object sender, RoutedEventArgs e)
        {
            Emaitza.Text = "";
        }

        private void Button_Imprimir_Click(object sender, RoutedEventArgs e)
        {
            if (ticketView != null)
            {
                ticketView.ImprimatuTicket();
            }
        }
    }
}