using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Model;
using Windows.UI.Popups;

namespace TestApp.ViewModel
{
    public class TeacherCreateViewModel
    {
        #region Constant Fields
        #endregion

        #region Fields
        private static TeacherCreateViewModel instance = null;
        private static readonly object padlock = new object();
        #endregion

        #region Constructors
        public TeacherCreateViewModel()
        {
            //Hardcoded questions intended for testing. These will be removed when the database is up and running.
            SubjectQuestions.Add(new Question(1, "Flervalsfråga", "Vad heter huvudstaden i Sverige?", "Stockholm", "Göteborg", "Malmö", "Geografi", 5));
            SubjectQuestions.Add(new Question(2, "Flervalsfråga", "Vilket år startade 1:a världskriget?","1914","1915","1912","Historia", 5));
            SubjectQuestions.Add(new Question(2, "Flervalsfråga", "Vilket år startade 2:a världskriget?", "1939", "1940", "1930", "Historia", 5));
            SubjectQuestions.Add(new Question(2, "Flervalsfråga", "Vilket år dog Olof Palme?", "1986", "1987", "Han lever forfarande", "Historia", 5));
            SubjectQuestions.Add(new Question(2, "Flervalsfråga", "Vad är pi?", "3.14", "3.11", "11", "Matematik", 5));

            CourseName = new List<string> //Dropdown för ämne på CreateTestView, ska bindas till en combobox
            {
                "Historia","Svenska","Matematik","Engelska","Geografi"
            };
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
        public List<Question> SubjectQuestions { get; set; }
        public List<string> CourseName { get; set; } //Dropdown för ämne på CreateTestView
        
        #endregion

        #region Methods
        public void CreateTest()
        {
            throw new NotImplementedException();
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

        public void GetQuestionsForTestCreation()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
