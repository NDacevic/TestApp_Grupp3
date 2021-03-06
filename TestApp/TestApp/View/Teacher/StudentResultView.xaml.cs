﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TestApp.ViewModel;
using TestApp.Model;
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

namespace TestApp.View.Teacher
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StudentResultView : Page
    {
        public StudentResultView()
        {
            this.InitializeComponent();
            GetAllTests();
        }
        /// <summary>
        /// We calls method to display all finished tests in a listview.
        /// </summary>
        private void GetAllTests()
        {
            TeacherStudentViewModel.Instance.GetTests();
        }
     
        /// <summary>
        /// When clicked, we send our test further to display the result of all students that have finished the choosen test.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bttn_SeeStudentResult_Click(object sender, RoutedEventArgs e) 
        {
            Test test = (Test)Lv_AllTests.SelectedItem;
            if(test != null)
                TeacherStudentViewModel.Instance.GetStudentResult(test.TestId);
        }

        private void Lv_AllTests_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            Bttn_SeeStudentResult_Click(sender, e);
        }
    }
}
