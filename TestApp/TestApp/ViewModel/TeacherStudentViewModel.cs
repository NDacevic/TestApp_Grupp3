using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Model;
using Windows.UI.Popups;

namespace TestApp.ViewModel
{
    public class TeacherStudentViewModel
    {
        #region Constant Fields
        #endregion

        #region Fields
        private static TeacherStudentViewModel instance = null;
        private static readonly object padlock = new object();
        #endregion

        #region Constructors
        public TeacherStudentViewModel()
        {
            AllStudents = new List<Student>();
            DisplayResult = new ObservableCollection<string>();
            GradedTests = new ObservableCollection<Test>();
            StudentTestResults = new ObservableCollection<TestResult>();
            AllTests = new List<Test>();
        }
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        public static TeacherStudentViewModel Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance==null)
                    {
                        instance = new TeacherStudentViewModel();
                    }
                    return instance;
                }
            }
        }
        
        public List <Test> AllTests { get; set; } //Keep all tests
        public ObservableCollection<Test> GradedTests { get; set; } //Keep all graded test in here
        public ObservableCollection<TestResult> StudentTestResults { get; set; } //Keep all students with graded tests
        public string StudentName { get; set; }
        public ObservableCollection<string> DisplayResult { get; set; } //Displays all students that has completed the choosen test
        public List<Student> AllStudents { get; set; }
       
        #endregion

        #region Methods
    
        /// <summary>
        /// Used to get all TestResults and all Tests from DB
        /// </summary>
        
        public async void GetTests()
        {
            StudentTestResults.Clear();
            AllTests.Clear();
            try
            {
                StudentTestResults = await ApiHelper.Instance.GetTestResults();
                AllTests = await ApiHelper.Instance.GetAllTests();
            }
           
            catch (Exception exc)
            {
                await new MessageDialog(exc.Message).ShowAsync();
            }
            DisplayAllTests();
        }
        /// <summary>
        /// Displays all the tests that have been finished
        /// </summary>
        public void DisplayAllTests()
        {
            GradedTests.Clear();
            foreach(TestResult tr in StudentTestResults.ToList())
            {
                foreach (Test t in AllTests.ToList())
                {
                     if (t.TestId == tr.TestId&&!GradedTests.Contains(t))
                     {
                         GradedTests.Add(t);
                     }
                }
            }
            
        }
        /// <summary>
        /// Used to get all students from DB
        /// </summary>
        /// <param name="testId"></param>

        public async void GetStudentResult(int testId)
        {
            AllStudents.Clear();
            try
            {
                AllStudents = await ApiHelper.Instance.GetAllStudents();
            }
            catch (Exception exc)
            {
                await new MessageDialog(exc.Message).ShowAsync();
            }

            DisplayStudentResult(testId);

        }
        /// <summary>
        /// Displays all students test results for one chosen test in the StudentResultView
        /// </summary>
        /// <param name="testId"></param>
        public void DisplayStudentResult(int testId)
        {
            
            DisplayResult.Clear();
            foreach (TestResult tr in StudentTestResults.ToList())
            {
                foreach (Student student in AllStudents)
                {
                    if (tr.TestId == testId&&student.StudentId==tr.StudentId)
                    {
                        
                        DisplayResult.Add($"TestId: {tr.TestId}\nNamn: {student.FirstName} {student.LastName}\nPoäng: {tr.TotalPoints}");
                        
                    }
                }
            }
        }       
        #endregion
    }
}
