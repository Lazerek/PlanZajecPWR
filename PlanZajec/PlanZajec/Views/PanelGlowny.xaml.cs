using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PlanZajec.CommonInformations;
using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using PlanZajec.ViewModels;
using Wpf;

namespace PlanZajec.Views
{
    /// <summary>
    ///     Interaction logic for LeweMenu.xaml
    /// </summary>
    public partial class PanelGlowny : UserControl
    {
        private static long tabsId;
        //
        private readonly HashSet<long> _openedScheuldes;
        //tab that make adding possible

        private readonly TabItem _tabAdd;

        private PlanView _aktualnyPlanView;

        private readonly ObservableCollection<TabItem> _tabItems;

        private readonly MainWindow _mainWindow;

        //kod aktualnie otwartego planu
        private long? OtwartyPlanId
        {
            get { return _otwartyPlanId; }
            set
            {
                _mainWindow?.UpdateOnSelectedPlanChange(value);
            }
        }
        private long? _otwartyPlanId;

        public PanelGlowny(MainWindow mainWindow)
        {
            _openedScheuldes = new HashSet<long>();
            InitializeComponent();
            _mainWindow = mainWindow;
            tabsId = 0;
            _tabItems = new ObservableCollection<TabItem>();
            //add tab
            _tabAdd = GetPlusCard();
            _tabItems.Add(_tabAdd);
            //
            var tab = new TabItem();
            tab.Header = "Przeglądanie grup";
            //tab.HeaderTemplate = LewyTabControl.FindResource("TabHeader") as DataTemplate;
            var pg = new PrzegladanieGrup();
            tab.Content = pg;
            //-end add item
            //add new plan
            _tabItems.Insert(0, tab);
            AddScheduleTabItem();
            LewyTabControl.DataContext = _tabItems;
            LewyTabControl.SelectedIndex = 0;
        }


        private TabItem GetPlusCard()
        {
            TabItem tab = new TabItem();
            tab.Header = "+";
            tab.Name = "AddSchedule";
            return tab;
        }


        private TabItem AddScheduleTabItem()
        {
            //TODO foreach tab last index +1 == name

            //var last = _tabItems.Max(t=>t.Name.Replace())
            var count = _tabItems.Count;

            // create new tab item
            var tab = new TabItem
            {
                Header = "Wybierz plan",
                Name = $"WybierzPlan{tabsId++}",
                HeaderTemplate = LewyTabControl.FindResource("TabHeader") as DataTemplate,
                Content = PrepareChosingPlan()
            };


            // insert tab item right before the last (+) tab item
            _tabItems.Insert(count - 1, tab);
            return tab;
        }

        private WyborPlanu PrepareChosingPlan()
        {
            var res = new WyborPlanu {DataContext = WyborPlanuViewModel.Instance};
            res.ChosenPlanToShowEventHandler += WybierzPlanDoWyswietlania;
            res.ChosenPlanToDeleteEventHandler += UsunPlan;
            res.AddToPlan += AddPlan;

            return res;
        }

        private void AddPlan(string Title)
        {
            Plany plan;
            using (var unit = new UnitOfWork(new PlanPwrContext()))
            {
                plan = new Plany {NazwaPlanu = Title};
                unit.Plany.Add(plan);
                unit.SaveChanges();
            }
            WyborPlanuViewModel.Instance.DodajPlan(plan);
            UsunPlanViewModel.Instance.DodajPlan(plan);
        }


        private void WybierzPlanDoWyswietlania(Plany plan)
        {
            //Podmiana aktualnego taba na wyswietlanie planu
            //TODO Greg - obsluz zmianę widoku
            _openedScheuldes.Add(plan.IdPlanu);
            ActChosenPlanSingleton.Instance.SetPlan(plan.IdPlanu);
            for (var i = 0; i < _tabItems.Count; i++)
            {
                var view = _tabItems[i].Content as PlanView;
                var viewModel = view?.DataContext as PlanViewModel;
                if (viewModel != null && viewModel.IdPlanu == plan.IdPlanu)
                {
                    LewyTabControl.SelectedItem = _tabItems[i];
                    return;
                }
            }
            var planView = ObslugaWidokuWieluPlanów.Instance.getPlanView(plan.IdPlanu);

            var tabItem = new TabItem
            {
                Content = planView,
                Header = $"Plan {plan.NazwaPlanu}",
                Name = $"Plan{plan.IdPlanu}",
                HeaderTemplate = LewyTabControl.FindResource("TabHeader") as DataTemplate
            };
            _tabItems[LewyTabControl.SelectedIndex] = tabItem;

            LewyTabControl.SelectedItem = tabItem;
        }

