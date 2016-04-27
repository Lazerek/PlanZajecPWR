using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using System.ComponentModel;
using System.Windows;
using PlanZajec.Views;
using System;
using PlanZajec.CommonInformations;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //DATABASE LOAD
            DataBaseReturnPoint.PrePrareDB();
            using(var unitOfWork = new UnitOfWork(new PlanPwrContext()))
            {
                if( unitOfWork.Plany.Count() < 1)
                {
                    Plany plan = new Plany() { };
                    unitOfWork.Plany.Add(plan);
                    unitOfWork.SaveChanges();
                    ActChosenPlanSingleton.Instance.SetPlan (plan);
                }
                else
                {
                    ActChosenPlanSingleton.Instance.SetPlan(unitOfWork.Plany.GetFirstOrDefault());
                }
            }
            //Initialize window
            InitializeComponent();
            PGlowny.Children.Add(new PanelGlowny());
            PFiltrow.Children.Add(new PanelFiltrow(this));
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //MENU

        public void ShowParserWindow(object sender, EventArgs e)
        {
            ParserWindow pw = new ParserWindow();
            pw.Show();
        }

        public void menuNowyPlan(object sender, EventArgs e)
        {
            
        }

        public void menuOtworz(object sender, EventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Plan zajęć (*.txt)|*.txt";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                //metoda() - otwiera plan
            }
        }

        public void menuZapisz(object sender, EventArgs e)
        {
            //metoda spawdzajaca dla kazdego planu czy ma sciezke zapisu +
            string fileText = "Wyjscie";

            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "Plan zajęć (*.txt)|*.txt"
            };

            if (dialog.ShowDialog() == true)
            {
                System.IO.File.WriteAllText(dialog.FileName, fileText);
            }

        }

        public void menuZapiszJako(object sender, EventArgs e)
        {
            string fileText = "Wyjscie";

            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "Plan zajęć (*.txt)|*.txt"
            };

            if (dialog.ShowDialog() == true)
            {
                System.IO.File.WriteAllText(dialog.FileName, fileText);
            }
        }

        public void menuDrukuj(object sender, EventArgs e)
        {
            //metoda drukujaca
        }

        public void menuEksportujPDF(object sender, EventArgs e)
        {
            //metoda eksportujaca
        }

        public void menuEksportujPlikGraficzny(object sender, EventArgs e)
        {
            //metoda eksportujaca
        }

        public void menuZakoncz(object sender, EventArgs e)
        {
            this.Close();
        }

        public void menuInformacje(object sender, EventArgs e)
        {
            //okno z informacjami
        }
    }
}
