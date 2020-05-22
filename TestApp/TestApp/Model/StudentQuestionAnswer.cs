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
        public StudentQuestionAnswer (int studentId, int testId, int questionId, string answer, bool? isCorrect)
        {
            StudentId = studentId;
            TestId = testId;
            QuestionId = questionId;
            Answer = answer;
            IsCorrect = isCorrect;
        }
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        public int StudentId { get; set; }
        public int TestId { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public bool? IsCorrect { get; set; }
        #endregion

        #region Methods
        #endregion
    }
}
