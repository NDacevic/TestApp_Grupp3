using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
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

        public void GetUngradedTest()
        {
            throw new NotImplementedException();
        }

        public void GetUngradedQuestion()
        {
            throw new NotImplementedException();
        }
        #endregion


    }
}
