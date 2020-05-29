using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TestApp.Model;
using TestApp.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
        /// Navigation to WriteTestView using double click on ListView item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Lv_AvailableTests_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            //Save the test attempted to be written
            Test selectedTest = ((sender as ListView).SelectedItem) as Test;
            TakeTestOrDont(selectedTest);            
        }

        /// <summary>
        /// Navigation to WriteTestView using button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bttn_TakeTest_Click(object sender, RoutedEventArgs e)
        {
            Test selectedTest = Lv_AvailableTests.SelectedItem as Test;
            TakeTestOrDont(selectedTest);
        }

        /// <summary>
        /// Checks if date and time conditions are met, if so navigates further and starts the test
        /// </summary>
        /// <param name="selectedTest"></param>
        private void TakeTestOrDont(Test selectedTest)
        {
            //Get the current Time
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            //Get the current Date
            DateTime currentDate = DateTime.Now.Date;

            //If the date for the test is equal to the current date && if the current time is after the tests start time...
            if (currentDate == selectedTest.StartDate.Date && currentTime >= selectedTest.StartDate.TimeOfDay)
            {
                Frame.Navigate(typeof(WriteTestView), selectedTest);
            }
            else
            {
                _ = new MessageDialog($"Det aktuella provet kan tidigast påbörjas {selectedTest.StartDate.DateTime:yyyy-mm-dd HH:mm}.").ShowAsync();
            }
        }
        #endregion


    }
}