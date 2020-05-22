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

            DependencyObject container;
            List<Control> children = new List<Control>();

            for (int i = 0; i < lv_allQuestions.Items.Count; i++)
            {
               container = lv_allQuestions.ContainerFromIndex(i);
               children.AddRange(AllChildren(container));
            }

            Debug.WriteLine("Test");

        }

        /// <summary>
        /// Gets all controls for each row in the listview 
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public List<Control> AllChildren(DependencyObject parent)
        {
            var childList = new List<Control>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is Control)
                {
                    if (child.GetType().Name == "TextBox")
                    {
                        childList.Add(child as Control);
                    }
                    else if(child.GetType().Name == "RadioButton")
                    {
                        //Todo: IsChecked misses RadioButtons that were never clicked
                        if (((RadioButton)(child as Control)).IsChecked==true)
                        {
                            childList.Add(child as Control);
                        }
                    }                                       
                }
                childList.AddRange(AllChildren(child));
            }
            return childList;
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

        #endregion
    }
}
