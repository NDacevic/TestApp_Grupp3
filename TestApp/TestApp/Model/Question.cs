using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Model
{
    public class Question : INotifyPropertyChanged
    {
        #region Constant Fields
        #endregion

        #region Fields
        private int rowInTest;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructors
        public Question(int questionId, string questionType, string testQuestion, string correctAnswer, string incorrectAnswer1, string incorrectAnswer2, string courseName, int point)
        {
            QuestionID = questionId;
            QuestionType = questionType;
            QuestionText = testQuestion;
            CorrectAnswer = correctAnswer;
            IncorrectAnswer1 = incorrectAnswer1;
            IncorrectAnswer2 = incorrectAnswer2;
            CourseName = courseName;
            PointValue = point;
        }
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        public int QuestionID { get; set; }
        public string QuestionType { get; set; }
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
        public string IncorrectAnswer1 { get; set; }
        public string IncorrectAnswer2 { get; set; }
        public int PointValue { get; set; }
        public string CourseName { get; set; }
        public string QuestionSummary
        {
            get { return $"[{CourseName}] Fråga: {QuestionText} [{QuestionType}] Poäng: {PointValue}"; }

        }
        public StudentQuestionAnswer QuestionAnswer { get; set; }
        
        //used to populate the textboxes in a test, for each question, that states which number the question has (1 to max)
        public int RowInTest 
        {
            get => rowInTest;
            set
            {
                rowInTest = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RowInTest"));
            }
        } 
        #endregion

        #region Methods
        /// <summary>
        /// Tells the Json converter that it shouldn't use the ID property when serializing.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeQuestionId() => false;   
        #endregion


    }
}
