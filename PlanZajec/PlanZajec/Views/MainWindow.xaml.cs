using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PlanZajec.Parser;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            Parser.Run();
            prePrareDB();
            InitializeComponent();
            LMenu.Children.Add(new PrzegladanieGrup());
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

        private void resizeElements()
        {

        }

        private void prePrareDB()
        {
            string dbName = "Super-egatron-5000X-DB.sqlite";
            string dbPathOutsideBinDebug = "../../" + dbName;
            bool fileExist = File.Exists(dbName);
            if (!fileExist && File.Exists(dbPathOutsideBinDebug))
            {
                System.Diagnostics.Debug.WriteLine("Kopiuje DB do bin/debug");
                File.Copy(dbPathOutsideBinDebug,dbName);
            }
            //string fullP = Path.GetFullPath("Super-egatron-5000X-DB.sqlite");
            //System.Diagnostics.Debug.WriteLine(fullP);
        }

    }
}
