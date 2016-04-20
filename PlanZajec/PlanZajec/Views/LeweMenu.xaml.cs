using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf;

namespace PlanZajec.Views
{
    /// <summary>
    /// Interaction logic for LeweMenu.xaml
    /// </summary>
    public partial class LeweMenu : UserControl
    {

        private List<TabItem> _tabItems;
        //tab that make adding possible
        private TabItem _tabAdd;

        public LeweMenu()
        {
            InitializeComponent();
            _tabItems = new List<TabItem>();
            //add tab
            _tabAdd = new TabItem();
            _tabAdd.Header = "+";
            _tabAdd.Name = "AddSchedule";
            _tabItems.Add(_tabAdd);
            //
            TabItem tab = new TabItem();
            tab.Header = "Przeglądanie grup";
            //tab.HeaderTemplate = LewyTabControl.FindResource("TabHeader") as DataTemplate;
            PrzegladanieGrup pg = new PrzegladanieGrup();
            tab.Content = pg;
            //-end add item
            //add new plan
            _tabItems.Insert(0, tab);
            AddScheduleTabItem();
            LewyTabControl.DataContext = _tabItems;
            LewyTabControl.SelectedIndex = 0;
        }
        private TabItem AddScheduleTabItem()
        {
            //TODO foreach tab last index +1 == name
            int count = _tabItems.Count;

            // create new tab item
            TabItem tab = new TabItem
            {
                Header = $"Plan {count - 1}",
                Name = $"Plan{count - 1}",
                HeaderTemplate = LewyTabControl.FindResource("TabHeader") as DataTemplate
            };

            //Plan pl = new Plan();
            //tab.Content = pl;
            // add controls to tab item, this case I added just a textbox

            // insert tab item right before the last (+) tab item
            _tabItems.Insert(count - 1, tab);
            return tab;
        }



        private void BtnCloseCard_OnClick(object sender, RoutedEventArgs e)
        {
            string tabName = (sender as Button).CommandParameter.ToString();

            var tab = LewyTabControl.Items.Cast<TabItem>().SingleOrDefault(i => i.Name.Equals(tabName));

            if (tab != null)
            {
                if (_tabItems.Count < 3)
                {
                    MessageBox.Show("Cannot remove last tab.");
                }
                else if (MessageBox.Show(string.Format("Are you sure you want to remove the tab '{0}'?", tab.Header.ToString()),
                    "Remove Tab", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    // get selected tab
                    TabItem selectedTab = LewyTabControl.SelectedItem as TabItem;

                    // clear tab control binding
                    LewyTabControl.DataContext = null;

                    _tabItems.Remove(tab);

                    // bind tab control
                    LewyTabControl.DataContext = _tabItems;

                    // select previously selected tab. if that is removed then select first tab
                    if (selectedTab == null || selectedTab.Equals(tab))
                    {
                        selectedTab = _tabItems[0];
                    }
                    LewyTabControl.SelectedItem = selectedTab;
                }
            }
        }

        private void LewyTabControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItem tab = LewyTabControl.SelectedItem as TabItem;

            //exit if tab or header is null
            if (tab?.Header == null) return;
            //if it is add tab
            if (tab.Header.Equals(_tabAdd.Header))
            {
                // clear tab control binding
                LewyTabControl.DataContext = null;

                // add new tab
                TabItem newTab = this.AddScheduleTabItem();

                // bind tab control
                LewyTabControl.DataContext = _tabItems;

                // select newly added tab item
                LewyTabControl.SelectedItem = newTab;
            }
            else
            {
                
            }
        }
    }
}
