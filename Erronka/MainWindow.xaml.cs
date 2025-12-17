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
        private string userRole;
        private ObservableCollection<Ticket> ticketItems = new ObservableCollection<Ticket>();
        private readonly DatabaseContext _context = new DatabaseContext();
        private ObservableCollection<Produktua> _produktuak = new ObservableCollection<Produktua>();
        private TicketView ticketView;


        public MainWindow(int loggedUserId, string role)
        {
            InitializeComponent();
            userId = loggedUserId;
            userRole = role;
            
            ticketView = new TicketView();
            MainContent.Content = ticketView;

            ApplyRoleRestrictions();

            CategoryList.SelectionChanged += CategoryList_SelectionChanged;
        }

        private void ApplyRoleRestrictions()
        {
            bool isAdmin = string.Equals(userRole, "admin", StringComparison.OrdinalIgnoreCase);

            if (!isAdmin)
            {
                if (BtnBiltegia != null) BtnBiltegia.IsEnabled = false;
                if (BtnErabiltzaileak != null) BtnErabiltzaileak.IsEnabled = false;
            }
        }

        private bool CheckAdminAccess(string featureName)
        {
            bool isAdmin = string.Equals(userRole, "admin", StringComparison.OrdinalIgnoreCase);
            if (!isAdmin)
            {
                MessageBox.Show($"Ezin da sartu: {featureName}. Admin rola behar da.", "Sarbidea ukatua",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return isAdmin;
        }

        private void Button_Erreserbak_Click(object sender, RoutedEventArgs e)
        {
            
            ErreserbakWindow win = new ErreserbakWindow(userId);
            win.Show();
        }

        private void Button_Stock_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckAdminAccess("Biltegia")) return;
            MainContent.Content = new Views.StockView();
        }

        private void Button_Erabiltzaileak_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckAdminAccess("Erabiltzaileak")) return;
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
            // StockViewrako kodea
            if (int.TryParse(Emaitza.Text, out int number))
            {
                
                this.Tag = number;
            }

            // TicketViewrako kodea
            var main = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            if (main != null && main.ticketView != null)
            {
                var selected = main.ticketView.OrderItemsGrid.SelectedItem as Ticket;
                if (selected != null)
                {
                    main.ticketView.AldatuKantitatea(number);
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

        public int Kalkulatu(string datoInicial)
        {
            Emaitza.Text = datoInicial;
            Tag = null;

            while (Tag == null)
            {
                DoEvents();
               Thread.Sleep(50);
            }

            return Convert.ToInt32(Tag);
        }

        void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Background,
                new Action(delegate { })
            );
        }

    }
}