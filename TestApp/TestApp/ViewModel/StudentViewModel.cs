using System;
using System.Collections.Generic;
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
        private Student ActiveStudent { get; set; }
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
        public void SeeActiveTest()
        {

        }
        #endregion
    }
}
