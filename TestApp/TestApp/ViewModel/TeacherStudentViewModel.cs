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
            DisplayResult = new ObservableCollection<TestResult>();
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
        public ObservableCollection<TestResult> DisplayResult { get; set; }
        public List<Student>  AllStudents { get; set; }
       
        #endregion

        #region Methods
        public void GetStudents()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Displays all tests that is already graded
        /// </summary>
        public async void DisplayAllTests()
        {
            try
            {
                StudentTestResults.Clear();
                AllTests.Clear();
                GradedTests.Clear();

                StudentTestResults = await ApiHelper.Instance.GetTestResults();
                AllTests = await ApiHelper.Instance.GetAllTests();

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
            catch (Exception exc)
            {
                await new MessageDialog(exc.Message).ShowAsync();
            }
        }
        /// <summary>
        /// Displays the test results for one chosen test in the StudentResultView
        /// </summary>
        /// <param name="testId"></param>

        public async void GetStudentResult(int testId)
        {
            AllStudents.Clear();
            AllStudents = await ApiHelper.Instance.GetAllStudents();

            DisplayStudentResult(testId);

        }
        public void DisplayStudentResult(int testId)
        {
            DisplayResult.Clear();
            foreach (TestResult tr in StudentTestResults.ToList())
            {
                if (tr.TestId == testId)
               
                   DisplayResult.Add(tr);
             
            }
        }
       
        #endregion

    }
}
