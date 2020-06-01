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

namespace TestApp.View.Teacher
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TeacherMenuPage : Page
    {
        private Frame mainFrame;
        public TeacherMenuPage()
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
            if(ViewModel.LogInViewModel.Instance.ActiveEmployee != null)
                displayName.Text = $"Inloggad som:\n{ViewModel.LogInViewModel.Instance.ActiveEmployee.FirstName} {ViewModel.LogInViewModel.Instance.ActiveEmployee.LastName}" +
                    $"\n{ViewModel.LogInViewModel.Instance.ActiveEmployee.Email}";           
        }

        /// <summary>
        /// Navigates to different pages depending on the users choice.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TeacherNavigate_btn(object sender, RoutedEventArgs e)
        {
            if (sender == createQuestion_btn)
            {
                mainFrame.Navigate(typeof(CreateQuestionView));
            }
            else if (sender == createTest_btn)
            {
                mainFrame.Navigate(typeof(CreateTestView));
            }
            else if(sender == gradeTest_btn)
            {
                mainFrame.Navigate(typeof(GradeTestView));
            }
            else if (sender == studentResult_btn)
            {
                mainFrame.Navigate(typeof(StudentResultView));
            }
           
            else if (sender == logOut_btn)
            {
                NavigationHelper.Instance.GlobalFrame.Navigate(typeof(LogInView));
            }

        }
    }
}
