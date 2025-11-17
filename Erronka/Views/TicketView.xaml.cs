using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Erronka.Models;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using Microsoft.Win32;
using System.Diagnostics;
using PdfSharp.Fonts;

namespace Erronka.Views
{
    public partial class TicketView : UserControl
    {
        private ObservableCollection<Ticket> ticketItems =
            new ObservableCollection<Ticket>();

        public TicketView()
        {
            InitializeComponent();
            OrderItemsGrid.ItemsSource = ticketItems;
        }

        public void AddProduct(string izena, decimal prezioa, int id)
        {
            var existing = ticketItems.FirstOrDefault(x => x.id == id);

            if (existing != null)
            {
                existing.Kantitatea++;
            }
            else
            {
                ticketItems.Add(new Ticket
                {
                    id = id,
                    Izena = izena,
                    Prezioa = prezioa,
                    Kantitatea = 1
                });
            }

            OrderItemsGrid.Items.Refresh();
        }

        public void AldatuKantitatea(int cantidad)
        {
            if (OrderItemsGrid.SelectedItem is Ticket item)
            {
                item.Kantitatea = cantidad;
                OrderItemsGrid.Items.Refresh();
            }
        }

        public decimal GetTotal()
        {
            return ticketItems.Sum(x => x.Totala);
        }

        private void TicketBerria_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ticketItems.Clear();
        }

        private void EzabatuItem(object sender, System.Windows.RoutedEventArgs e)
        {
            if (OrderItemsGrid.SelectedItem is Ticket item)
            {
                ticketItems.Remove(item);
                OrderItemsGrid.Items.Refresh();
            }
        }

        public void ImprimatuTicket()
        {
            if (ticketItems.Count == 0) return;

            PdfDocument document = new PdfDocument();
            document.Info.Title = "Ticket TPV";

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            GlobalFontSettings.UseWindowsFontsUnderWindows = true;
            var font = new XFont("Arial", 10, XFontStyleEx.Regular);
            XFont fontBold = new XFont("Arial", 10, XFontStyleEx.Bold);

            double y = 20;

            gfx.DrawString("Elkartea TPV Ticket", fontBold, XBrushes.Black, new XRect(0, y, page.Width, 20), XStringFormats.TopCenter);
            y += 40;

            gfx.DrawString("Artikuloa", fontBold, XBrushes.Black, 20, y);
            gfx.DrawString("Prezioa", fontBold, XBrushes.Black, 220, y);
            gfx.DrawString("Kantitatea", fontBold, XBrushes.Black, 320, y);
            gfx.DrawString("Zenbatekoa", fontBold, XBrushes.Black, 420, y);
            y += 25;

            decimal total = 0;
            foreach (var item in ticketItems)
            {
                gfx.DrawString(item.Izena, font, XBrushes.Black, 20, y);
                gfx.DrawString(item.Prezioa.ToString("0.00"), font, XBrushes.Black, 220, y);
                gfx.DrawString(item.Kantitatea.ToString(), font, XBrushes.Black, 320, y);
                gfx.DrawString(item.Totala.ToString("0.00"), font, XBrushes.Black, 420, y);
                total += item.Totala;
                y += 20;
            }

            y += 20;
            gfx.DrawString("TOTAL: " + total.ToString("0.00") + " €", fontBold, XBrushes.Black, 20, y);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF file|*.pdf";
            saveFileDialog.FileName = "Ticket.pdf";
            if (saveFileDialog.ShowDialog() == true)
            {
                document.Save(saveFileDialog.FileName);
                Process.Start(new ProcessStartInfo(saveFileDialog.FileName) { UseShellExecute = true });
            }
        }
    }
}
