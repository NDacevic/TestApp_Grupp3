using Newtonsoft.Json;
using System;
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
        
        
        #region Constant Fields
        #endregion

        #region Fields
        private int maxPoints;
        #endregion

        #region Constructors
        public Test (int testId, int grade, string courseName, int maxPoints, int testDuration, bool isActive, bool isTestGraded, DateTimeOffset startDate)
        {
            TestId = testId;
            Grade = grade;
            CourseName = courseName;
            this.maxPoints = maxPoints;
            TestDuration = testDuration;
            IsActive = isActive;
            Questions = new ObservableCollection<Question>(); //Changed List->Obs.Coll.Johnny
            IsGraded = isTestGraded;
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
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        public int TestId { get; set; }
        public int Grade { get; set; }
        public string CourseName { get; set; }      
        public int TestDuration { get; set; }
        public bool IsActive { get; set; }
        public ObservableCollection<Question> Questions { get; set; }
        public bool IsGraded { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public List<StudentQuestionAnswer> Result { get; set; } = new List<StudentQuestionAnswer>();
        
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
        /// <summary>
        /// INotifyPropertyChanged catcher method
        /// </summary>
        /// <param name="caller"></param>
        private void NotifyPropertyChanged(string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }

        /// <summary>
        /// Tells the Json converter that it shouldn't use the ID property when serializing.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeTestId() => false;
        #endregion
    }
}
