using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TestApp.Model;
using TestApp.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TestApp.View.Teacher
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateTestView : Page,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }

        TeacherCreateViewModel teacherCreateViewModel  = TeacherCreateViewModel.Instance;

        public CreateTestView()
        {
            this.InitializeComponent();
            this.DataContext = teacherCreateViewModel;
            this.DataContext = teacherCreateViewModel.SubjectQuestions;

            foreach (var x in teacherCreateViewModel.Grades) //Make sure that our dropdown with Grades only contains digits
                if (x.All(c => char.IsDigit(c)))
                    ChooseGrade.Items.Add(x);
        }

        /// <summary>
        /// Checking if the user have choosen an subject for the test.
        /// If no subject is choosen we give the user a warning. Otherwise we send the question further and adds it to the test.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddQuestionToTest_Btn_Click(object sender, RoutedEventArgs e)
        {
            if(ChooseCourseComboBox.SelectedValue==null)
            {
               teacherCreateViewModel.DisplayNoSubjectWarning(); 
            }
            else
            {
                var selected = DisplayQuestionsListView.SelectedItems;
                foreach (Question selectedQuestion in selected)
                {
                    teacherCreateViewModel.AddQuestionToTest(selectedQuestion);
                }  
            }
        }

        /// <summary>
        /// Populates the Test with the given values and sends it to method that adds it to DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateTest_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                teacherCreateViewModel.CreatedTest.CourseName = ChooseCourseComboBox.SelectedValue.ToString();
                teacherCreateViewModel.CreatedTest.Grade = int.Parse(ChooseGrade.SelectedValue.ToString());
                teacherCreateViewModel.CreatedTest.TestDuration = int.Parse(TestTime_txtBox.Text);
                AddDateAndTimeToTest();
                
            }
            catch(Exception)
            {
                teacherCreateViewModel.DisplayFieldsAreEmpty();
            }
        }
        /// <summary>
        /// Sends picked question and sends it further to remove it from the test.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveQuestionFromTest_Btn_Click(object sender, RoutedEventArgs e)
        {
            var selected = DisplayAddedQuestionsListView.SelectedItems;
            foreach (Question selectedQuestion in selected)
            {
                teacherCreateViewModel.RemoveQuestionFromTest(selectedQuestion); 
            }
        }
        /// <summary>
        /// If there are questions added to the test and user changes subject, the list is reset.
        /// Test Maxpoints are also reset if user changes subject.
        /// We send the coursename further to only display questions related to that course.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseCourseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            teacherCreateViewModel.CreatedTest.Questions.Clear(); 
            teacherCreateViewModel.CreatedTest.MaxPoints = 0; 
            teacherCreateViewModel.GetQuestionsForTest(ChooseCourseComboBox.SelectedValue.ToString()); 

        }

        /// <summary>
        /// Used to send different information further to filter the list of questions based on the users choice.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterQuestionTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChooseCourseComboBox.SelectedValue == null) //If we try to filter without choosing a course we get a warning
            {
                teacherCreateViewModel.DisplayNoSubjectWarning();
            }
            else
            {
                if(FilterQuestionPointComboBox.SelectedValue==null)
                    teacherCreateViewModel.FilterQuestionByType(FilterQuestionTypeComboBox.SelectedValue.ToString(),""); //If filter by point is not used we send this.
                else
                    teacherCreateViewModel.FilterQuestionByType(FilterQuestionTypeComboBox.SelectedValue.ToString(), FilterQuestionPointComboBox.SelectedValue.ToString()); //If filter by point is used we send this

            }
        }
        /// <summary>
        /// Used to send different information further to filter the list of questions based on the users choice.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterQuestionPointComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChooseCourseComboBox.SelectedValue == null)
            {
                teacherCreateViewModel.DisplayNoSubjectWarning();
            }
            else
            {
                if (FilterQuestionTypeComboBox.SelectedValue == null)
                    teacherCreateViewModel.FilterQuestionByPoint(FilterQuestionPointComboBox.SelectedValue.ToString(), "");
                else
                    teacherCreateViewModel.FilterQuestionByPoint(FilterQuestionPointComboBox.SelectedValue.ToString(), FilterQuestionTypeComboBox.SelectedValue.ToString());
            }
        }
        /// <summary>
        /// Assigning time and date to our test based on the users choice. 
        /// </summary>
        private void AddDateAndTimeToTest()
        {
            //Setting date and time of when the Test starts.
            try
            {
                DateTimeOffset dateAndTime; //Converting the choosen date and time given by the user to a DateTimeOffset object.
                dateAndTime = new DateTimeOffset(TestDatePicker.Date.Value.Year, TestDatePicker.Date.Value.Month, TestDatePicker.Date.Value.Day
              , TestTimePicker.Time.Hours-2, TestTimePicker.Time.Minutes, TestTimePicker.Time.Seconds,
                                         new TimeSpan(0, 0, 0));
               
                    teacherCreateViewModel.CreatedTest.StartDate = dateAndTime; //We give our Test the given date and time.
                    teacherCreateViewModel.CreateTestToDB();

            }
            catch(InvalidOperationException)
            {
                teacherCreateViewModel.DisplayFieldsAreEmpty();
            }
            catch(ArgumentOutOfRangeException)
            {
                teacherCreateViewModel.DisplayInvalidTimeForTest();
            }
            ResetControllers();
        }
        
        /// <summary>
        /// Give our calender datepicker some rules, so that the user can´t create a test on days prior to this and weekends.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestDatePicker_CalendarViewDayItemChanging(CalendarView sender, CalendarViewDayItemChangingEventArgs e)
        {
            
            if (e.Item.Date < DateTime.Today)
            {
                e.Item.IsBlackout = true;
            }
            if(e.Item.Date.DayOfWeek.ToString()=="Sunday") 
            {
                e.Item.IsBlackout = true;
            }
            if(e.Item.Date.DayOfWeek.ToString() == "Saturday")
            {
                e.Item.IsBlackout = true;
            }
        }
        /// <summary>
        /// We clear the list of questions when user navigates to this, to make sure there is no old list saved.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            teacherCreateViewModel.QuestionsToFilter.Clear();
        }
        /// <summary>
        /// When a test is created, we reset all controlls.
        /// </summary>
        private void ResetControllers()
        {
            teacherCreateViewModel.CreatedTest.MaxPoints = 0;
            this.Frame.Navigate(typeof(CreateTestView));
        }


    }
}
