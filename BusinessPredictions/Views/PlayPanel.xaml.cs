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
    }
}
