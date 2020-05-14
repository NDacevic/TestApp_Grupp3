using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
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

        }
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        public TeacherStudentViewModel Instance
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
