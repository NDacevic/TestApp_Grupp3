﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Model
{
    public class Test:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
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
            Questions = new ObservableCollection<Question>(); //Changed List->Obs.Coll.Johnny
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

        private int maxPoints;
        public int TestTime { get; set; }
        public bool IsActive { get; set; }
        public ObservableCollection<Question> Questions { get; set; }
        public bool IsTestGraded { get; set; }
        public DateTime StartDate { get; set; }
        public List<StudentQuestionAnswer> Result { get; set; }
        public int MaxPoints
        {
            get { return maxPoints; }
            set
            {
                maxPoints = value;
                NotifyPropertyChanged("MaxPoints"); //To be able to update the points in CreateTestView when you add/remove a question
            }
        }
        #endregion

        #region Methods
        public bool ShouldSerializeId()
        {
            throw new Exception();
        }
        #endregion
    }
}
