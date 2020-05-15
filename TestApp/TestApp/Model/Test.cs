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

        #endregion

        #region Constructors
        public Test (int testId, int grade, string courseName, int maxPoints, int testTime, bool isActive, bool isTestGraded, DateTime startDate)
        {
            TestId = testId;
            Grade = grade;
            CourseName = courseName;
            MaxPoints = maxPoints;
            TestTime = testTime;
            IsActive = isActive;
            Questions = new List<Question>();
            IsTestGraded = isTestGraded;
            StartDate = startDate;
            Result = new List<StudentQuestionAnswer>();

        }
        public Test() //Created this to be able to create a test object without serializing it /Johnny
        {

        }
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        public int TestId { get; set; }
        public int Grade { get; set; }
        public string CourseName { get; set; }
        public int MaxPoints { get; set; }
        public int TestTime { get; set; }
        public bool IsActive { get; set; }
        public List<Question> Questions { get; set; }
        public bool IsTestGraded { get; set; }
        public DateTime StartDate { get; set; }
        public List<StudentQuestionAnswer> Result { get; set; }
        #endregion

        #region Methods
        public bool ShouldSerializeId()
        {
            throw new Exception();
        }
        #endregion
    }
}
