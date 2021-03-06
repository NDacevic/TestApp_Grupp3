﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace TestApp.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DeleteTestView : Page
    {
        AdminViewModel adminViewModel = new AdminViewModel();
        TeacherCreateViewModel teacherCVM = TeacherCreateViewModel.Instance;
        public DeleteTestView()
        {
            this.InitializeComponent();
            adminViewModel.DisplayTests();
        }

        /// <summary>
        /// Choosing course in combobox, sending further to filter list of tests.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseCourseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (ChooseGradeForTest.SelectedValue!= null && ChooseGradeForTest.SelectedValue!="Alla")
            {
                adminViewModel.FilterTests(ChooseCourseComboBox.SelectedValue.ToString(), int.Parse(ChooseGradeForTest.SelectedValue.ToString()));
            }
            else
            {
                adminViewModel.FilterTests(ChooseCourseComboBox.SelectedValue.ToString(), 0);
            }

        }

        /// <summary>
        /// Choosing grade in combobox, sending further to filter list of tests.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseGradeForTest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ChooseGradeForTest.SelectedValue.ToString()=="Alla")
            {
                if(ChooseCourseComboBox.SelectedValue==null)
                {
                    adminViewModel.FilterListByCourse("");
                }
                else
                {
                    adminViewModel.FilterListByCourse(ChooseCourseComboBox.SelectedValue.ToString());
                }
            }
            else if(ChooseCourseComboBox.SelectedValue!=null)
            {
                adminViewModel.FilterTests(ChooseCourseComboBox.SelectedValue.ToString(), int.Parse(ChooseGradeForTest.SelectedValue.ToString()));
            }
            else
            {
                adminViewModel.FilterTests("", int.Parse(ChooseGradeForTest.SelectedValue.ToString()));
            }
        }

        /// <summary>
        /// Sends the choosen test in listview to method for deletion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteTestButton_Click(object sender, RoutedEventArgs e) //Pick a Test and delete it
        {
            var selected = DisplayTestsLV.SelectedItems;
            foreach (Test selectedQuestion in selected)
            {
                adminViewModel.DeleteTest(selectedQuestion.TestId);
            }
            ResetControlls();
        }

        /// <summary>
        /// Pickas a test and displays details about the choosen test.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PickTestButton_Click(object sender, RoutedEventArgs e) 
        {
            var selected = DisplayTestsLV.SelectedItems;
            foreach (Test selectedTest in selected)
            {
                TestDate.Text = selectedTest.StartDate.ToString("yyyy-MM-dd");
                TestGrade.Text = selectedTest.Grade.ToString();
                TestCourse.Text = selectedTest.CourseName;
                adminViewModel.DisplayQuestionsOnTest(selectedTest);
            }
        }

        /// <summary>
        /// Clearing list of questions when navigated to page.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            TeacherCreateViewModel.Instance.QuestionsToFilter.Clear();
        }

        /// <summary>
        /// Resets the controlls after test have been deleted from DB
        /// </summary>
        private void ResetControlls()
        {
            adminViewModel.TestQuestions.Clear();
            TestDate.Text = "";
            TestGrade.Text = "";
            TestCourse.Text = "";
        }

        private void DisplayTestsLV_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            PickTestButton_Click(sender, e);
        }
    }
}
