using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BusinessPredictions
{
    /// <summary>
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : UserControl
    {
        public AdminWrapperViewModel AdminWrapperViewModel { get; set; } = new AdminWrapperViewModel();
        public AdminPanel()
        {
            InitializeComponent();
            BindControls();
        }
        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }
        private void BindControls()
        {
            AdminGridWrapper.DataContext = AdminWrapperViewModel;
        }
        public void CallActionClickHitted(TextBox txtBox, KeyEventArgs e)
        {

            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.LeftShift))
                switch (e.Key)
                {
                    case Key.Enter:
                        AdminWrapperViewModel.AddSelected(txtBox);
                        break;
                    case Key.Escape:
                        AdminWrapperViewModel.ClearAllSelected();
                        break;

                    case Key.Back:
                        AdminWrapperViewModel.DeleteSelected(txtBox);
                        break;

                    case Key.F5:

                        break;

                    default:
                        break;
                }
        }
        private void TextBox_OnKeyDown(object sender, KeyEventArgs e) => CallActionClickHitted((TextBox)sender, e);
        private void Clear_ButtonClick(object sender, RoutedEventArgs e) => AdminWrapperViewModel.ClearAllSelected();
    }
}
