using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using TestApp.Model;
using TestApp.View.Student;
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

namespace TestApp.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LogInView : Page
    {
        public LogInView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Method checking email and password för the user trying to log in 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bttn_Login_Click(object sender, RoutedEventArgs e)
        {
            CheckLogin();            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //Save the frame when we have access to it so we can navigate back to which ever page
            //we want at any time. (Currently it's only used for navigating back to the login page).
            NavigationHelper.Instance.GlobalFrame = Frame;
        }

        /// <summary>
        /// Gets a student by inserted email, checking if inserted password is correct. If CheckStudentPassword
        /// returns true we navigate to the mainpage for the student.
        /// </summary>
        /// <param name="password"></param>
        public async void StudentLogIn(string password)
        {
            await LogInViewModel.Instance.GetStudent(Tb_InsertEmail.Text);
            bool correctStudent = LogInViewModel.Instance.CheckStudentPassword(password);
            if (correctStudent)
            {
                //Populates the property used on the next page before navigating to it, so it is fully loaded when we navigate there.
                await StudentViewModel.Instance.SeeActiveTests();
                Frame.Navigate(typeof(MainPage), "Elev");
            }
            else
            {
                await new MessageDialog("Inkorrekt data, försök igen.").ShowAsync();
            }
        }
        /// <summary>
        /// Gets an employee by inserted email, checking if inserted password is correct. If CheckEmployeePassword
        /// returns true we check what role the employee is listed as. Depending on the role the program will navigate 
        /// to different pages.
        /// </summary>
        /// <param name="password"></param>
        public async void EmployeeLogIn(string password)
        {
            await LogInViewModel.Instance.GetEmployee(Tb_InsertEmail.Text);
            bool correctEmployee = LogInViewModel.Instance.CheckEmployeePassword(password);
            if (correctEmployee)
            {
                if (LogInViewModel.Instance.ActiveEmployee.Role.RoleId == 1)
                {
                    Frame.Navigate(typeof(MainPage), "Teacher");
                }
                else if (LogInViewModel.Instance.ActiveEmployee.Role.RoleId == 2)
                {
                    Frame.Navigate(typeof(MainPage), "Admin");
                }
            }
            else
            {
                await new MessageDialog("Inkorrekt data, försök igen.").ShowAsync();
            }
        }

        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            //You can hit enter if you're focused on either the username och password boxes
            //and the app will check your login information. No need to click the login button
            if(e.Key == Windows.System.VirtualKey.Enter)
            {
                CheckLogin();
            }
        }

        /// <summary>
        /// Checking if it's an employee or a student trying to log in
        /// </summary>
        private void CheckLogin()
        {
            string password = LogInViewModel.EncryptPassword(PB_InsertPassword.Password);

            if (Rb_Student.IsChecked == true)
            {
                StudentLogIn(password);

            }
            else if (Rb_Employee.IsChecked == true)
            {
                EmployeeLogIn(password);
            }
        }
    }
}
