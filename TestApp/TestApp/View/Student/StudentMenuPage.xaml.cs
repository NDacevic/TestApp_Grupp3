using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            mainFrame = (Frame)e?.Parameter;
        }


        private void StudentNavigate_btn(object sender, RoutedEventArgs e)
        {
            if (sender == historicalTest_btn)
            {
                mainFrame.Navigate(typeof(HistoricalTestsView));
            }
            else if(sender == availableTest_btn)
            {
                mainFrame.Navigate(typeof(AvailableTestsView));
            }
            else if(sender == logOut_btn)
            {
                NavigationHelper.Instance.GlobalFrame.Navigate(typeof(LogInView));                
            }
              
        }
    }
}
