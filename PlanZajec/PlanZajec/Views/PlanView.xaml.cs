using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PlanZajec.ViewModels;

namespace PlanZajec.Views
{
    /// <summary>
    ///     Interaction logic for PlanView.xaml
    /// </summary>
    public partial class PlanView : UserControl
    {
        private static PlanView pv;
        private PlanViewModel viewModel;

        public PlanView()
        {
            InitializeComponent();
            viewModel = new PlanViewModel();
            DataContext = viewModel;
            NarysujPlan();
            pv = this;
        }
        private void NarysujPlan()
        {
            for (var i = 0; i < viewModel.Kafelki.Count; i++)
            {
                var kafelek = viewModel.Kafelki[i];
                var numerDnia = 0;
                switch (viewModel.ListaGrupZajeciowych[i].Dzień)
                {
                    case "pn":
                        numerDnia = 0;
                        break;
                    case "wt":
                        numerDnia = 1;
                        break;
                    case "śr":
                        numerDnia = 2;
                        break;
                    case "cz":
                        numerDnia = 3;
                        break;
                    case "pt":
                        numerDnia = 4;
                        break;
                    case "sb":
                        numerDnia = 5;
                        break;
                    case "nd":
                        numerDnia = 6;
                        break;
                }
                //poprawka dla 14 row
                numerDnia *= 2;

                var godzinaRozpoczeciaString = viewModel.ListaGrupZajeciowych[i].Godzina;
                var godzinaRozpoczecia = int.Parse(godzinaRozpoczeciaString.Split(':')[0]) +
                                         (int.Parse(godzinaRozpoczeciaString.Split(':')[1]) <= 30 ? 0 : 1);
                var godzinaZakonczeniaString = viewModel.ListaGrupZajeciowych[i].GodzinaKoniec;
                var godzinaZakonczenia = int.Parse(godzinaZakonczeniaString.Split(':')[0]) +
                                         (int.Parse(godzinaZakonczeniaString.Split(':')[1]) <= 30 ? 0 : 1);
                var czasTrwania = godzinaZakonczenia - godzinaRozpoczecia;

                TabelaGrup.Children.Add(kafelek);
                switch (viewModel.ListaGrupZajeciowych[i].Tydzien)
                {
                    case "//":
                        Grid.SetRow(kafelek, numerDnia); Grid.SetRowSpan(kafelek, 2);
                        break;
                    case "TN":
                        Grid.SetRow(kafelek, numerDnia);
                        break;
                    case "TP":
                        Grid.SetRow(kafelek, numerDnia+1);
                        break;
                }
                Grid.SetColumn(kafelek, godzinaRozpoczecia - 7);
                Grid.SetColumnSpan(kafelek, czasTrwania);
            }
        }
        private void Usun()
        {
            foreach (var gr in viewModel.Kafelki)
            {
                TabelaGrup.Children.Remove(gr);
            }
        }

        public static void Aktualizuj()
        {
            pv.viewModel = new PlanViewModel();
            pv.DataContext = pv.viewModel;
            pv.Usun();
            pv.NarysujPlan();
        }
        /// <summary>
        /// TabelaGrup resize handler
        /// </summary>
        /// <param name="sender">In this function sender == TabelaGrup</param>
        /// <param name="e">New sizes</param>
        private void TabelaGrup_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var sizeOfTile = new Size(e.NewSize.Width / 7, e.NewSize.Height / 7);
            TabelaGrup.Background = CreateGridBrush(new Rect(0, 0, e.NewSize.Width, e.NewSize.Height), sizeOfTile);
        }
        /// <summary>
        /// Static function that returns painted grid
        /// Basic Template from http://stackoverflow.com/questions/6434284/how-to-draw-gridline-on-wpf-canvas, posted by Jeff Mercado
        /// </summary>
        /// <param name="bounds">Specify starting position and length(startingX,startingY,availableWidth,availableHeight)</param>
        /// <param name="tileSize">Size of one tile</param>
        /// <returns></returns>
        static Brush CreateGridBrush(Rect bounds, Size tileSize)
        {
            var gridColor = Brushes.Black;
            var gridThickness = 1.0;
            var tileRect = new Rect(tileSize);

            var gridTile = new DrawingBrush
            {
                Stretch = Stretch.None,
                TileMode = TileMode.Tile,
                Viewport = tileRect,
                ViewportUnits = BrushMappingMode.Absolute,
                Drawing = new GeometryDrawing
                {
                    Pen = new Pen(gridColor, gridThickness),
                    Geometry = new GeometryGroup
                    {
                        Children = new GeometryCollection
                {
                    new LineGeometry(tileRect.TopLeft, tileRect.TopRight),
                    new LineGeometry(tileRect.BottomLeft, tileRect.BottomRight),
                    new LineGeometry(tileRect.TopLeft, tileRect.BottomLeft),
                    new LineGeometry(tileRect.TopRight, tileRect.BottomRight)
                }
                    }
                }
            };

            var offsetGrid = new DrawingBrush
            {
                Stretch = Stretch.None,
                AlignmentX = AlignmentX.Left,
                AlignmentY = AlignmentY.Top,
                Transform = new TranslateTransform(bounds.Left, bounds.Top),
                Drawing = new GeometryDrawing
                {
                    Geometry = new RectangleGeometry(new Rect(bounds.Size)),
                    Brush = gridTile
                }
            };

            return offsetGrid;
        }
    }
}