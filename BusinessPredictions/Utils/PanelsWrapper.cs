using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq.Expressions;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BusinessPredictions
{
    public class PanelsWrapper
    {
        private static readonly PanelsWrapper _panelsWrapper = new PanelsWrapper(); 
        private readonly Dictionary<Type, UserControl> _panels = new Dictionary<Type, UserControl>(); 
        private PanelsWrapper() 
        {
            _panels.Add(typeof(PlayPanel), new PlayPanel());
            _panels.Add(typeof(AdminPanel),new AdminPanel()); 
        }
        public static  PanelsWrapper GetUserControlWrapper() => _panelsWrapper;
        public bool TryGetPanelByType(Type panelType, out UserControl desiredPanel) =>  _panels.TryGetValue(panelType, out desiredPanel);
    }
}
