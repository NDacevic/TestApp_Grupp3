using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TestApp.View;
using TestApp.View.Student;
using TestApp.View.Teacher;
using Windows.ApplicationModel.Activation;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Button_NavigateToPage(object sender, RoutedEventArgs e)
        {
            if (sender == bttn_HistoricalTestsViewNavigate)
                mainFrame.Navigate(typeof(HistoricalTestsView));
            else if (sender == bttn_MainPageStudentViewNavigate)
                mainFrame.Navigate(typeof(MainPageStudentView));
            else if (sender == bttn_WriteTestViewNavigate)
                mainFrame.Navigate(typeof(WriteTestView));
            else if (sender == bttn_CreateQuestionViewNavigate)
                mainFrame.Navigate(typeof(CreateQuestionView));
            else if (sender == bttn_CreateTestViewNavigate)
                mainFrame.Navigate(typeof(CreateTestView));
            else if (sender == bttn_GradeTestViewNavigate)
                mainFrame.Navigate(typeof(GradeTestView));
            else if (sender == bttn_MainPageTeacherViewNavigate)
                mainFrame.Navigate(typeof(MainPageTeacherView));
            else if (sender == bttn_StudentResultViewNavigate)
                mainFrame.Navigate(typeof(StudentResultView));
            else if (sender == bttn_LogInViewNavigate)
                mainFrame.Navigate(typeof(LogInView));
        }
    }
}
