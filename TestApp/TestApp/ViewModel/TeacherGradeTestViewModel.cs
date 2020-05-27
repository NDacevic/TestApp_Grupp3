using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Model;
using TestApp.View;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TestApp.ViewModel

{
    public class TeacherGradeTestViewModel
    {
        #region Constant Fields
        #endregion

        #region Fields
        private static TeacherGradeTestViewModel instance = null;
        private static readonly object padlock = new object();
        private ContentDialog loadScreen = new LoadDataView();
        #endregion

        #region Constructors
        public TeacherGradeTestViewModel()
        {
            //Created but left empty intentionally in case it will be used in the future
        }
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        public static TeacherGradeTestViewModel Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance==null)
                    {
                        instance = new TeacherGradeTestViewModel();
                    }
                    return instance;
                }
            }
        }

        public List<Student> allStudents { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Calls the GetAllStudents Method in the APIHelper class
        /// </summary>
        /// <returns></returns>
        public async Task DownloadStudents()
        {

            loadScreen.ShowAsync();
            allStudents = await ApiHelper.Instance.GetAllStudents();
            loadScreen.Hide();

        }

        /// <summary>
        /// Finds the tests that contain questions that havn't been graded yet and populates a list with them.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Test>> GetUngradedTests()
        {
            await DownloadStudents();
            List<Test> ungradedTests = new List<Test>();

            foreach (var student in allStudents)
            {
                foreach(var studentTest in student.Tests)
                {
                    if(!studentTest.IsGraded)
                    {
                        if (!ungradedTests.Any(test => test.TestId == studentTest.TestId))
                            ungradedTests.Add(studentTest);
                    }
                }
            }
            
            return ungradedTests;
        }

        /// <summary>
        /// After clicking a test this method populates the list with students who've take
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="studentsWithTestList"></param>
        public void PopulateStudentsWithTestList(int testId, ObservableCollection<Student> studentsWithTestList)
        {
            var tempStudentList = allStudents.Where(student => student.Tests.Any(test => test.TestId == testId && test.IsGraded == false)).Select(x => x).ToList();
            foreach(var student in tempStudentList)
            {
                studentsWithTestList.Add(student);
            }
        }

        /// <summary>
        /// Finds the questions based on a student and test and populates a list that is bound to the Listview for grading
        /// </summary>
        /// <param name="chosenTestId"></param>
        /// <param name="chosenStudent"></param>
        /// <param name="questionsForStudentAndTestList"></param>
        public void PopulateUngradedQuestionsForStudent(int chosenTestId, Model.Student chosenStudent, ObservableCollection<Question> questionsForStudentAndTestList)
        {
            var tempQuestionList = chosenStudent.Tests.Where(test => test.TestId == chosenTestId).Select(test => test.Questions).FirstOrDefault();

            foreach (var question in tempQuestionList)
            {
                if (question.QuestionAnswer.IsCorrect == null)
                    questionsForStudentAndTestList.Add(question);
            }
        }
        /// <summary>
        /// When the user has finished grading the questions they want to grade,
        /// This method compiles the questions into a list and sends it off to the API for writing to the database.
        /// If all the questions have been graded it also sends a PATCH request to update the test for that student.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void FinishGradingTest(ListView listView_QuestionsForStudentAndTest, Student chosenStudent, int chosenTestId)
        {
            List<StudentQuestionAnswer> gradedQuestions = new List<StudentQuestionAnswer>();
            Question question;

            foreach (var item in listView_QuestionsForStudentAndTest.Items)
            {
                question = (Question)item;
                var container = listView_QuestionsForStudentAndTest.ContainerFromItem(item);
                var children = AllChildren(container);
                foreach (var control in children)
                {
                    RadioButton button = (RadioButton)control;
                    if (button.Name == "radioButton_QuestionCorrect" && button.IsChecked == true)
                    {
                        gradedQuestions.Add(new StudentQuestionAnswer(chosenStudent.StudentId, chosenTestId, question.QuestionID, question.QuestionAnswer.Answer, true) { }); //TODO: Change this to the normal constructor once Micke has implemented StudentQuestionAnswer fully
                    }
                    else if (button.Name == "radioButton_QuestionIncorrect" && button.IsChecked == true)
                    {
                        gradedQuestions.Add(new StudentQuestionAnswer(chosenStudent.StudentId, chosenTestId, question.QuestionID, question.QuestionAnswer.Answer, false) { }); //TODO: Change this to the normal constructor once Micke has implemented StudentQuestionAnswer fully
                    }
                }
            }

            var success = await ApiHelper.Instance.UpdateStudentQuestionAnswer(gradedQuestions);

            if (success && 
                gradedQuestions.Count == listView_QuestionsForStudentAndTest.Items.Count)
            {
                var points = await GetTotalPoints(chosenStudent, chosenTestId);

                TestResult result = new TestResult(chosenStudent.StudentId, chosenTestId, points);
                ApiHelper.Instance.PostTestResult(result);
            }
        }

        private async Task<int> GetTotalPoints(Model.Student chosenStudent, int chosenTestId)
        {
            List<StudentQuestionAnswer> studentQuestionAnswerList = await ApiHelper.Instance.GetAllStudentQuestionAnswers();
            var filteredSQAList = studentQuestionAnswerList.Where(sqa => sqa.StudentId == chosenStudent.StudentId && sqa.TestId == chosenTestId).Select(sqa => sqa).ToList();

            var questionList = chosenStudent.Tests.Where(t => t.TestId == chosenTestId).Select(t => t.Questions).FirstOrDefault();

            var totalPoints = 0;
            foreach(var sqa in filteredSQAList)
            {
                Question q = questionList.FirstOrDefault(x => x.QuestionID == sqa.QuestionId);
                if(q != null)
                {
                    if (sqa.IsCorrect == true)
                        totalPoints += q.PointValue;
                }
            }
            return totalPoints;
        }
        /// <summary>
        /// Goes through a UI element and gets all the children of it that are labeled as 'Controlls'
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public List<Control> AllChildren(DependencyObject parent)
        {

            //Todo: Move this method to another class
            var list = new List<Control>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is Control)
                    list.Add(child as Control);
                list.AddRange(AllChildren(child));
            }
            return list;
        }

        #endregion
    }
}
