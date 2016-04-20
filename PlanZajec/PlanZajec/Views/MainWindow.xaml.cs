using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using System.ComponentModel;
using System.Windows;
using PlanZajec.Parser;
using PlanZajec.Views;
using System;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            DataBaseReturnPoint.PrePrareDB();
            InitializeComponent();
            LMenu.Children.Add(new LeweMenu());
            RMenu.Children.Add(new PraweMenu());
           

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
    }
}
