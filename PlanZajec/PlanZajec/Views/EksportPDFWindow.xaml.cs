using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using PlanZajec.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PlanZajec.Views
{
    /// <summary>
    /// Klasa pozwalająca zapisać plan w formacie PDF
    /// </summary>
    public partial class EksportPDFWindow : Window
    {
        public ObservableCollection<Plany> plany { get; private set; }
        private long[] tab;
        /// <summary>
        /// Konstruktor pobierający plany i je wyświetlajćy w combobox
        /// </summary>
        public EksportPDFWindow()
        {
            InitializeComponent();
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
        /// Metoda zapisująca pdf
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
        /// Metoda tworząca bitmapę planu
        /// </summary>
        /// <param name="view">Widok planu</param>
        /// <returns>Bitmapa planu</returns>
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
        /*
        public static void createPdfFromImage(string imageFile, string pdfFile)

        {

            using (var ms = new MemoryStream())

            {

                var document = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER.Rotate(), 0, 0, 0, 0);

                PdfWriter.GetInstance(document, new FileStream(pdfFile, FileMode.Create));

                iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms).SetFullCompression();

                document.Open();

                FileStream fs = new FileStream(imageFile, FileMode.Open);

                var image = iTextSharp.text.Image.GetInstance(fs);

                image.ScaleToFit(document.PageSize.Width, document.PageSize.Height);

                document.Add(image);

                document.Close();
            }
       }
       */
    }
}
