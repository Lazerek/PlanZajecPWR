using PlanZajec.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PlanZajec.Views
{
    /// <summary>
    /// Klasa pozwalająca eksportować plan
    /// </summary>
    public partial class Eksportuj : Window
    {
        bool typ;
        public Eksportuj()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Konstruktor tworzący okno do eksportu planu
        /// </summary>
        /// <param name="typ">Informacja czy plik ma być eskportowany jako pdf czy graficzny</param>
        public Eksportuj(bool typ)
        {
            InitializeComponent();
            this.DataContext = PlanyViewModel.Instance;
            this.typ = typ;
            if (typ)
            {
                tekst.Visibility = Visibility.Hidden;
                format.Visibility = Visibility.Hidden;
                Grid.SetRow(anulujButton, 4);
                Grid.SetRow(eksportujButton, 4);
                mGrid.RowDefinitions[2].Height = new GridLength(40);
                eksportujButton.Content = "Eksportuj PDF";
                eksportujButton.Width = 100;
                this.Height = 230;
                this.Title = "Eksportuj jako plik PDF";
            }
        }

        private void Eksport(object sender, RoutedEventArgs e)
        {
            if (sciezka.Text.Length < 1)
            {
                System.Windows.MessageBox.Show("Podaj ścieżkę zapisu pliku");
            }
            else
            {
                if (plany.SelectedIndex == -1)
                {
                    System.Windows.MessageBox.Show("Wybierz plan do zapisu");
                }
                else
                {
                    if (typ)
                    {
                        PlanViewModel pvm = new PlanViewModel(plany.SelectedIndex);
                        PlanView pv = new PlanView(pvm);
                        RenderTargetBitmap src = GetImage(pv);
                        Stream outputStream = File.Create(sciezka.Text + "/" + plany.Text + ".png");
                        SaveAsPng(src, outputStream);
                    }
                    else
                    {
                        if (format.SelectedIndex == -1)
                        {
                            System.Windows.MessageBox.Show("Podaj format pliku");
                        }
                        else
                        {
                            PlanViewModel pvm = new PlanViewModel(plany.SelectedIndex);
                            PlanView pv = new PlanView(pvm);
                            RenderTargetBitmap src = GetImage(pv);
                            Stream outputStream = File.Create(sciezka.Text + "/" + plany.Text + ".png");
                            SaveAsPng(src, outputStream);
                        }
                    }
                }
            }
        }

        private void wyborSciezki(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog directchoosedlg = new FolderBrowserDialog();
            directchoosedlg.ShowDialog();
            sciezka.Text = directchoosedlg.SelectedPath;
        }

        private void Anuluj(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public static RenderTargetBitmap GetImage(PlanView view)
        {
            Size size = new Size(view.PlanWidth, view.PlanHeight);
            if (size.IsEmpty)
                return null;

            RenderTargetBitmap result = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);

            DrawingVisual drawingvisual = new DrawingVisual();
            using (DrawingContext context = drawingvisual.RenderOpen())
            {
                context.DrawRectangle(new VisualBrush(view), null, new Rect(new Point(), size));
                context.Close();
            }

            result.Render(drawingvisual);
            return result;
        }
        /// <summary>
        /// Metoda zapisująca bitmapę jako png
        /// </summary>
        /// <param name="src">Źródło obrazka</param>
        /// <param name="outputStream">Strumień zapisu</param>
        public static void SaveAsPng(RenderTargetBitmap src, Stream outputStream)
        {
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(src));
            encoder.Save(outputStream);
        }

        public static void SaveAsPng(RenderTargetBitmap src, string targetFile)

        {

            PngBitmapEncoder encoder = new PngBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(src));



            using (var stm = System.IO.File.Create(targetFile))

            {

                encoder.Save(stm);

            }

        }
    }
}