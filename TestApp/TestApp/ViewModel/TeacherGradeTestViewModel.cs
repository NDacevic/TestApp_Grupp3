using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Model;

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
        #endregion

        #region Methods
        public void GradeTextQuestion()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Test>> GetUngradedTests()
        {
            List<Student> allStudents = await ApiHelper.Instance.GetAllStudents();
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

        public void GetUngradedQuestion()
        {
            throw new NotImplementedException();
        }
        #endregion


    }
}
