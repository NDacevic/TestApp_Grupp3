using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Model;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

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
        /// <summary>
        /// We fill our combobox with courses from DB
        /// </summary>
        public async void GetCoursesForList() 
        {
            try
            {
                CourseName = new List<string>();
                Courses = await ApiHelper.Instance.GetAllCourses();
                foreach (Course course in Courses)
                {
                    CourseName.Add(course.CourseName);
                }
            }
            catch (Exception exc)
            {
                await new MessageDialog(exc.Message).ShowAsync();
            }
        }

        /// <summary>
        /// Used to display questions thats only related to the given course
        /// </summary>
        /// <param name="course"></param>
        public async void GetQuestionsForTest(string course) 
        {
           QuestionsToFilter.Clear(); 

           SubjectQuestions = await ApiHelper.Instance.GetQuestion(course); 

            foreach (Question questionTofilter in SubjectQuestions)
            {
                QuestionsToFilter.Add(questionTofilter); 
            }
        }

        /// <summary>
        /// We send our created test to be saved in DB.
        /// </summary>
        public async void CreateTestToDB()
        {
            if (CreatedTest.Questions.Count != 0)
            {
                try
                {
                    CreatedTest.IsActive = true;
                    await ApiHelper.Instance.PostCreatedTestAsync(CreatedTest);
                    ResetTest();
                }
                catch (Exception exc)
                {
                    await new MessageDialog(exc.Message).ShowAsync();
                }
            }
            else
            {
                DisplayNoQuestionsOnTest();
            }
        }
        /// <summary>
        /// Method to reset the object after posting the Test to DB
        /// </summary>
        public void ResetTest()
        {
            CreatedTest.Questions.Clear();
            QuestionsToFilter.Clear();
            CreatedTest.Questions.Clear();
        }

        /// <summary>
        /// Adding question that the user picked
        /// </summary>
        /// <param name="question"></param>
        public void AddQuestionToTest(Question question)
        {          
                CreatedTest.Questions.Add(question);
                QuestionsToFilter.Remove(question);
                CreatedTest.MaxPoints += question.PointValue;     
        }
        /// <summary>
        /// Removing question that the user picked
        /// </summary>
        /// <param name="question"></param>
        public void RemoveQuestionFromTest(Question question) 
        {
            CreatedTest.Questions.Remove(question);
            QuestionsToFilter.Add(question);
            CreatedTest.MaxPoints -= question.PointValue;
        }
        /// <summary>
        /// Resets list of questions after filtering.
        /// </summary>
        public void ResetQuestionList()
        {
            foreach (var subject in SubjectQuestions)
            {
                if (!QuestionsToFilter.Contains(subject))
                {
                    QuestionsToFilter.Add(subject);
                }
            }
        }
        /// <summary>
        /// We filter our list with questions by a questions PointValue depending on the parameters given by the user
        /// </summary>
        /// <param name="point"></param>
        /// <param name="type"></param>
        public void FilterQuestionByPoint(string point, string type)
        {
            ResetQuestionList();

            foreach (var filtered in QuestionsToFilter.ToList())
            {
                if (point == "Alla")
                {

                }
                else if (filtered.PointValue.ToString() != point)
                {
                    QuestionsToFilter.Remove(filtered);
                }
                if (type != "" && type != "Alla")
                {
                    if (filtered.QuestionType != type)
                    {
                        QuestionsToFilter.Remove(filtered);
                    }
                }
            }
        }
        /// <summary>
        /// We filter our list with questions by a questions QuestionType depending on the parameters given by the user
        /// </summary>
        /// <param name="type"></param>
        /// <param name="point"></param>
        public void FilterQuestionByType(string type,string point)
        {
            ResetQuestionList();

            foreach (var filtered in QuestionsToFilter.ToList()) 
            {
                if (type == "Alla") //If the user choose to se all questions then we dont remove anything.
                {

                }
                else if (filtered.QuestionType != type) //If the questions QuestionType doesnt match, we remove it.
                {
                  QuestionsToFilter.Remove(filtered);
                }
                if (point != "" && point != "Alla")
                {
                    if (filtered.PointValue != int.Parse(point))
                    {
                        QuestionsToFilter.Remove(filtered);
                    }
                }
            }
        }
   

        /// <summary>
        /// Calls the PostCreatedQuestion APIHelper method and submits the created question.
        /// After that it resets the created question to null
        /// </summary>
        public async Task<bool> CreateQuestion()
        {
            try
            {
                bool success = await ApiHelper.Instance.PostCreatedQuestion(CreatedQuestion);
                CreatedQuestion = null;
                return success;
            }
            catch (Exception exc)
            {
                await new MessageDialog(exc.Message).ShowAsync();
                return false;
            }
        }
        /// <summary>
        /// Informs user that date or time is incorrect
        /// </summary>
        public async void DisplayInvalidTimeForTest() 
        {
            ContentDialog warning = new ContentDialog
            {
                Title = "Varning",
                Content = "Var vänlig se över datum och tid för prov",
                CloseButtonText = "Ok"
            };
            await warning.ShowAsync();
        }
        /// <summary>
        /// Informs user that it can´t continue untill all fields are filled out
        /// </summary>
        public async void DisplayFieldsAreEmpty()
        {
            ContentDialog warning = new ContentDialog
            {
                Title = "Varning",
                Content = "Var vänlig se till att alla fälten är fyllda",
                CloseButtonText = "Ok"
            };
            await warning.ShowAsync();
        }
        /// <summary>
        /// Asks the user to choose a subject before trying to filter or adding a question.
        /// </summary>
        public async void DisplayNoSubjectWarning() 
        {
            ContentDialog warning = new ContentDialog
            {
                Title = "Varning",
                Content = "Var vänlig välj ett ämne för provet",
                CloseButtonText = "Ok"
            };
            await warning.ShowAsync();
        }
        /// <summary>
        /// Asks the user to choose a subject before trying to filter or adding a question.
        /// </summary>
        public async void DisplayNoQuestionsOnTest() 
        {
            ContentDialog warning = new ContentDialog
            {
                Title = "Varning",
                Content = "Du måste lägga till frågor på provet",
                CloseButtonText = "Ok"
            };
            await warning.ShowAsync();
        }
        #endregion
    }
}