        private bool UsunPlan(Plany plan)
        {
            //if scheulde is open then u cant delete
            if (_openedScheuldes.Contains(plan.IdPlanu))
            {
                MessageBox.Show("Otwarty plan, zamknij aby usunąć");
                return false;
            }
            //if u can delete then...
            using (var unit = new UnitOfWork(new PlanPwrContext()))
            {
                unit.Plany.Remove(plan);
                unit.SaveChanges();
            }
            WyborPlanuViewModel.Instance.UsunPlan(plan);
            UsunPlanViewModel.Instance.UsunPlan(plan);
            return true;
        }

        private void BtnCloseCard_OnClick(object sender, RoutedEventArgs e)
        {
            var tabName = ((Button) sender).CommandParameter.ToString();

            var tab = LewyTabControl.Items.Cast<TabItem>().SingleOrDefault(i => i.Name.Equals(tabName));

            if (tab != null)
            {
                if (_tabItems.Count < 3)
                {
                    MessageBox.Show("Cannot remove last tab.");
                }

                /*Old format for repair causes
                 * else if (MessageBox.Show(string.Format("" +
                 *                                      "Are you sure" +
                 *                                      " you want to remove the tab '{0}'?", tab.Header),
                 *   "Remove Tab", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                */
                else 
                {
                    if (!tabName.Contains("WybierzPlan"))
                    {
                        long idPlanu = long.Parse(tab.Name.Substring(4, tab.Name.Length - 4));
                        _openedScheuldes.Remove(idPlanu);
                    }
                    // get selected tab
                    var selectedTab = LewyTabControl.SelectedItem as TabItem;

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
                    UpdateActualChoseningPlan(selectedTab);
                    //load planViewModel datacontext from tab content if not null
                    if (selectedTab.Content is PlanView)
                    {
                        PlanViewModel pvm = (selectedTab.Content as PlanView).DataContext as PlanViewModel;
                        if (pvm != null)
                        {
                            ObslugaWidokuWieluPlanów.Instance.deletePlanView(pvm.IdPlanu);
                        }
                    }
                    /*var vm = planView?.DataContext as PlanViewModel;
                    if (vm != null)
                    {
                        ObslugaWidokuWieluPlanów.Instance.deletePlanView(vm.IdPlanu);
                    }*/
            }
        }
        }

        private void LewyTabControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tab = LewyTabControl.SelectedItem as TabItem;


            //exit if tab or header is null
            if (tab?.Header == null) return;
            //if it is add tab
            if (tab.Header.Equals(_tabAdd.Header))
            {
                // clear tab control binding
                LewyTabControl.DataContext = null;

                // add new tab
                var newTab = AddScheduleTabItem();

                // bind tab control
                LewyTabControl.DataContext = _tabItems;

                // select newly added tab item
                LewyTabControl.SelectedItem = newTab;

                //
            }

            UpdateActualChoseningPlan(LewyTabControl.SelectedItem as TabItem);

            PlanView otwartyPlan = LewyTabControl.SelectedContent as PlanView;
            if (otwartyPlan!=null)
            {
                OtwartyPlanId = otwartyPlan.GetPlanId();
            }

        }

        private void UpdateActualChoseningPlan(TabItem tab)
        {
            var result = false;
            if (tab.Content != null && tab.Content is PlanView)
            {
                var view = tab.Content as PlanView;
                if (view.DataContext is PlanViewModel)
                {
                    var vm = view.DataContext as PlanViewModel;
                    ActChosenPlanSingleton.Instance.SetPlan(vm.IdPlanu);
                    result = true;
                }
            }
            if (!result)
            {
                ActChosenPlanSingleton.Instance.SetPlan(-1);
            }
        }
    }
}