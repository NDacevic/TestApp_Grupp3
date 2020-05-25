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
            if (e.Parameter == "Elev")
            {
                mainMenuFrame.Navigate(typeof(MainPageStudentView),mainFrame);
                mainFrame.Navigate(typeof(AvailableTestsView));
            }
            else if (e.Parameter == "Lärare")
            {
                mainMenuFrame.Navigate(typeof(MainPageTeacherView), mainFrame);
                mainFrame.Navigate(typeof(GradeTestView));
            }
            else if (e.Parameter == "Admin")
            {
                mainMenuFrame.Navigate(typeof(MainPageAdminViewxaml), mainFrame);
                mainFrame.Navigate(typeof(RemoveUserView)); //This is going to change to "AddUserPage" instead
            }

        }

        private void Button_NavigateToPage(object sender, RoutedEventArgs e)
        {
            //    if (sender == bttn_HistoricalTestsViewNavigate)
            //        mainFrame.Navigate(typeof(HistoricalTestsView));
            //    else if (sender == bttn_MainPageStudentViewNavigate)
            //        mainFrame.Navigate(typeof(MainPageStudentView));
            //    else if (sender == bttn_WriteTestViewNavigate)
            //        mainFrame.Navigate(typeof(WriteTestView));
            //    else if (sender == bttn_CreateQuestionViewNavigate)
            //        mainFrame.Navigate(typeof(CreateQuestionView));
            //    else if (sender == bttn_CreateTestViewNavigate)
            //        mainFrame.Navigate(typeof(CreateTestView));
            //    else if (sender == bttn_GradeTestViewNavigate)
            //        mainFrame.Navigate(typeof(GradeTestView));
            //    else if (sender == bttn_MainPageTeacherViewNavigate)
            //        mainFrame.Navigate(typeof(MainPageTeacherView));
            //    else if (sender == bttn_StudentResultViewNavigate)
            //        mainFrame.Navigate(typeof(StudentResultView));
            //    else if (sender == bttn_LogInViewNavigate)
            //        mainFrame.Navigate(typeof(LogInView));
            //    else if (sender == bttn_AvailableTestsViewNavigate)
            //        mainFrame.Navigate(typeof(AvailableTestsView));
            //    else if (sender == bttn_deleteTestView)
            //        mainFrame.Navigate(typeof(DeleteTestView));
            //    else if (sender == bttn_LogInView)
            //        mainFrame.Navigate(typeof(LogInView));
            //    else if (sender == bttn_DeleteUser)
            //        mainFrame.Navigate(typeof(RemoveUserView));
        }

    }
}
