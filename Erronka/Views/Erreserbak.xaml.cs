using Erronka.Data;
using Erronka.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Erronka.Views
{
    public partial class ErreserbakWindow : Window
    {
        private int mesaSeleccionada = 0;
        private readonly DatabaseContext _context;
        private int usuarioId; 

        public ErreserbakWindow(int loggedUserId)
        {
            InitializeComponent();
            _context = new DatabaseContext();
            usuarioId = loggedUserId;

            FechaPicker.SelectedDatesChanged += FechaPicker_SelectedDatesChanged;

            ActualizarMesas();
        }

        private void Button_Mahia_Click(object sender, RoutedEventArgs e)
        {
            Button mesa = sender as Button;
            mesaSeleccionada = int.Parse(mesa.Tag.ToString());
            MessageBox.Show($"Mesa seleccionada: {mesaSeleccionada}");
        }

        private void Reservar_Click(object sender, RoutedEventArgs e)
        {
            if (mesaSeleccionada == 0)
            {
                MessageBox.Show("¡Aukeratu mahi bat!");
                return;
            }

            string turno = ComidaRB.IsChecked == true ? ComidaRB.Content.ToString().Trim() : CenaRB.Content.ToString().Trim();
            string cliente = ClienteTxt.Text;

            if (string.IsNullOrWhiteSpace(cliente))
            {
                MessageBox.Show("Bete galdetegi osoa.");
                return;
            }

            DateTime fecha = FechaPicker.SelectedDate?.Date ?? DateTime.Today;

            bool existe = _context.Erreserbak.Any(r =>
                r.mahi_id == mesaSeleccionada &&
                r.data == fecha &&  
                r.mota == turno);

            if (existe)
            {
                MessageBox.Show("¡Mahi hau dagoeneko aukeratuta dago!");
                return;
            }

            Erreserba nueva = new Erreserba
            {
                mahi_id = mesaSeleccionada,
                erabiltzaile_id = usuarioId,
                izena = cliente,
                data = fecha,  
                mota = turno
            };

            _context.Erreserbak.Add(nueva);
            _context.SaveChanges();

            MessageBox.Show($"¡Mahia erreserbatu da!\nMahia: {mesaSeleccionada}\nData: {fecha:dd/MM/yyyy}\nMota: {turno}\nBezeroa: {cliente}");

            ActualizarMesas();
        }

        private void FechaPicker_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            ActualizarMesas();
        }

        private void ActualizarMesas()
        {
            DateTime fecha = FechaPicker.SelectedDate?.Date ?? DateTime.Today;
            string turno = ComidaRB.IsChecked == true ? ComidaRB.Content.ToString().Trim() : CenaRB.Content.ToString().Trim();

            foreach (UIElement element in MahiakGrid.Children)
            {
                if (element is Button btn)
                {
                    int mesa = int.Parse(btn.Tag.ToString());

                    // Solo mira reservas para la fecha y el turno actual
                    bool reservadaEnTurno = _context.Erreserbak.Any(r =>
                        r.mahi_id == mesa &&
                        r.data == fecha &&
                        r.mota == turno);

                    btn.Background = reservadaEnTurno ? Brushes.Red : (Brush)new BrushConverter().ConvertFrom("#C6FFC6");
                }
            }
        }

    }
}
