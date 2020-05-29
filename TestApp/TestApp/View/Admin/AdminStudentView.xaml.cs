using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class AdminStudentView : Page
    {
        public AdminStudentView()
        {
            this.InitializeComponent();

            TeacherCreateViewModel.Instance.GetCoursesForList();

            foreach (var x in TeacherCreateViewModel.Instance.Grades) //Make sure that our dropdown with Grades only contains digits
                if (x.All(c => char.IsDigit(c)))
                    Cb_Grade.Items.Add(x);
        }

        private async void Bttn_AddStudent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string password = LogInViewModel.EncryptPassword(Pb_Password.Password);
                AdminViewModel.Instance.SetValuesForStudent(Tb_FirstName.Text, Tb_LastName.Text, Tb_Email.Text, password, int.Parse(Cb_Grade.SelectedValue.ToString()));
              
            }
            catch (Exception)
            {
                await new MessageDialog("Data var felaktigt inmatad, vänligen försök igen.").ShowAsync();
                return;               
            }

        }
        public void ClearValues()
        {
            Tb_FirstName.Text = "";
            Tb_LastName.Text = "";
            Tb_Email.Text = "";
            Pb_Password.Password = "";
            Cb_Grade.SelectedIndex = 0;
        }
    }
}
