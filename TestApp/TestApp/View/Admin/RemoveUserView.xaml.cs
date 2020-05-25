using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public sealed partial class RemoveUserView : Page
    {
        AdminViewModel adminViewModel = AdminViewModel.Instance;
        public RemoveUserView()
        {
            this.InitializeComponent();
        }

        private void EmployeeRadioBtn_Click(object sender, RoutedEventArgs e)
        {
            adminViewModel.AllUsers.Clear();
            adminViewModel.DisplayEmployees();
        }

        private void StudentRadioBtn_Click(object sender, RoutedEventArgs e)
        {
            adminViewModel.AllUsers.Clear();
            adminViewModel.DisplayStudents();
        }

        private void SearchIdTxtBox_KeyDown(object sender, KeyRoutedEventArgs e) //Search person based on ID
        {
            if (e.Key==Windows.System.VirtualKey.Enter) //Instead of using a button for the textbox we can use 'Enter-key'
            {
                if (StudentRadioBtn.IsChecked == true) //Check if it´s an employee or a student and displays on screen
                {
                    if(SearchIdTxtBox.Text=="")
                    {
                        adminViewModel.DisplayStudents();
                    }
                    else
                    adminViewModel.DisplayStudentById(int.Parse(SearchIdTxtBox.Text));
                }
                else if(EmployeeRadioBtn.IsChecked==true)
                {
                    if (SearchIdTxtBox.Text == "")
                    {
                        adminViewModel.DisplayEmployees();
                    }
                    else
                        adminViewModel.DisplayEmployeeById(int.Parse(SearchIdTxtBox.Text));
                }
            }
           
        }

        private async void DeleteUser_btn_Click(object sender, RoutedEventArgs e)
        {
            string user;
            var selectedUser = DisplayUsersLV.SelectedItems; //The selected user is saved here
            ContentDialog confirmButton = new ContentDialog() //Make sure that the user is aware of the action
            {
                Title = "Ta bort",
                Content = "Detta kommer att radera personen från databasen, är du säker?",
                PrimaryButtonText = "Ja",
                CloseButtonText = "Nej"
            };
            ContentDialogResult result = await confirmButton.ShowAsync();

            if (result == ContentDialogResult.Primary) //If they are ok we send the users id forward for deletion
            {
                if(StudentRadioBtn.IsChecked==true)
                {
                    user = "Elev";
                    foreach(Model.Student student in selectedUser)
                    {
                        adminViewModel.DeleteUser(student.StudentId, user);
                    }
                }
                else if(EmployeeRadioBtn.IsChecked==true)
                {
                    user = "Anställd";
                    foreach (Employee employee in selectedUser)
                    {
                        adminViewModel.DeleteUser(employee.EmployeeId, user);
                    }
                }
            }
        }

        private void listView_ChoosePersonClick(object sender, ItemClickEventArgs e)
        {
            if(e.ClickedItem.GetType() == typeof(Model.Student))
                textBlock_IdTextBlock.Text = $"ID: {((Model.Student)e.ClickedItem).StudentId}";
            else if (e.ClickedItem.GetType() == typeof(Model.Employee))
                textBlock_IdTextBlock.Text = $"ID: {((Model.Employee)e.ClickedItem).EmployeeId}";

            textBox_FirstName.Text = ((Person)e.ClickedItem).FirstName;
            textBox_LastName.Text = ((Person)e.ClickedItem).LastName;
            textBox_Email.Text = ((Person)e.ClickedItem).Email;

        }
    }
}
