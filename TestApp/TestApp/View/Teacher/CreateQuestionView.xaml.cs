using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TestApp.Model;
using TestApp.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class CreateQuestionView : Page
    {
        Model.Teacher teacherInstance = Model.Teacher.Instance;
        TeacherCreateViewModel createInstance = TeacherCreateViewModel.Instance;

        public CreateQuestionView()
        {
            this.InitializeComponent();
        }

        private async void CreateQuestionClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(textBox_QuestionPoints.Text, out int points))
                    throw new FormatException("Points total invalid. Use numbers only");

                if(comboBox_QuestionType.Text == "Multiple Choice")
                    createInstance.CreatedQuestion = new Question(0, comboBox_QuestionType.Text, textBox_questionText.Text, textBox_CorrectAnswer.Text,textBox_IncorrectAnswer1.Text, textBox_IncorrectAnswer2.Text, comboBox_CourseNames.Text, points);
                else if (comboBox_QuestionType.Text == "Text")
                    createInstance.CreatedQuestion = new Question(0, comboBox_QuestionType.Text, textBox_questionText.Text, null, null, null, comboBox_CourseNames, points);
                createInstance.CreateQuestion();

            }
            catch (FormatException exc)
            {
                await new MessageDialog(exc.Message).ShowAsync();
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(comboBox_QuestionType.Text == "Multiple Choice")
            {
                grid_MultipleChoiceAnswers.Visibility = Visibility.Visible;
            }
            else if (comboBox_QuestionType.Text == "Text")
            {
                grid_MultipleChoiceAnswers.Visibility = Visibility.Collapsed;
            }
        }
    }
}
