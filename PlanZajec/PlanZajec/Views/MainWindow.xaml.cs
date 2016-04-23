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
        /// <summary>
        /// !!!
        /// !!!
        /// TODO MAKE THE FUCKING COMMENTS
        /// TODO IT'S HARD TO READ THAT SHIT LATER
        /// !!!
        /// !!!
        /// </summary>
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
            LMenu.Children.Add(new PanelGlowny());
            RMenu.Children.Add(new PanelFiltrow());

            //TODO And that s*it if for what?
            //TODO IF FOR TESTING THEN CLEAN YOUR S*HIT
            using (var unitWork = new UnitOfWork(new PlanPwrContext()))
            {
                var bloki = unitWork.Bloki.GetAll();
                foreach(var blok in bloki)
                {
                    System.Diagnostics.Debug.WriteLine(blok.KodBloku);
                }
                //unitWork.SaveChanges();
            }

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

        }

        public void menuZapiszJako(object sender, EventArgs e)
        {

        }

        public void menuDrukuj(object sender, EventArgs e)
        {

        }

        public void menuEksportujPDF(object sender, EventArgs e)
        {

        }

        public void menuEksportujPlikGraficzny(object sender, EventArgs e)
        {

        }

        public void menuZakoncz(object sender, EventArgs e)
        {

        }

        public void menuInformacje(object sender, EventArgs e)
        {

        }
    }
}
