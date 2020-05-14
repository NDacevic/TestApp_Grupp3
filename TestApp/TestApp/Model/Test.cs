using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Model
{
    public class Test
    {
        #region Constant Fields
        #endregion

        #region Fields
        private int TestId { get; set; }
        private int Grade { get; set; }
        private string CourseName { get; set; }
        private int MaxPoints { get; set; }
        private int TestTime { get; set; }
        private bool IsActive { get; set; }
        private List<Question> Questions { get; set; }
        private bool IsTestGrades { get; set; }
        private DateTime StartDate { get; set; }
        private List<StudentQuestionAnswer> Result { get; set; }
        #endregion

        #region Constructors
        public Test ()
        {

        }
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        #endregion

        #region Methods
        public bool ShouldSerializeId()
        {
            throw new Exception();
        }
        #endregion
    }
}
