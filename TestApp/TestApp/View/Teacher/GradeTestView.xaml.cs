using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TestApp.Model;
using TestApp.ViewModel;
using Windows.Devices.Radios;
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

namespace TestApp.View.Teacher
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GradeTestView : Page
    {
        TeacherGradeTestViewModel gradeInstance = TeacherGradeTestViewModel.Instance;

        ObservableCollection<Test> ungradedTests = new ObservableCollection<Test>();
        ObservableCollection<Model.Student> studentsWithTestList = new ObservableCollection<Model.Student>();
        ObservableCollection<Model.Question> questionsForStudentAndTestList = new ObservableCollection<Question>();

        int chosenTestId = 0;
        Model.Student chosenStudent = new Model.Student();

        public GradeTestView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets the tests and adds them to a global list
        /// </summary>
        private async void GetTests()
        {
            if (ungradedTests != null)
                ungradedTests.Clear();

            List<Test> tempTests = await gradeInstance.GetUngradedTests();

            foreach (var x in tempTests)
                ungradedTests.Add(x);
        }

        /// <summary>
        /// When the user clicks a test. It saves the ID of the test.
        /// It also shows the next listview and hides the current one.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitialTestListClick(object sender, ItemClickEventArgs e)
        {
            chosenTestId = ((Test)e.ClickedItem).TestId;

            scrollViewer_InitialTestList.Visibility = Visibility.Collapsed; 
            textBlock_TestTitle.Visibility = Visibility.Collapsed;
            
            scrollViewer_StudentsUngradedTestofType.Visibility = Visibility.Visible;
            textBlock_StudentTitle.Visibility = Visibility.Visible;

            //call the method that populates the list used in the next listview
            gradeInstance.PopulateStudentsWithTestList(chosenTestId, studentsWithTestList);

        }

        /// <summary>
        /// When the user clicks a student in the list it saves the ID of the student.
        /// It also shows the next listview and hides the current one.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectStudentToGrade(object sender, ItemClickEventArgs e)
        {
            chosenStudent = ((Model.Student)e.ClickedItem);

            scrollViewer_StudentsUngradedTestofType.Visibility = Visibility.Collapsed;
            textBlock_StudentTitle.Visibility = Visibility.Collapsed;

            scrollViewer_QuestionsForStudentAndTest.Visibility = Visibility.Visible;
            textBlock_QuestionTitle.Visibility = Visibility.Visible;

            //call the method that populates the list used in the next listview
            gradeInstance.PopulateUngradedQuestionsForStudent(chosenTestId, chosenStudent, questionsForStudentAndTestList);
        }

        /// <summary>
        /// When the user has finished grading the questions they want to grade,
        /// This method compiles the questions into a list and sends it off to the API for writing to the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FinishGrading(object sender, RoutedEventArgs e)
        {
            if (scrollViewer_QuestionsForStudentAndTest.Visibility == Visibility.Visible)
            {
                gradeInstance.FinishGradingTest(listView_QuestionsForStudentAndTest, chosenStudent, chosenTestId);

                GetTests();
                studentsWithTestList.Clear();
                questionsForStudentAndTestList.Clear();

                ResetView();
            }
        }

        /// <summary>
        /// Todo: Comments!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void ReloadAllStudentsTestsQuestionsClick(object sender, RoutedEventArgs args)
        {
            scrollViewer_QuestionsForStudentAndTest.Visibility = Visibility.Collapsed;
            scrollViewer_StudentsUngradedTestofType.Visibility = Visibility.Collapsed;
            scrollViewer_InitialTestList.Visibility = Visibility.Visible;

            await gradeInstance.DownloadStudents();
            GetTests();
        }
        /// <summary>
        /// Todo: Comments!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void PreviousListClick(object sender, RoutedEventArgs args)
        {
            if (scrollViewer_StudentsUngradedTestofType.Visibility == Visibility.Visible)
            {
                scrollViewer_StudentsUngradedTestofType.Visibility = Visibility.Collapsed;
                textBlock_StudentTitle.Visibility = Visibility.Collapsed;

                scrollViewer_InitialTestList.Visibility = Visibility.Visible;
                textBlock_TestTitle.Visibility = Visibility.Visible;
            }
            else if (scrollViewer_QuestionsForStudentAndTest.Visibility == Visibility.Visible)
            {
                scrollViewer_QuestionsForStudentAndTest.Visibility = Visibility.Collapsed;
                textBlock_QuestionTitle.Visibility = Visibility.Collapsed;

                scrollViewer_StudentsUngradedTestofType.Visibility = Visibility.Visible;
                textBlock_StudentTitle.Visibility = Visibility.Visible;
            }
        }
        /// <summary>
        /// Todo: Comments!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (gradeInstance.allStudents == null)
            {
                await gradeInstance.DownloadStudents();
            }
            GetTests();
        }

        /// <summary>
        /// Todo: Comments!
        /// </summary>
        private void ResetView()
        {
            scrollViewer_InitialTestList.Visibility = Visibility.Visible;
            scrollViewer_StudentsUngradedTestofType.Visibility = Visibility.Collapsed;
            scrollViewer_QuestionsForStudentAndTest.Visibility = Visibility.Collapsed;

            textBlock_TestTitle.Visibility = Visibility.Visible;
            textBlock_StudentTitle.Visibility = Visibility.Collapsed;
            textBlock_QuestionTitle.Visibility = Visibility.Collapsed;
        }
    }
}
