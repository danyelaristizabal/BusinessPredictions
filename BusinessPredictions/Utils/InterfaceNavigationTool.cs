using System;
using System.Windows;

namespace BusinessPredictions
{
    public static class InterfaceNavigationTool
    {
        public static Window GetThisWindow(Type type) 
        {
            try
            {
               return  Application.Current.Dispatcher.Invoke(() =>
                {
                    Window window = new Window();

                    foreach (Window item in App.Current.Windows)
                    {
                        if (item.GetType() == type)
                        {
                            window = item;
                        }
                    }
                    return window;
                });
            }
            catch 
            {
                MessageBox.Show($"Error trying to get window type {type.Name}");
            }
            return null;
        } 
    }
}
