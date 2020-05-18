using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Model;

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
            AllTests = new ObservableCollection<Test>();
            AllTests.Add(new Test(1, 7, "Svenska", 50, 60, false, true, DateTime.Now));
            StudentTestResults = new ObservableCollection<TestResult>();
            StudentTestResults.Add(new TestResult(1, 1, 30));
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
        public ObservableCollection<Test> AllTests { get; set; }
        public ObservableCollection<TestResult> StudentTestResults { get; set; }
        #endregion

        #region Methods
        public void GetStudents()
        {
            throw new NotImplementedException();
        }

        public void DisplayStudentResult()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
