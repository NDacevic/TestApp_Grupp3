using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class CreateTestView : Page
    {
        
        TeacherCreateViewModel teacherCreateViewModel  = TeacherCreateViewModel.Instance; //Creating reference

        //TO DO: Figure out how to filter list

        public CreateTestView()
        {
            this.InitializeComponent();
            this.DataContext = teacherCreateViewModel;
            this.DataContext = teacherCreateViewModel.SubjectQuestions;
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

        //Setting the test up
        private void CreateTest_btn_Click(object sender, RoutedEventArgs e)
        {
            //TO DO: Fix Datetime

            teacherCreateViewModel.CreatedTest.CourseName = ChooseCourseComboBox.SelectedValue.ToString();
            teacherCreateViewModel.CreatedTest.Grade = int.Parse(ChooseGrade_txtBox.Text); //Implement try catch
            teacherCreateViewModel.CreatedTest.TestTime = int.Parse(TestTime_txtBox.Text); //Implement try catch
            
            teacherCreateViewModel.CreateTestToDB();
           
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
            ResetQuestionList(); //We go to this method to "reset" our list of questions.

            foreach (var filtered in teacherCreateViewModel.QuestionsToFilter.ToList()) //Going through our alternative list of questions
            {
                if (filtered.CourseName != ChooseCourseComboBox.SelectedValue.ToString()) //If the questions CourseName doesnt match the choosen Course, we remove it.
                {
                    teacherCreateViewModel.QuestionsToFilter.Remove(filtered);
                }
            }
        }
        private void ResetQuestionList()
        {
            foreach (var subject in teacherCreateViewModel.SubjectQuestions) //Going through our list with questions.
            {
                if (!teacherCreateViewModel.QuestionsToFilter.Contains(subject) && ChooseCourseComboBox.SelectedValue.ToString() == subject.CourseName) //We check if our QuestionsToFilter contain our subject question
                {
                    teacherCreateViewModel.QuestionsToFilter.Add(subject); //If not, we add it to the list.
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
                    else if (filtered.Point.ToString() != FilterQuestionPointComboBox.SelectedValue.ToString()) //If the questions Point doesnt match, we remove it.
                    {
                        teacherCreateViewModel.QuestionsToFilter.Remove(filtered);
                    }
                }
            }
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

    }
}
