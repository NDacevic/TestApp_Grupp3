using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace TestApp.View.Admin
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminEmployeeView : Page
    {
        public AdminEmployeeView()
        {
            this.InitializeComponent();
            AdminViewModel.Instance.GetRoles(); 
        }
        /// <summary>
        /// Encrypting password before sending values to be set to the new employee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Bttn_AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string password = LogInViewModel.EncryptPassword(Pb_Password.Password);
                AdminViewModel.Instance.SetValuesForEmployee(Tb_FirstName.Text, Tb_LastName.Text, Tb_Email.Text, password, Cb_EmployeeRole.SelectedItem.ToString());
            }
            catch (Exception)
            {
                await new MessageDialog("Data var felaktigt inmatad, vänligen försök igen.").ShowAsync();

            }
            ClearValues();
        }
        /// <summary>
        /// Clear values when new student has been posted to DB
        /// </summary>
        public void ClearValues ()
        {
            Tb_FirstName.Text = "";
            Tb_LastName.Text = "";
            Tb_Email.Text = "";
            Pb_Password.Password = "";
            Cb_EmployeeRole.SelectedIndex = 0;
        }
    }
}

