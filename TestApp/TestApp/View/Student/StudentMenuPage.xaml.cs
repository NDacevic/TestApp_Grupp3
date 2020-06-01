using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class StudentMenuPage : Page
    {
        private Frame mainFrame;
        public StudentMenuPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Displays the name and email of user that logged in
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            mainFrame = (Frame)e?.Parameter;

            if(LogInViewModel.Instance.ActiveStudent != null)
                displayName.Text = $"Inloggad som:\n{LogInViewModel.Instance.ActiveStudent.FirstName} {LogInViewModel.Instance.ActiveStudent.LastName}" +
                    $"\n{LogInViewModel.Instance.ActiveStudent.Email}";
        }

        /// <summary>
        /// Navigaste to the choosen page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void StudentNavigate_btn(object sender, RoutedEventArgs e)
        {
            if (sender == historicalTest_btn)
            {
                mainFrame.Navigate(typeof(HistoricalTestsView));
            }
            else if(sender == availableTest_btn)
            {
                await StudentViewModel.Instance.SeeActiveTests();
                mainFrame.Navigate(typeof(AvailableTestsView));
            }
            else if(sender == logOut_btn)
            {
                NavigationHelper.Instance.GlobalFrame.Navigate(typeof(LogInView));                
            }              
        }
    }
}
