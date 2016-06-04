using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using PlanZajec.ViewModels;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PlanZajec.Views
{
    /// <summary>
    /// Klasa pozwalająca zapisać plan jako plik graficzny
    /// </summary>
    public partial class EksportGraficznyWindow : Window
    {
        public ObservableCollection<Plany> plany { get; private set; }
        private long[] tab;
        /// <summary>
        /// Konstruktor, który pobiera plany i wyświetla ich nazwy w combobox
        /// </summary>
        public EksportGraficznyWindow()
        {
            InitializeComponent();
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                plany = new ObservableCollection<Plany>(uw.Plany.GetAll().ToList());
            }
            tab = new long[plany.Count()];
            int i = 0;
            foreach (Plany plan in plany)
            {
                PlanyComboBox.Items.Add(plan.NazwaPlanu);
                tab[i] = plan.IdPlanu;
                i++;
            }
            PlanyComboBox.SelectedIndex = 0;
        }
        /// <summary>
        /// Metoda zapisująca obraz
        /// </summary>

        private void Zapisz_Click(object sender, RoutedEventArgs e)
        {
            PlanViewModel pvm = new PlanViewModel(tab[PlanyComboBox.SelectedIndex]);
            PlanView pv = new PlanView(pvm);
            RenderTargetBitmap src = GetImage(pv);
            Stream outputStream = File.Create("C:\\File.png");
            SaveAsPng(src, outputStream);

        }

        /// <summary>
        /// Metoda tworząca bitmapę na podstawie planu
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
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
        /// Metoda zapisująca konwertująca bitmapę ma obraz w rozszerzeniu png
        /// </summary>
        /// <param name="src">Obraz źródłowy</param>
        /// <param name="outputStream">Strumień do zapisu pliku</param>
        public static void SaveAsPng(RenderTargetBitmap src, Stream outputStream)
        {
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(src));
            encoder.Save(outputStream);
        }
    }
}
