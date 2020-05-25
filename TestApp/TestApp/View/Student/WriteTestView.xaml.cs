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
    public sealed partial class WriteTestView : Page
    {
        #region Constant Fields
        #endregion

        #region Fields
        private Test selectedTest;  //used for binding
        #endregion

        #region Constructors
        public WriteTestView()
        {
            this.InitializeComponent();
        }
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        #endregion

        #region Methods
        /// <summary>
        /// Executes when this page is navigated to
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Saves the selected test from last page
            selectedTest = (Test)e.Parameter;                                
        }

        #endregion

        private void Bttn_SubmitTest_Click(object sender, RoutedEventArgs e)
        {
            StudentViewModel.Instance.StopAndSubmitTest();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TxtBl_TestTimer.Text = $"Tid kvar: {selectedTest.TestDuration} min";
            //Starts the timer instantly after the page has been fully loaded
            StudentViewModel.Instance.DispatcherTimerSetup(selectedTest, TxtBl_TestTimer, Lv_AllQuestions, Bttn_SubmitTest);
        }
    }
   
}
