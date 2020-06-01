using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
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
    public sealed partial class RemoveUserView : Page
    {
        AdminViewModel adminViewModel = AdminViewModel.Instance;
        Person chosenPerson;
        public RemoveUserView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// When clicked, navigates to method that displays Employees in school
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EmployeeRadioBtn_Click(object sender, RoutedEventArgs e)
        {
            adminViewModel.AllUsers.Clear();
            adminViewModel.DisplayEmployees();
        }

        /// <summary>
        /// When clicked, navigates to method that displays Students in school
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StudentRadioBtn_Click(object sender, RoutedEventArgs e)
        {
            adminViewModel.AllUsers.Clear();
            adminViewModel.DisplayStudents();
        }

        /// <summary>
        /// Search and display user based on ID, instead of a button the user can press 'Enter'.
        /// We also check if the user is a student or employee with radiobuttons.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchIdTxtBox_KeyDown(object sender, KeyRoutedEventArgs e) 
        {
            try
            {
                if (e.Key == Windows.System.VirtualKey.Enter)
                {
                    if (StudentRadioBtn.IsChecked == true)
                    {
                        if (SearchIdTxtBox.Text == "")
                        {
                            adminViewModel.DisplayStudents();
                        }
                        else
                            adminViewModel.DisplayStudentById(int.Parse(SearchIdTxtBox.Text));
                    }
                    else if (EmployeeRadioBtn.IsChecked == true)
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
            catch(FormatException)
            {
                
            }
           
        }

        /// <summary>
        /// Gives the user a warning before deletion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DeleteUser_btn_Click(object sender, RoutedEventArgs e)
        {
           
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
                DeleteUser();
                ResetControllers();
            }
        }

        /// <summary>
        /// Checks if the picked user is a student or employee and sends further for deletion.
        /// </summary>
        public void DeleteUser()
        {
            var selectedUser = DisplayUsersLV.SelectedItems;
            if (StudentRadioBtn.IsChecked == true)
            {
                foreach (Model.Student student in selectedUser)
                {
                    adminViewModel.DeleteStudent(student);
                }
            }
            else if (EmployeeRadioBtn.IsChecked == true)
            {
                foreach (Employee employee in selectedUser)
                {
                    adminViewModel.DeleteEmployee(employee);
                }
            }
        }

        /// <summary>
        /// Todo: Comments!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_ChoosePersonClick(object sender, ItemClickEventArgs e)
        {
            chosenPerson = (Person)e.ClickedItem;

            textBox_FirstName.Text = ((Person)e.ClickedItem).FirstName;
            textBox_LastName.Text = ((Person)e.ClickedItem).LastName;
            textBox_Email.Text = ((Person)e.ClickedItem).Email;
        }

        /// <summary>
        /// Todo: Comments!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void EditInformationClick(object sender, RoutedEventArgs args)
        {
            try
            {
                if (textBox_FirstName.Text == "" ||
                        textBox_LastName.Text == "" ||
                        textBox_Email.Text == "")
                    throw new FormatException();

                if (chosenPerson.GetType() == typeof(Model.Student))
                    adminViewModel.EditUserInfo
                        (
                        (Model.Student)chosenPerson,
                        textBox_FirstName.Text,
                        textBox_LastName.Text,
                        textBox_Email.Text
                        );
                else if (chosenPerson.GetType() == typeof(Model.Employee))
                    adminViewModel.EditUserInfo
                        (
                        (Model.Employee)chosenPerson,
                        textBox_FirstName.Text,
                        textBox_LastName.Text,
                        textBox_Email.Text
                        );
            }
            catch (FormatException)
            {
                await new MessageDialog("Förnamn, Efternamn och Email får inte vara tomma").ShowAsync();
            }
            catch
            {
                await new MessageDialog("Välj en person att redigera först").ShowAsync();
            }
        }

        /// <summary>
        /// Todo: Comments!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void EditPasswordClick(object sender, RoutedEventArgs args)
        {
            try
            {
                if (passwordBox_Password.Password == "" && passwordBox_repeatPassword.Password == "")
                    throw new FormatException();

                if (passwordBox_Password.Password == passwordBox_repeatPassword.Password)
                {
                    string hashPass = LogInViewModel.EncryptPassword(passwordBox_Password.Password);

                    if (chosenPerson.GetType() == typeof(Model.Student))
                        adminViewModel.EditPassword((Model.Student)chosenPerson, hashPass);
                    else if (chosenPerson.GetType() == typeof(Model.Employee))
                        adminViewModel.EditPassword((Model.Employee)chosenPerson, hashPass);
                }
                else
                    await new MessageDialog("Lösenorden stämmer ej").ShowAsync();

            }
            catch (NullReferenceException)
            {
                await new MessageDialog("Välj en person först").ShowAsync();
            }
            catch(FormatException)
            {
                await new MessageDialog("Lösenordet får inte vara tomt").ShowAsync();
            }
            catch
            {
                await new MessageDialog("Error. Kontakta administratör").ShowAsync();
            }
        }

        /// <summary>
        /// Todo: Comments!
        /// </summary>

        private void ResetControllers()
        {
            textBox_FirstName.Text = "";
            textBox_LastName.Text = "";
            textBox_Email.Text = "";

            passwordBox_Password.Password = "";
            passwordBox_repeatPassword.Password = "";
        }
    }
}