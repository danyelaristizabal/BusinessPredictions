using System.Windows.Controls;

namespace BusinessPredictions
{
    /// <summary>
    /// Interaction logic for PlayPanel.xaml
    /// </summary>
    public partial class PlayPanel : UserControl
    {
        internal PlayPanelViewModel GameWrapperVM { get; set; } = new PlayPanelViewModel();
        public PlayPanel()
        {
            InitializeComponent();
            BindControls();
            LoadData();
        }
        private void BindControls()
        {
            MainGridWrapper.DataContext = GameWrapperVM;
        }
        private void LoadData() => GameWrapperVM.LoadData();

        private void RightSubject_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Grid rightGrid && rightGrid.DataContext is Subject selectedRightSubject)
            {
                GameWrapperVM.SelectRandomlyOrDefault(selectedRightSubject);
            }
        }
        private void LeftSubject_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Grid leftGrid && leftGrid.DataContext is Subject selectedLeftSubject)
            {
                GameWrapperVM.SelectRandomlyOrDefault(selectedLeftSubject, true);
            }
        }



    }
}
