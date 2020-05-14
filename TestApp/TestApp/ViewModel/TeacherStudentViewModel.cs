using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //Created but left empty intentionally in case it will be used in the future
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
