﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestApp.Model;
using TestApp.View;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TestApp.ViewModel
{
    public class StudentViewModel : INotifyPropertyChanged
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
        private int numberOFQuestionsInTest;
        private ContentDialog loadScreen;
        
        public event PropertyChangedEventHandler PropertyChanged;


        #endregion

        #region Constructors
        public StudentViewModel()
        {

            ActiveTests = new ObservableCollection<Test>();

            StudentTestResult = new ObservableCollection<string>();
            AllTests = new List<Test>();
            activeStudent = LogInViewModel.Instance.ActiveStudent;
            loadScreen = new LoadDataView();


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


        //Bound to the textboxes in a test, for each question, that states how many questions are in the test
        public int NumberOfQuestionsInTest {
            get => numberOFQuestionsInTest;
            set
            {
                numberOFQuestionsInTest = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NumberOfQuestionsInTest"));
            }
        } 

        public Student ActiveStudent { get; set; }
        public ObservableCollection<string> StudentTestResult { get; set; }
        public List<Test> AllTests { get; set; }

        #endregion

        #region Methods
        /// <summary>
        /// Method to set sequence numbers for text elements in each question in a test to be written
        /// </summary>
        /// <param name="selectedTest"></param>
        public void PopulateQuestionRelatedTextBoxes(Test selectedTest)
        {
            //Initialize with "question 0" to be able to increment, and to reset between tests
            NumberOfQuestionsInTest = 0;

            //Catch the selectedTest and store localy to be used in other methods
            this.selectedTest = selectedTest;

            //Loop through all questions in the test and set the correct rownumbers for each question
            for (int i = 0; i < selectedTest.Questions.Count; i++)
            {
                selectedTest.Questions[i].RowInTest = i + 1;
                NumberOfQuestionsInTest++;
            }
        }

        /// <summary>
        /// Calls the ApiHelper to get all available tests from the database
        /// </summary>
        /// <returns></returns>
        public async Task GetAllTests()
        {
            AllTests = await ApiHelper.Instance.GetAllTests();
        }

        /// <summary>
        /// Saves all student answers
        /// </summary>
        public void FinishTest()
        {
            bool? isCorrect;
            bool isTestOnlyMultiChoice;
            int questionScore;
            int scoredTestPoints = 0;

            //Save all student answers
            List<string> answers =  GetAnswersFromListView();
            //Check if test only contains MultiChoice Questions. If true, then the test can be completelly corrected instantly and the full result can be posted to the database
            isTestOnlyMultiChoice = CheckIfTestOnlyContainsMultiChoiseQuestions(selectedTest.Questions);
                        
            //Loop through all questions
            for (int i = 0; i < selectedTest.Questions.Count; i++)
            {
                //Validate if the given answer is correct and potentially distribute points for it
                (isCorrect, questionScore) = ValidateAnswersAndCalculateTestScore(selectedTest.Questions[i], answers[i]);
                scoredTestPoints += questionScore;
                                
                //Create the result in the test object
                selectedTest.Result.Add(new StudentQuestionAnswer(activeStudent.StudentId, selectedTest.TestId, selectedTest.Questions[i].QuestionID, answers[i], isCorrect));
            }

            //Post results per question and potentially also for the test itsel
            PostQuestionAndTestResultToDb(isTestOnlyMultiChoice, scoredTestPoints);
        }

        /// <summary>
        /// Method that takes all tests from the database and saves the wanted tests to an OC, for list display
        /// </summary>
        public async Task SeeActiveTests()
        {
           
            bool isTestAlreadyWritten;
            ActiveTests.Clear();
           
            try
            {
                loadScreen.ShowAsync();              
                await GetAllTests();
                
                //Making the ApiCall here cause this is the front page when the student log in
                //Get all answers for all tests from databse
                List<StudentQuestionAnswer> allAnswers = await ApiHelper.Instance.GetAllStudentQuestionAnswers();
                loadScreen.Hide();

                //Loop through all tests and keep those that:
                // - Does not have any rows in allAnswers where studentID and testID matches activeStudent and the current test. If so the test has already been written.
                // - And where the test is constructed for the same grade/year as the student is in right now

                foreach (Test test in AllTests)
                //Loop through all tests and check which ones already has answers from the logged in student
                {
                    //State that the test has not been written until the opposite has been proven...
                    isTestAlreadyWritten = false;

                    //Loop through all answer objects.
                    foreach (StudentQuestionAnswer sqa in allAnswers)
                    {
                        //If an object is found which has this student's ID and the test's ID in it, then the test has been written before
                        if ((activeStudent.StudentId == sqa.StudentId && test.TestId == sqa.TestId))
                        {
                            isTestAlreadyWritten = true;
                            //If an answer object is found then the test has already been written, no need to look further
                            break;
                        }
                    }
                    //if the test osn't already written, and it is for the student's grade, then add it to the list
                    if (!isTestAlreadyWritten && test.Grade == activeStudent.ClassId)
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
        public void DispatcherTimerSetup(TextBlock txtBl_TestTimer, ListView lv_allQuestions, Button bttn_SubmitTest)
        {
            this.txtBl_TestTimer = txtBl_TestTimer;
            this.lv_allQuestions = lv_allQuestions;
            this.bttn_SubmitTest = bttn_SubmitTest;

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
                this.txtBl_TestTimer.Text = $"Tid kvar: {remainingTestDuration} min";
                dispatcherTimer.Start();
            }
            else
            {
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
        /// Stops the test, disables manipulation controls, starts registration of answers
        /// </summary>
        public void StopAndSubmitTest()
        {
            dispatcherTimer.Stop();
            bttn_SubmitTest.IsEnabled = false;
            lv_allQuestions.IsEnabled = false;
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

        /// <summary>
        /// Checks if the test only contains MultiChoiceQuestion
        /// </summary>
        /// <param name="questions"></param>
        /// <returns></returns>
        private bool CheckIfTestOnlyContainsMultiChoiseQuestions(ObservableCollection<Question> questions)
        {
            int numberOfTextQuestions=0;
            
            //Loop through all questions and check if there are any Text questions
            foreach (Question question in questions)
            {
                if (question.QuestionType=="Fritext")
                {
                    numberOfTextQuestions++;
                }
            }

            //If there are any Text questions the test does not only contain MultiChoice questions
            if (numberOfTextQuestions>0)
            {
                return false; //The test contains Text questions too
            }
            return true; //The test only contains MultiChoice questions
        }

        /// <summary>
        /// Validate the answer to the question and potentially distribute some points for the answer
        /// </summary>
        /// <param name="currentQuestion"></param>
        /// <param name="currentAnswer"></param>
        /// <returns></returns>
        private (bool?, int) ValidateAnswersAndCalculateTestScore(Question currentQuestion, string currentAnswer)
        {
            int scoredPoints=0;
            bool? isCorrect;

            //Validate MultipleChoice answers. If Quesion is MultipleChoice
            if (currentQuestion.QuestionType == "Flerval")
                //If the CorrectAnswer is equal to the students answer
                if (currentQuestion.CorrectAnswer == currentAnswer)
                {
                    isCorrect = true;
                    scoredPoints = currentQuestion.PointValue;
                }
                else
                    isCorrect = false;
            //If question is not MultipleChoice
            else
                isCorrect = null;

            return (isCorrect, scoredPoints);

        }
        /// <summary>
        /// Post question result and potentially also Test result to the Db
        /// </summary>
        /// <param name="isTestOnlyMultiChoice"></param>
        /// <param name="scoredTestPoints"></param>
        private void PostQuestionAndTestResultToDb(bool isTestOnlyMultiChoice, int scoredTestPoints)
        {
            //Call ApiHelper to Post the result
            ApiHelper.Instance.PostQuestionAnswers(selectedTest.Result);

            //If there are only MultiChoice questions in the test we can post the result instantly, as a TestResult object
            if (isTestOnlyMultiChoice)
            {
                ApiHelper.Instance.PostTestResult(new TestResult(activeStudent.StudentId, selectedTest.TestId, scoredTestPoints));
            }
        }

        /// <summary>
        /// Displays the students test result.
        /// </summary>
        public async void GetTestResult()
        {
            StudentTestResult.Clear();
            ObservableCollection<TestResult> tempList = await ApiHelper.Instance.GetTestResults();

                foreach (Test t in AllTests)
                {
                       foreach (TestResult tr in tempList)
                       {
                             if (t.TestId.Equals(tr.TestId))
                             {
                                 if (tr.StudentId.Equals(activeStudent.StudentId))
                                 {
                                        decimal percentage = (Convert.ToDecimal(tr.TotalPoints)/Convert.ToDecimal(t.MaxPoints))*100;
                            StudentTestResult.Add($"Ämne: {t.CourseName} Åk{t.Grade}\nDatum för provet: {t.StartDate.ToString("yyyy-MM-dd")}\nMaxpoäng: " +
                                $"{t.MaxPoints}\nDitt resultat: {tr.TotalPoints} poäng ({Math.Round(percentage, 0)}%)");
                                 }
                             }
                       }
                }      
        }
        #endregion
    }
}
