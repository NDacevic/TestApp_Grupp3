using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Model
{
    public class StudentQuestionAnswer
    {
        #region Constant Fields
        #endregion

        #region Fields

        #endregion

        #region Constructors
        public StudentQuestionAnswer (string answer, int totalpoints)
        {
            Answer = answer;
            TotalPoints = totalpoints;
        }
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        public string Answer { get; set; }
        public int TotalPoints { get; set; }
        #endregion

        #region Methods
        #endregion
    }
}
