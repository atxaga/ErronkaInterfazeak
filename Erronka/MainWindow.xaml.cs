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
        public MainWindow()
        {
            InitializeComponent();

            MainContent.Content = new Views.TicketView();
        }

        private void Button_Erreserbak_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.ErreserbakView();
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
    }
}