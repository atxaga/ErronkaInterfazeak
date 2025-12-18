using Erronka.Data;
using Erronka.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

            using (var db = new DatabaseContext())
            {
                var ids = ticketItems.Select(t => t.id).Distinct().ToList();
                var productos = db.Produktuak
                    .Where(p => ids.Contains(p.id))
                    .ToDictionary(p => p.id, p => p);

                foreach (var item in ticketItems)
                {
                    if (!productos.TryGetValue(item.id, out var prod))
                    {
                        MessageBox.Show($"Produktua ez da existitzen: {item.Izena}", "Errorea",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (prod.stock < item.Kantitatea)
                    {
                        MessageBox.Show($"Ez dago stock nahikorik: {item.Izena} (eskuragarri {prod.stock}, behar {item.Kantitatea})",
                            "Stock eskasa", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                PdfDocument document = new PdfDocument();
                document.Info.Title = "Ticket TPV";
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                GlobalFontSettings.UseWindowsFontsUnderWindows = true;
                var font = new XFont("Arial", 10, XFontStyleEx.Regular);
                var fontBold = new XFont("Arial", 10, XFontStyleEx.Bold);

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

                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF file|*.pdf",
                    FileName = "Ticket.pdf"
                };
                if (saveFileDialog.ShowDialog() == true)
                {
                    document.Save(saveFileDialog.FileName);
                    Process.Start(new ProcessStartInfo(saveFileDialog.FileName) { UseShellExecute = true });

                    foreach (var item in ticketItems)
                    {
                        var prod = productos[item.id];
                        prod.stock -= item.Kantitatea;

                        db.Entry(prod).Property(p => p.stock).IsModified = true;
                    }

                    try
                    {
                        db.SaveChanges();
                        MessageBox.Show("Stock eguneratua eta ticketa gordeta.", "Arrakasta",
                            MessageBoxButton.OK, MessageBoxImage.Information);

                        ticketItems.Clear();
                        OrderItemsGrid.Items.Refresh();
                    }
                    catch (DbUpdateException ex)
                    {
                        MessageBox.Show($"DB eguneratze-errorea: {ex.InnerException?.Message ?? ex.Message}", "Errorea",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ezin da stocka eguneratu: {ex.Message}", "Errorea",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
