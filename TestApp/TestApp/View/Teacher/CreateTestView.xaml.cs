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

        TeacherCreateViewModel teacherCreateViewModel  = TeacherCreateViewModel.Instance; //Creating reference

        //TO DO: Figure out how to filter list

        public CreateTestView()
        {
            this.InitializeComponent();
            this.DataContext = teacherCreateViewModel;
            this.DataContext = teacherCreateViewModel.SubjectQuestions;

            foreach (var x in teacherCreateViewModel.Grades)
                if (x.All(c => char.IsDigit(c)))
                    ChooseGrade.Items.Add(x);

        }

        private void AddQuestionToTest_Btn_Click(object sender, RoutedEventArgs e) //Adding the question the user choose from the list to the test.
        {
            if(ChooseCourseComboBox.SelectedValue==null)//Checking if the user have choosen an subject for the test
            {
                DisplayNoSubjectWarning(); //If no subject is choosen we give the user a warning.
            }
            else
            {
                var selected = DisplayQuestionsListView.SelectedItems;
                foreach (Question selectedQuestion in selected)
                {
                    if (teacherCreateViewModel.CreatedTest.Questions.Contains(selectedQuestion))//Check if the question is already in the test.
                    {
                        DisplayQuestionWarning();//Displays a warning that the question already exists
                    }
                    else
                    {
                        teacherCreateViewModel.AddQuestionToTest(selectedQuestion);
                    }
            }   }
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
                teacherCreateViewModel.CreateTestToDB();
            }
            catch(NullReferenceException)
            {
                DisplayFieldsAreEmpty();
            }
            catch(FormatException)
            {
                DisplayFieldsAreEmpty();
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
        private void ResetQuestionList()
        {
            foreach (var subject in teacherCreateViewModel.SubjectQuestions) //Going through our list with questions.
            {
                if (!teacherCreateViewModel.QuestionsToFilter.Contains(subject)) //We check if our QuestionsToFilter contain our subject question
                {
                    teacherCreateViewModel.QuestionsToFilter.Add(subject);
                }
            }
        }

        private void FilterQuestionTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChooseCourseComboBox.SelectedValue == null) //If we try to filter without choosing a course we get a warning
            {
                DisplayNoSubjectWarning();
            }
            else
            {
                ResetQuestionList();

                foreach (var filtered in teacherCreateViewModel.QuestionsToFilter.ToList()) //Going through our alternative list of questions
                {
                    if (FilterQuestionTypeComboBox.SelectedValue.ToString() == "Alla") //If the user choose to se all questions then we dont remove anything.
                    {

                    }
                    else if (filtered.QuestionType != FilterQuestionTypeComboBox.SelectedValue.ToString()) //If the questions QuestionType doesnt match, we remove it.
                    {
                        teacherCreateViewModel.QuestionsToFilter.Remove(filtered);
                    }
                }
            }
        }

        private void FilterQuestionPointComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChooseCourseComboBox.SelectedValue == null)
            {
                DisplayNoSubjectWarning();
            }
            else
            {
                ResetQuestionList();
                foreach (var filtered in teacherCreateViewModel.QuestionsToFilter.ToList()) 
                {
                    if (FilterQuestionPointComboBox.SelectedValue.ToString() == "Alla")
                    {

                    }
                    else if (filtered.PointValue.ToString() != FilterQuestionPointComboBox.SelectedValue.ToString()) //If the questions Point doesnt match, we remove it.
                    {
                            teacherCreateViewModel.QuestionsToFilter.Remove(filtered);
                    }
                   
                }
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

                if (TestTimePicker.Time.Hours==0) 
                {
                    DisplayInvalidTimeForTest();
                }
                else
                {
                    teacherCreateViewModel.CreatedTest.StartDate = dateAndTime; //We give our Test the given date and time.
                }
            }
            catch(InvalidOperationException)
            {
                DisplayFieldsAreEmpty();
            }

        }
        private async void DisplayInvalidTimeForTest() //Informs user that date or time is incorrect
        {
            ContentDialog warning = new ContentDialog
            {
                Title = "Varning",
                Content = "Var vänlig se över datum och tid för prov",
                CloseButtonText = "Ok"
            };
            await warning.ShowAsync();
        }
        private async void DisplayFieldsAreEmpty()//Informs user that it can´t continue untill all fields are filled out
        {
            ContentDialog warning = new ContentDialog
            {
                Title = "Varning",
                Content = "Var vänlig se till att alla fälten är fyllda",
                CloseButtonText = "Ok"
            };
            await warning.ShowAsync();
        }
        private async void DisplayNoSubjectWarning() //Asks the user to choose a subject before trying to filter or adding a question.
        {
            ContentDialog warning = new ContentDialog
            {
                Title = "Varning",
                Content = "Var vänlig välj ett ämne för provet",
                CloseButtonText = "Ok"
            };
            await warning.ShowAsync();
        }
        private async void DisplayQuestionWarning() //Informs the user that the question is already added to the test.
        {
            ContentDialog warning = new ContentDialog
            {
                Title = "Varning",
                Content = "Denna fråga finns redan på provet",
                CloseButtonText = "Ok"
            };
            await warning.ShowAsync();
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

     
    }
}
