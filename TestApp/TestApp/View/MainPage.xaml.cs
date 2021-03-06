﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TestApp.View;
using TestApp.View.Admin;
using TestApp.View.Student;
using TestApp.View.Teacher;
using Windows.ApplicationModel.Activation;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //When this page is navigated to it instantly navigates the 2 frames inside to different pages depending on what string value
            //is passed along inside the NavigationEventArgs object.
            if ((string)e.Parameter == "Elev")
            {
                mainMenuFrame.Navigate(typeof(StudentMenuPage),mainFrame);
                mainFrame.Navigate(typeof(AvailableTestsView));
            }
            else if ((string)e.Parameter == "Teacher")
            {
                mainMenuFrame.Navigate(typeof(TeacherMenuPage), mainFrame);
                mainFrame.Navigate(typeof(GradeTestView));
            }
            else if ((string)e.Parameter == "Admin")
            {
                mainMenuFrame.Navigate(typeof(AdminMenuPage), mainFrame);
                mainFrame.Navigate(typeof(RemoveUserView)); //This is going to change to "AddUserPage" instead
            }
        }
    }
}
