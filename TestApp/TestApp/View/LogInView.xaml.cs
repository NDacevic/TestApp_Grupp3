using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
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
        private async void Bttn_Login_Click(object sender, RoutedEventArgs e)
        {

            //string password = LogInViewModel.Instance.EncryptedPassword(PB_InsertPassword.ToString(), new SHA256CryptoServiceProvider());
            string password = PB_InsertPassword.Password;

            ////!!Following code is only for testing purpose.
            //if(Tb_InsertEmail.Text=="1")
            //{
            //    LogInViewModel.Instance.ActiveStudent = new Model.Student(2,"Mikael","Ollhage","nej@ja.com","pass",8,new List<Test>()); 
            //    this.Frame.Navigate(typeof(MainPage),"Elev");
                
            //}
            //else if(Tb_InsertEmail.Text == "2")
            //{
            //    this.Frame.Navigate(typeof(MainPage), "Teacher");
            //}
            //else if (Tb_InsertEmail.Text == "3")
            //{
            //    this.Frame.Navigate(typeof(MainPage), "Admin");
            //}
            ////TestCode stopped






            if (Rb_Student.IsChecked == true)
            {
                LogInViewModel.Instance.GetStudent(Tb_InsertEmail.Text);
                LogInViewModel.Instance.CheckStudentPassword(password);
                Frame.Navigate(typeof(MainPage), "Elev");

            }
            else if (Rb_Employee.IsChecked == true)
            {
                await LogInViewModel.Instance.GetEmployee(Tb_InsertEmail.Text);
                LogInViewModel.Instance.CheckEmployeePassword(password);
                //LogInViewModel.Instance.GetEmployeeRole();


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
                new MessageDialog("Vänligen klicka i om du är student eller personal, tack!");
            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationHelper.Instance.GlobalFrame = Frame;
        }
    }
}
