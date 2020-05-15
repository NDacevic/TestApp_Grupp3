using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Model
{
    public class Student
    {
        #region Constant Fields
        #endregion

        #region Fields

        #endregion

        #region Constructors
        public Student (int studentId, List<Test> writtenTests)
        {
            StudentId = studentId;
            WrittenTests = writtenTests;
        }

        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        public int StudentId { get; set; }
        public List<Test> WrittenTests { get; set; }
        #endregion

        #region Methods
        #endregion
    }
}
