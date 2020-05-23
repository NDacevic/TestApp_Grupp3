using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Model;
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

        private List<Student> allStudents { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Calls the GetAllStudents Method in the APIHelper class
        /// </summary>
        /// <returns></returns>
        public async Task DownloadStudents()
        {
            allStudents = await ApiHelper.Instance.GetAllStudents();
        }

        /// <summary>
        /// Returns the list of all the students
        /// </summary>
        /// <returns></returns>
        public List<Student> GetStudents()
        {
            return allStudents;
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
                questionsForStudentAndTestList.Add(question);
        }

        /// <summary>
        /// When the user has finished grading the questions they want to grade,
        /// This method compiles the questions into a list and sends it off to the API for writing to the database.
        /// If all the questions have been graded it also sends a PATCH request to update the test for that student.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FinishGradingTest(ListView listView_QuestionsForStudentAndTest, int chosenStudentId, int chosenTestId)
        {
            List<StudentQuestionAnswer> gradedQuestions = new List<StudentQuestionAnswer>();
            Question question;

            foreach (var item in listView_QuestionsForStudentAndTest.Items)
            {
                question = (Question)item;
                var container = listView_QuestionsForStudentAndTest.ContainerFromItem(item);
                var children = AllChildren(container);
                foreach (var x in children)
                {
                    RadioButton button = (RadioButton)x;
                    if (button.Name == "radioButton_QuestionCorrect" && button.IsChecked == true)
                    {
                        gradedQuestions.Add(new StudentQuestionAnswer(chosenStudentId, chosenTestId, question.QuestionID, question.Answer, true) { }); //TODO: Change this to the normal constructor once Micke has implemented StudentQuestionAnswer fully
                    }
                    else if (button.Name == "radioButton_QuestionIncorrect" && button.IsChecked == true)
                    {
                        gradedQuestions.Add(new StudentQuestionAnswer(chosenStudentId, chosenTestId, question.QuestionID, question.Answer, false) { }); //TODO: Change this to the normal constructor once Micke has implemented StudentQuestionAnswer fully
                    }
                }
            }

            ApiHelper.Instance.UpdateStudentQuestionAnswer(gradedQuestions);

            if (listView_QuestionsForStudentAndTest.Items.Count == gradedQuestions.Count)
            {
                JsonPatchDocument<Test> jsonPatchTest = new JsonPatchDocument<Test>();
                jsonPatchTest.Replace(x => x.IsGraded, true);

                ApiHelper.Instance.PatchTest(chosenTestId, jsonPatchTest);
            }
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
