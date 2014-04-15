using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ShellService.MVVM
{
    public class UserControlBase : UserControl
    {
        #region Properties
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(UserControlBase), new UIPropertyMetadata(string.Empty));


        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(UserControlBase), new UIPropertyMetadata(false));


        public int Left
        {
            get { return (int)GetValue(IsLeftProperty); }
            set { SetValue(IsLeftProperty, value); }
        }

        public static readonly DependencyProperty IsLeftProperty =
            DependencyProperty.Register("IsLeft", typeof(int), typeof(UserControlBase), new UIPropertyMetadata(0));

        public int Top
        {
            get { return (int)GetValue(IsTopProperty); }
            set { SetValue(IsTopProperty, value); }
        }

        public static readonly DependencyProperty IsTopProperty =
            DependencyProperty.Register("IsTop", typeof(int), typeof(UserControlBase), new UIPropertyMetadata(0));

        #endregion

        public UserControlBase()
        {
            this.Loaded += new RoutedEventHandler(OnUserControlLoaded);
        }

        /// <summary>
        /// Subscribes event(s) that'll be used by all UserControls in the MDI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnUserControlLoaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
