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
        public bool ShouldSerializeId()
        {
            throw new Exception();
        }
        #endregion
    }
}
