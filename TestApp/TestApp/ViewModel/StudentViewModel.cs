using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Model;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TestApp.ViewModel
{
    public class StudentViewModel
    {
        #region Constant Fields
        #endregion

        #region Fields
        private static StudentViewModel instance = null;
        private Student activeStudent;
        DispatcherTimer dispatcherTimer;
        private int testDuration;           //used to keep track of the current test time
        ListView lv_allQuestions;              //used to disable the listview when the timer reaches 0
        private TextBlock txtBl_TestTimer;

        #endregion

        #region Constructors
        public StudentViewModel ()
        {
            ActiveTests = new ObservableCollection<Test>();
            //Todo: Remove later when a student can log in
            activeStudent = new Student(1, "Mikael", "Ollhage", "ja@ja.com","nej",7,null);
            
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
        public void FinishTest()
        {

        }
        public void StartTest()
        {

        }
        public void CheckResult()
        {

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
                    //Todo: kraschar eventuellt om ListView är tom
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
        public void DispatcherTimerSetup(Test selectedTest, TextBlock txtBl_TestTimer, ListView lv_allQuestions)
        {
            //Takes the TextBlock from WriteTestView and sets the ref to the local private field so it can be used in all methods in this class
            this.txtBl_TestTimer = txtBl_TestTimer;
            //Takes the ListView from WriteTestView and sets the ref to the local private field so it can be used in all methods in this class
            this.lv_allQuestions = lv_allQuestions;
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            //Sets that the timer should update once every minute
            dispatcherTimer.Interval = new TimeSpan(0, 1, 0);
            testDuration = selectedTest.TestDuration;
            dispatcherTimer.Start();
        }

        /// <summary>
        /// Dictates what will happen every timer interval
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void dispatcherTimer_Tick(object sender, object e)
        {
            //reduces by 1 every minute
            testDuration -= 1;
            //Gives TextBlock on WriteTestView new content
            txtBl_TestTimer.Text = $"Tid kvar: {testDuration} min";

            if (testDuration == 0)
            {
                dispatcherTimer.Stop();
                lv_allQuestions.IsEnabled = false;
            }
        }

        #endregion
    }
}
