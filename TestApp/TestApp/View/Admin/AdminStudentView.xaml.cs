﻿using System;
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
        }

        private async void Bttn_AddStudent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string password = LogInViewModel.EncryptPassword(Pb_Password.Password);
                AdminViewModel.Instance.SetValuesForStudent(Tb_FirstName.Text, Tb_LastName.Text, Tb_Email.Text, password, int.Parse(Tb_Grade.Text));
              
            }
            catch (Exception)
            {
                await new MessageDialog("Data var felaktigt inmatad, vänligen försök igen.").ShowAsync();
                return;               
            }

        }
    }
}
