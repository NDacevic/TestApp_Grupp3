using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
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
            //string password = LogInViewModel.Instance.EncryptedPassword(PB_InsertPassword.ToString(), new SHA256CryptoServiceProvider());
            string password = PB_InsertPassword.ToString();

            if (Rb_Student.IsChecked == true)
            {
                LogInViewModel.Instance.GetStudent(Tb_InsertEmail.Text);
                LogInViewModel.Instance.CheckStudentPassword(password);
                Frame.Navigate(typeof(Student.MainPageStudentView));
                
            }
            else if (Rb_Employee.IsChecked == true)
            {
                LogInViewModel.Instance.GetEmployee(Tb_InsertEmail.Text);
                LogInViewModel.Instance.CheckEmployeePassword(password);

                if (LogInViewModel.Instance.ActiveEmployee.Role.RoleName == "Admin")
                {
                    Frame.Navigate(typeof(MainPageAdminViewxaml));
                }
                else if (LogInViewModel.Instance.ActiveEmployee.Role.RoleName == "Teacher")
                {
                    Frame.Navigate(typeof(Teacher.MainPageTeacherView));
                }
            }
            else
            {
                new MessageDialog("Vänligen klicka i om du är student eller personal, tack!");
            }
              
        }
    }
}
