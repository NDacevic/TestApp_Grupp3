using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TestApp.Model;
using TestApp.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TestApp.View.Student
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AvailableTestsView : Page
    {
        #region Constant Fields
        #endregion

        #region Fields
        #endregion

        #region Constructors
        public AvailableTestsView()
        {
            this.InitializeComponent();
            //Call method to set the Property that binds to this page's ListView
            StudentViewModel.Instance.SeeActiveTests();
        }
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        #endregion

        #region Methods
        #endregion

        private void Lv_AvailableTests_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {            
            Frame.Navigate(typeof(WriteTestView), sender as Test);
        }

        private void Bttn_TakeTest_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(WriteTestView), Lv_AvailableTests.SelectedItem);
        }
    }
}
