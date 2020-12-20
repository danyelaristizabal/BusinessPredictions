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
                        break;

                    case Key.F5:

                        break;

                    default:
                        break;
                }
        }
        private void TextBox_OnKeyDown(object sender, KeyEventArgs e) => CallActionClickHitted((TextBox)sender, e);
        private void Clear_ButtonClick(object sender, RoutedEventArgs e) => AdminWrapperViewModel.ClearAllSelected();

        private async void DeleteSelectedElement_Click(object sender, RoutedEventArgs e)
        {
            if (_lastSelectedListBoxItem != null)
                AdminWrapperViewModel.DeleteSelected(_lastSelectedListBoxItem);
            else
                MessageBox.Show("Please choose an element to delete from the lists");
        }

        private object _lastSelectedListBoxItem;
        private object _lastSelectedTextBoxDataContext;

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox focusedlistBox)
                _lastSelectedListBoxItem = focusedlistBox.SelectedItem;
        }

        private void AddElement_Click(object sender, RoutedEventArgs e)
        {
            if (_lastSelectedTextBoxDataContext != null)
                AdminWrapperViewModel.AddSelected(_lastSelectedTextBoxDataContext);
            else
                MessageBox.Show("Please choose an element to add from the lists");
        }

        private void TextChangedInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox texBox)
                _lastSelectedTextBoxDataContext = texBox.DataContext;
        }
    }
}
