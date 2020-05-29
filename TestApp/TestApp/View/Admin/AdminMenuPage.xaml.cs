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

namespace TestApp.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminMenuPage : Page
    {
        private Frame mainFrame;
        public AdminMenuPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            mainFrame = (Frame)e?.Parameter;
            displayName.Text = $"Inloggad som:\n{ViewModel.LogInViewModel.Instance.ActiveEmployee.FirstName} {ViewModel.LogInViewModel.Instance.ActiveEmployee.LastName}" +
    $"\n{ViewModel.LogInViewModel.Instance.ActiveEmployee.Email}";
        }


        private void AdminNavigate_btn(object sender, RoutedEventArgs e)
        {
            if (sender == addEmployee_btn)
            {
                mainFrame.Navigate(typeof(Admin.AdminEmployeeView));
            }
            else if(sender == addStudent_btn)
            {
                mainFrame.Navigate(typeof(Admin.AdminStudentView));
            }
            else if (sender == editUser_btn)
            {
                mainFrame.Navigate(typeof(Admin.RemoveUserView));
            }
            else if (sender == removeTest_btn)
            {
                mainFrame.Navigate(typeof(DeleteTestView));
            }
            else if (sender == removeQuestion_btn)
            {
                mainFrame.Navigate(typeof(Teacher.RemoveQuestionView));
            }
            else if (sender == logOut_btn)
            {
                NavigationHelper.Instance.GlobalFrame.Navigate(typeof(LogInView));
            }
        }
    }
}
