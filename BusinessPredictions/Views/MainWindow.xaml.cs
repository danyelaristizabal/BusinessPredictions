using System.Windows;
using System.Windows.Controls;

namespace BusinessPredictions
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainWindowInit();
        }

        internal void MainWindowInit()
        {
            if (PanelsWrapper.GetUserControlWrapper().TryGetPanelByType(typeof(PlayPanel), out UserControl playPanel))
                GridMain.Children.Add(playPanel);
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }
        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridMain.Children.Clear();

            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "AdminPanel":
                    PanelsWrapper.GetUserControlWrapper().TryGetPanelByType(typeof(AdminPanel), out UserControl adminPanel);
                    GridMain.Children.Add(adminPanel);
                    break;
                case "PlayPanel":
                    PanelsWrapper.GetUserControlWrapper().TryGetPanelByType(typeof(PlayPanel), out UserControl playPanel);
                    GridMain.Children.Add(playPanel);
                    break;
                default:
                    break;
            }
        }
    }
}
