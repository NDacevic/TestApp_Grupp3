using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace TestApp.View.Teacher
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GradeTestView : Page
    {
        TeacherGradeTestViewModel gradeInstance = TeacherGradeTestViewModel.Instance;
        Model.Employee teacherInstance = new Employee(); //TODO: Denna är ändrad av JS
        ObservableCollection<Test> ungradedTests = new ObservableCollection<Test>();
        ObservableCollection<Model.Student> studentsWithTestList = new ObservableCollection<Model.Student>();
        ObservableCollection<Model.Question> questionsForStudentAndTestList = new ObservableCollection<Question>();

        int chosenTestId = 0;
        public GradeTestView()
        {
            this.InitializeComponent();

            GetTests();
        }

        private async void GetTests()
        {
            List<Test> tempTests = await gradeInstance.GetUngradedTests();

            foreach (var x in tempTests)
                ungradedTests.Add(x);
        }

        private void InitialTestListClick(object sender, ItemClickEventArgs e)
        {
            chosenTestId = ((Test)e.ClickedItem).TestId;

            grid_InitialTestList.Visibility = Visibility.Collapsed;
            grid_StudentsUngradedTestofType.Visibility = Visibility.Visible;

            gradeInstance.PopulateStudentsWithTestList(chosenTestId, studentsWithTestList);

        }

        private void SelectStudentToGrade(object sender, ItemClickEventArgs e)
        {
            Model.Student chosenStudent = ((Model.Student)e.ClickedItem);

            grid_StudentsUngradedTestofType.Visibility = Visibility.Collapsed;
            stackPanel_QuestionsForStudentAndTest.Visibility = Visibility.Visible;

            gradeInstance.PopulateUngradedQuestionsForStudent(chosenTestId, chosenStudent, questionsForStudentAndTestList);
        }

        private void FinishGrading(object sender, RoutedEventArgs e)
        {

            ungradedTests.Clear();
            studentsWithTestList.Clear();
            questionsForStudentAndTestList.Clear();

            grid_InitialTestList.Visibility = Visibility.Visible;
            grid_StudentsUngradedTestofType.Visibility = Visibility.Collapsed;
            stackPanel_QuestionsForStudentAndTest.Visibility = Visibility.Collapsed;

        }
    }
}
