using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

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

           if (QuestionType=="Flerval")
                RandomPositioningOfAnswers();
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

        public string TopRadioButton { get; internal set; }
        public string MiddleRadioButton { get; internal set; }
        public string BottomRadioButton { get; internal set; }
        #endregion

        #region Methods

        private void RandomPositioningOfAnswers()
        {
            Random r = new Random();
            int randomNumber;
            List<int> randomPositions = new List<int>();

            while (randomPositions.Count<3)
            {
                randomNumber = r.Next(1, 4);
                if (!randomPositions.Contains(randomNumber))
                {
                    randomPositions.Add(randomNumber);
                }
            }

            TopRadioButton = GetTextContent(randomPositions[0]);
            MiddleRadioButton = GetTextContent(randomPositions[1]);
            BottomRadioButton = GetTextContent(randomPositions[2]);
        } 

        private string GetTextContent(int randomPosition)
        {
            switch (randomPosition)
            {
                case 1:
                    return CorrectAnswer;
                case 2:
                    return IncorrectAnswer1;
                case 3:
                    return IncorrectAnswer2;
                default:
                    return null; //This will never happen
            }
        }


        /// <summary>
        /// Tells the Json converter that it shouldn't use the ID property when serializing.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeQuestionId() => false;   
        #endregion
    }
}
