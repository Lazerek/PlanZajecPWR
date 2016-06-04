using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PlanZajec.ViewModels;
using PlanZajec.CommonInformations;

namespace PlanZajec.Views
{
    /// <summary>
    ///     Klasa odpowiedzialna za wyświetlanie planu
    /// </summary>
    public partial class PlanView : UserControl
    {
        public double PlanWidth {private set; get; }
        public double PlanHeight { private set; get; }

        private PlanViewModel _planViewModel;

        /// <summary>
        /// Konstruktor wyświetlania planu
        /// </summary>
        /// <param name="plan">ViewModel planu</param>
        public PlanView(PlanViewModel plan)
        {
            InitializeComponent();
            _planViewModel = plan;
            DataContext = _planViewModel;
            PaintSchedule();
        }

        public long GetPlanId()
        {
            return _planViewModel.IdPlanu;
        }
        /// <summary>
        ///     Add each lesson to schedule and set cell/cells to it
        /// </summary>
        private void PaintSchedule()
        {
            for (var i = 0; i < _planViewModel.Kafelki.Count; i++)
            {
                if (_planViewModel.ListaGrupZajeciowych[i].Godzina.Length==5)
                {
                    var lesson = _planViewModel.Kafelki[i];
                    var dayNumber = 0;
                    switch (_planViewModel.ListaGrupZajeciowych[i].Dzień)
                    {
                        case "pn":
                            dayNumber = 0;
                            break;
                        case "wt":
                            dayNumber = 1;
                            break;
                        case "śr":
                            dayNumber = 2;
                            break;
                        case "cz":
                            dayNumber = 3;
                            break;
                        case "pt":
                            dayNumber = 4;
                            break;
                        case "sb":
                            dayNumber = 5;
                            break;
                        case "nd":
                            dayNumber = 6;
                            break;
                    }
                    //poprawka dla 14 row
                    dayNumber *= 2;

                    var startingHourString = _planViewModel.ListaGrupZajeciowych[i].Godzina;
                    var startingHour = int.Parse(startingHourString.Split(':')[0]) +
                                       (int.Parse(startingHourString.Split(':')[1]) <= 30 ? 0 : 1);
                    var endHourString = _planViewModel.ListaGrupZajeciowych[i].GodzinaKoniec;
                    var endHour = int.Parse(endHourString.Split(':')[0]) +
                                  (int.Parse(endHourString.Split(':')[1]) <= 30 ? 0 : 1);
                    var czasTrwania = endHour - startingHour;

                    TabelaGrup.Children.Add(lesson);
                    switch (_planViewModel.ListaGrupZajeciowych[i].Tydzien)
                    {
                        case "//":
                            Grid.SetRow(lesson, dayNumber);
                            Grid.SetRowSpan(lesson, 2);
                            break;
                        case "TN":
                            Grid.SetRow(lesson, dayNumber);
                            break;
                        case "TP":
                            Grid.SetRow(lesson, dayNumber + 1);
                            break;
                    }
                    Grid.SetColumn(lesson, startingHour - 7);
                    Grid.SetColumnSpan(lesson, czasTrwania);
                }
            }
        }

        /// <summary>
        /// Funkcja usuwająca widok planu
        /// </summary>
        private void Delete()
        {
            foreach (var gr in _planViewModel.Kafelki)
            {
                TabelaGrup.Children.Remove(gr);
            }
        }

        /// <summary>
        /// Metoda odświeżająca plan
        /// </summary>
        public static void RefreshSchedule()
        {
            PlanView view = ObslugaWidokuWieluPlanów.Instance.getPlanView(ActChosenPlanSingleton.Instance.IdPlanu);

            view.Delete();
            view._planViewModel.aktualizuj();
            view.PaintSchedule();
        }
       

        /// <summary>
        ///  Obsługa tabeli grup na zmianę rozmiaru
        /// </summary>
        /// <param name="sender">Tabela grup</param>
        /// <param name="e">Nowy rozmiar</param>
        private void TabelaGrup_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            PlanWidth = e.NewSize.Width;
            PlanHeight = e.NewSize.Height;
            var sizeOfTile = new Size(e.NewSize.Width/7, e.NewSize.Height/7);
            TabelaGrup.Background = CreateGridBrush(new Rect(0, 0, e.NewSize.Width, e.NewSize.Height), sizeOfTile);
        }

        /// <summary>
        ///     Generate grid brush
        ///     Basic Template from http://stackoverflow.com/questions/6434284/how-to-draw-gridline-on-wpf-canvas, posted by Jeff
        ///     Mercado
        /// </summary>
        /// <param name="bounds">Specify starting position and length(startingX,startingY,availableWidth,availableHeight)</param>
        /// <param name="tileSize">Size of one tile</param>
        /// <returns>New brush that paints grid</returns>
        private static Brush CreateGridBrush(Rect bounds, Size tileSize)
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