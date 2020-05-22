using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Model;
using Windows.UI.Popups;

namespace TestApp.ViewModel
{
    public class TeacherCreateViewModel:INotifyPropertyChanged
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

        private static TeacherCreateViewModel instance = null;
        private static readonly object padlock = new object();
        #endregion

        #region Constructors
        public TeacherCreateViewModel()
        {
            CreatedTest = new Test(); //Creating a test object once you enter the page.
            CreatedTest.Questions = new ObservableCollection<Question>();
            SubjectQuestions = new ObservableCollection<Question>(); //Populated from DB
            QuestionsToFilter = new ObservableCollection<Question>(); //This is the list we gonna use to filter the questions
            Courses = new List<Course>(); //Used for a dropdown combobox to filter school subject in CreateTestView and to easy apply subject to a question
            QuestionType = new List<string> //Used for a dropdown to filter question type in CreateTestView
            {
                "Alla","Flerval","Fritext"
            };

            QuestionPoint = new List<string> //Used for a dropdown to filter question point in CreateTestView and to easy choose point when creating a question
            {
                "Alla","1","2","5","10"
            };

            Grades = new List<string> //Used for a dropdown to filter question point in CreateTestView and to easy choose point when creating a question
            {
                "Alla","7","8","9"
            };
            GetCoursesForList();//Method to populate our List<Course> with courses from DB
        }
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        public static TeacherCreateViewModel Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance==null)
                    {
                        instance = new TeacherCreateViewModel();
                    }
                    return instance;
                }
            }
        }
        public Test CreatedTest { get; set; }
        public Question CreatedQuestion { get; set; }
        public ObservableCollection<Question> QuestionsToFilter { get; set; }
        public ObservableCollection<Question> SubjectQuestions { get; set; }
        public List<string> CourseName { get; set; } //Used for a dropdown combobox with course name in CreateTestView and CreateQuestionView
        public List<Course> Courses { get; set; }
        public List<string> Grades { get; set; }
        public List<string> QuestionType { get; set; } //Used for a dropdown combobox with question type in CreateTestView
        public List<string> QuestionPoint { get; set; } //Used for a dropdown combobox with question point in CreateTestView and CreateQuestionView

        #endregion

        #region Methods
        public async void GetCoursesForList() //Populating combobox with courses from DB
        {
            CourseName = new List<string>();
            Courses = await ApiHelper.Instance.GetAllCourses();
            foreach(Course course in Courses)
            {
                CourseName.Add(course.CourseName);
            }
        
        }

        public async void GetQuestionsForTest(string course) 
        {
           QuestionsToFilter.Clear(); //Everytime we want to change subject on the Test we clear the list with questions to filter.

           SubjectQuestions = await ApiHelper.Instance.GetQuestion(course); //Send CourseName to ApiHelper and want a Obs.Coll in return.

            foreach (Question questionTofilter in SubjectQuestions)
            {
                QuestionsToFilter.Add(questionTofilter); //Populating the list with questions from SubjectQuestions
            }
        }

        public async void CreateTestToDB()
        {
            try
            {
                CreatedTest.IsActive = true;
                ApiHelper.Instance.PostCreatedTestAsync(CreatedTest);
                ResetTest();
            }
            catch (Exception exc)
            {
                await new MessageDialog(exc.Message).ShowAsync();
            }
           
           
        }
        public void ResetTest()//Method to reset the object after posting the Test to DB
        {
            CreatedTest.Questions.Clear();
            QuestionsToFilter.Clear();
            CreatedTest.Questions.Clear();
        }
        public void AddQuestionToTest(Question question) //Adding question that the user choose
        {
            CreatedTest.Questions.Add(question);
            CreatedTest.MaxPoints += question.PointValue;

        }
        public void RemoveQuestionFromTest(Question question) //Removing question that the user choose
        {
            CreatedTest.Questions.Remove(question);
            CreatedTest.MaxPoints -= question.PointValue;
        }
   

        /// <summary>
        /// Calls the PostCreatedQuestion APIHelper method and submits the created question.
        /// After that it resets the created question to null
        /// </summary>
        public async void CreateQuestion()
        {
            try
            {
                ApiHelper.Instance.PostCreatedQuestion(CreatedQuestion);
                CreatedQuestion = null;
            }
            catch (Exception exc)
            {
                await new MessageDialog(exc.Message).ShowAsync();
            }
        }
        

     
        #endregion

    }
}
