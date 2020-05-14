﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Question
    {
        #region Constant Fields
        #endregion

        #region Fields
        #endregion

        #region Constructors
        public Question(int questionId, string questionType, string testQuestion, string correctAnswer, string incorrectAnswer1, string incorrectAnswer2, string courseName, int point, int time)
        {
            QuestionID = questionId;
            QuestionType = questionType;
            TestQuestion = testQuestion;
            CorrectAnswer = correctAnswer;
            IncorrectAnswer1 = incorrectAnswer1;
            IncorrectAnswer2 = incorrectAnswer2;
            CourseName = courseName;
            Point = point;
            Time = time;
        }
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        public int QuestionID { get; set; }
        public string QuestionType { get; set; }
        public string TestQuestion { get; set; }
        public string CorrectAnswer { get; set; }
        public string IncorrectAnswer1 { get; set; }
        public string IncorrectAnswer2 { get; set; }
        public string CourseName { get; set; }
        public int Point { get; set; }
        public int Time { get; set; }
        #endregion

        #region Methods
        public bool ShouldSerializeQuestionId() => false;        
        #endregion


    }
}
