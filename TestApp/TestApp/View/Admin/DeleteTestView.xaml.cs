using System;
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
        /// Todo: Comments!
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
        /// Todo: Comments!
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
        /// Todo: Comments!
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
        /// Todo: Comments!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PickTestButton_Click(object sender, RoutedEventArgs e) //Pick a test and display its content
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
        /// Todo: Comments!
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            TeacherCreateViewModel.Instance.QuestionsToFilter.Clear();
        }

        /// <summary>
        /// Todo: Comments!
        /// </summary>
        private void ResetControlls()
        {
            adminViewModel.TestQuestions.Clear();
            TestDate.Text = "";
            TestGrade.Text = "";
            TestCourse.Text = "";
        }
    }
}
