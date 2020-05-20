using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Model
{
    public class Student : Person
    {
        #region Constant Fields
        #endregion

        #region Fields

        #endregion

        #region Constructors
        public Student (int studentId, string firstName, string lastName, string email, string password,int classId, List<Test> writtenTests) : base(firstName, lastName, email, password)
        {
            StudentId = studentId;
            ClassId = classId;
            Tests = writtenTests;
        }

        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public List<Test> Tests { get; set; }
        #endregion

        #region Methods
        #endregion
    }
}
