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
        
        TeacherCreateViewModel teacherCreateViewModels  = TeacherCreateViewModel.Instance; //Skapar referens

        //TO DO: Figure out how to filter list

        public CreateTestView()
        {
            this.InitializeComponent();

            this.DataContext = teacherCreateViewModels;
            this.DataContext = teacherCreateViewModels.SubjectQuestions;
        }

        private void AddQuestionToTest_Btn_Click(object sender, RoutedEventArgs e) //Adding the question the user choose from the list to the test.
        {

            var selected = DisplayQuestionsListView.SelectedItems;
            foreach (Question selectedQuestion in selected)
            {
                teacherCreateViewModels.AddQuestionToTest(selectedQuestion); //Method that adds the question to the object.
            }

        }

        //Setting the test up and sending it to the TeacherCreateViewModel
        private void CreateTest_btn_Click(object sender, RoutedEventArgs e)
        {
            //TO DO: Fix Datetime
            
            foreach(var testPoint in teacherCreateViewModels.SubjectQuestions) //Adding all questions points and get the Max point for the test. 
            {
                teacherCreateViewModels.CreatedTest.MaxPoints += testPoint.Point;
            }

            teacherCreateViewModels.CreatedTest.CourseName = ChooseCourseComboBox.SelectedValue.ToString();
            teacherCreateViewModels.CreatedTest.Grade = int.Parse(ChooseGrade_txtBox.Text); //Implement try catch
            teacherCreateViewModels.CreatedTest.TestTime = int.Parse(TestTime_txtBox.Text); //Implement try catch
            
            teacherCreateViewModels.CreateTestToDB();
           
        }
    }
}
