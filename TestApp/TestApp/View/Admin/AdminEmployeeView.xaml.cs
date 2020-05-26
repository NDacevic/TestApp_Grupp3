using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TestApp.Model;
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

        private void Bttn_AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            string firstName = Tb_FirstName.Text;
            string lastName = Tb_LastName.Text;
            string email = Tb_Email.Text;
            string password = Tb_Password.Text;
            EmployeeRole employeeRole = (EmployeeRole)Cb_EmployeeRole.SelectedItem;

            AdminViewModel.Instance.SetValuesForEmployee(firstName, lastName, email, password, employeeRole);
        }
    }
}
