using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Model;

namespace TestApp.ViewModel
{
    public class StudentViewModel
    {
        #region Constant Fields
        #endregion

        #region Fields
        private static StudentViewModel instance = null;
        private Student activeStudent;
        
        #endregion

        #region Constructors
        public StudentViewModel ()
        {

        }
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        public static StudentViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StudentViewModel();
                }
                return instance;
            }
        }

        //Property used to store all currently active tests, later used to populate the Listview ActiveTests on AvailableTestsView
        public ObservableCollection<Test> ActiveTests { get; internal set; }      
        
        public Student ActiveStudent { get; set; }
        #endregion

        #region Methods
        public void FinishTest()
        {

        }
        public void StartTest()
        {

        }
        public void CheckResult()
        {

        }
        public void SeeActiveTests()
        {
            List<Test> allTests = ApiHelper.Instance.GetAllTests();
            foreach (Test test in allTests)
            {
                if (test.IsActive==true && test.Grade==activeStudent.ClassId)
                {

                }
            }
            //Sort out the ones where IsActive=true and where Student.classId=Test.Grade
        }
        #endregion
    }
}
