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

        //TO DO: Figure out how to filter list

        public CreateTestView()
        {
            this.InitializeComponent();
            this.DataContext = teacherCreateViewModel;
            this.DataContext = teacherCreateViewModel.SubjectQuestions;

            foreach (var x in teacherCreateViewModel.Grades) //Make sure that our dropdown with Grades only contains digits
                if (x.All(c => char.IsDigit(c)))
                    ChooseGrade.Items.Add(x);

        }

        private void AddQuestionToTest_Btn_Click(object sender, RoutedEventArgs e) //Adding the question the user choose from the list to the test.
        {
            if(ChooseCourseComboBox.SelectedValue==null)//Checking if the user have choosen an subject for the test
            {
               teacherCreateViewModel.DisplayNoSubjectWarning(); //If no subject is choosen we give the user a warning.
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

     
        private void CreateTest_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Populates the Test with the given values and sends it to the DB

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

        private void RemoveQuestionFromTest_Btn_Click(object sender, RoutedEventArgs e)
        {
            var selected = DisplayAddedQuestionsListView.SelectedItems;
            foreach (Question selectedQuestion in selected)
            {
                teacherCreateViewModel.RemoveQuestionFromTest(selectedQuestion); //Method that removes the question from the object.
            }
        }

        private void ChooseCourseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            teacherCreateViewModel.CreatedTest.Questions.Clear(); //If there are questions added to the test and user changes subject, the list is reset.
            teacherCreateViewModel.CreatedTest.MaxPoints = 0; //Test Maxpoints are reset if user changes subject.
            teacherCreateViewModel.GetQuestionsForTest(ChooseCourseComboBox.SelectedValue.ToString()); //Sending CourseName to method that´s gonna get all questions on that subject

        }
      

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
        
  
        private void TestDatePicker_CalendarViewDayItemChanging(CalendarView sender, CalendarViewDayItemChangingEventArgs e)
        {
            //Greying out Sundays,Saturdays and days prior to today, so the user can´t set wrong date.
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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            teacherCreateViewModel.QuestionsToFilter.Clear();
        }
        private void ResetControllers()
        {
            teacherCreateViewModel.CreatedTest.MaxPoints = 0;
            this.Frame.Navigate(typeof(CreateTestView));
        }


    }
}
