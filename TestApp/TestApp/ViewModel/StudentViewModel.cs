using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Model;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TestApp.ViewModel
{
    public class StudentViewModel
    {
        #region Constant Fields
        #endregion

        #region Fields
        private static StudentViewModel instance = null;
        private Student activeStudent;
        private DispatcherTimer dispatcherTimer;
        private int remainingTestDuration;          //used to keep track of the current test time
        private ListView lv_allQuestions;           //used to disable the listview when the timer reaches 0
        private TextBlock txtBl_TestTimer;          //used to continuously update the timer
        private Button bttn_SubmitTest;             //used to disable the button when the timer reaches 0
        private Test selectedTest;


        #endregion

        #region Constructors
        public StudentViewModel()
        {
            ActiveTests = new ObservableCollection<Test>();
            //Todo: Remove later when a student can log in - MO
            activeStudent = new Student(1, "Mikael", "Ollhage", "ja@ja.com", "nej", 8, null);

        }
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        public static StudentViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StudentViewModel();
                }
                return instance;
            }
        }

        //Property used to store all currently active tests, later used to populate the Listview ActiveTests on AvailableTestsView
        public ObservableCollection<Test> ActiveTests { get; internal set; }

        public Student ActiveStudent { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Saves all student answers
        /// </summary>
        public void FinishTest()
        {
            bool? isCorrect;
            //Save all student answers
            List<string> answers =  GetAnswersFromListView();
            
            //Loop through all questions
            for (int i = 0; i < selectedTest.Questions.Count; i++)
            {
                //Validate MultipleChoice answers. If Quesion is MultipleChoice
                if (selectedTest.Questions[i].QuestionType == "Flerval")
                    //If the CorrectAnswer is equal to the students answer
                    if (selectedTest.Questions[i].CorrectAnswer == answers[0])
                        isCorrect = true;
                    else
                        isCorrect = false;
                //If question is not MultipleChoice
                else
                    isCorrect = null;

                //Create the result in the test object
                selectedTest.Result.Add(new StudentQuestionAnswer(activeStudent.StudentId, selectedTest.TestId, selectedTest.Questions[i].QuestionID, answers[i], isCorrect));
            }

            //Call ApiHelper to Post the result
            ApiHelper.Instance.PostQuestionAnswers(selectedTest.Result);

            //Todo: Implementera Post av prov som enbart har MC-frågor. Då kan ett TestResult objekt också skrivas till databasen   MO

        }



        public void CheckResult()
        {
            //Todo: Om alla frågor är MultipleChoice kan resultatet ges omgående
        }
        /// <summary>
        /// Method that takes all tests from the database and saves the wanted tests to an OC, for list display
        /// </summary>
        public async void SeeActiveTests()
        {
            ActiveTests.Clear();
            //Tries to contact API to get all Tests
            try
            {
                //Temporary list to hold all tests
                List<Test> allTests = await ApiHelper.Instance.GetAllTests();

                //Loop through and keep all tests that are active and for the correct grade/year
                foreach (Test test in allTests)
                {
                    //Todo: kraschar eventuellt om ListView är tom. Testa! - MO
                    if (test.IsActive == true && test.Grade == activeStudent.ClassId)
                    {
                        ActiveTests.Add(test);
                    }
                }
            }
            catch (Exception)
            {
                await new MessageDialog("No tests retrieved from database. Contact an admin for help.").ShowAsync();
            }
        }

        /// <summary>
        /// Sets up the test's timer
        /// </summary>
        /// <param name="selectedTest"></param>
        /// <param name="TxtBl_TestTimer"></param>
        public void DispatcherTimerSetup(Test selectedTest, TextBlock txtBl_TestTimer, ListView lv_allQuestions, Button bttn_SubmitTest)
        {
            this.txtBl_TestTimer = txtBl_TestTimer;
            this.lv_allQuestions = lv_allQuestions;
            this.bttn_SubmitTest = bttn_SubmitTest;
            this.selectedTest = selectedTest;

            //Registers the test's start time
            TimeSpan startTime = selectedTest.StartDate.TimeOfDay;
            //Registers the current time
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            //Calculates and registers how many minutes has elapsed since the test was supposed to start
            int elapsedMinutes = (currentTime - startTime).Hours * 60 + (currentTime - startTime).Minutes;

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            //Sets that the timer should update once every minute
            dispatcherTimer.Interval = new TimeSpan(0, 1, 0);

            //Sets how many minutes is left of the test
            remainingTestDuration = elapsedMinutes > 0 ? selectedTest.TestDuration - elapsedMinutes : selectedTest.TestDuration;

            //Start test if there is time remaining, else register blank answers
            if (remainingTestDuration > 0)
            {
                dispatcherTimer.Start();
            }
            else
            {
                //Todo: Testa om man kan kalla på denna som innehåller timer.Stop() trots att timern i detta fallet aldrig startats - MO
                StopAndSubmitTest();
            }
        }

        /// <summary>
        /// Dictates what will happen every timer interval
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void DispatcherTimer_Tick(object sender, object e)
        {
            //reduces by 1 every minute
            remainingTestDuration -= 1;
            //Gives TextBlock on WriteTestView new content
            txtBl_TestTimer.Text = $"Tid kvar: {remainingTestDuration} min";

            //If no test time is left
            if (remainingTestDuration < 1)
            {
                StopAndSubmitTest();
            }
        }
        /// <summary>
        /// Displays a message
        /// </summary>
        /// <param name="message"></param>
        private async Task DisplayMessage(string message)
        {
            //Display anonymous message
            _ = await new MessageDialog(message).ShowAsync();
        }

        /// <summary>
        /// Stops the test, disables manipulation controls, starts registration of answers
        /// </summary>
        public async void StopAndSubmitTest()
        {
            dispatcherTimer.Stop();
            bttn_SubmitTest.IsEnabled = false;
            lv_allQuestions.IsEnabled = false;
            await DisplayMessage("Provet är nu avslutat. Dina svar är härmed registrerade.");
            FinishTest();
        }
        /// <summary>
        /// Creates a container of each ListView row and calls method to save info from all Controls containing an answer from the student
        /// </summary>
        private List<string> GetAnswersFromListView()
        {
            DependencyObject container;                 //This will represent each ListView row. A container of Controls
            List<string> answers = new List<string>();

            //Loop through all Listview rows
            for (int i = 0; i < lv_allQuestions.Items.Count; i++)
            {
                //Get correct container from the row index
                container = lv_allQuestions.ContainerFromIndex(i);
                //submit the container to AllChildren to map which controls are in
                answers.AddRange(AllChildren(container));
            }

            return answers;
        }

        /// <summary>
        /// Recursive method that finds all elements in 1 listview row (the container/parent)
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public List<string> AllChildren(DependencyObject parent)
        {
            var answerList = new List<string>();

            //Loop through all children (elements) in the container (parent)
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                //Save current element
                var child = VisualTreeHelper.GetChild(parent, i);
                //Check if it is a Control
                if (child is Control)
                {
                    //We are only interested to save the controls containing answers, which are the TextBox or RadioButtons
                    //if the control is a TextBox...
                    if (child.GetType().Name == "TextBox")
                    {
                        //...then it is an answer, so save its Text property
                        answerList.Add((child as TextBox).Text);
                    }
                    //if the control is a RadioButton and if it is checked as the students answer...
                    else if (child.GetType().Name == "RadioButton" && ((RadioButton)(child as Control)).IsChecked == true)
                    {
                        //...then it is an answer, so save its content
                        answerList.Add((child as RadioButton).Content.ToString());
                    }
                }
                //Call this same method to see if this specific control contains any children that are of value for us. If so add all answers to the same list 
                answerList.AddRange(AllChildren(child));
            }
            //Return the list to the calling method. In the end we have looped through all elements in the listView row and saved 1 answer per question/listview row.
            return answerList;
        }

        #endregion
    }
}
