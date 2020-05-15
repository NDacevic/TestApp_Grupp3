using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Model
{
    class TestResult
    {
        #region Constant Fields
        #endregion

        #region Fields
        #endregion

        #region Constructors
        public TestResult(int studentId, int testId, int totalPoints)
        {
            StudentId = studentId;
            TestId = testId;
            TotalPoints = totalPoints;

        }
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        public int StudentId { get; set; }
        public int TestId { get; set; }
        public int TotalPoints { get; set; }
        #endregion

        #region Methods
        #endregion

    }
}
