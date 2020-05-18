using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        /// <summary>
        /// Formats the information in the different boxes and appends them to the CreatedQuestion object.
        /// Then it calls the CreateQuestion() method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CreateQuestionClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(textBox_QuestionPoints.Text, out int points))
                    throw new FormatException("Points total invalid. Use numbers only");

                if(((ComboBoxItem)comboBox_QuestionType.SelectedValue).Content.ToString() == "Multiple Choice")
                    createInstance.CreatedQuestion = new Question(0, ((ComboBoxItem)comboBox_QuestionType.SelectedValue).Content.ToString(), textBox_questionText.Text, textBox_CorrectAnswer.Text,textBox_IncorrectAnswer1.Text, textBox_IncorrectAnswer2.Text, ((ComboBoxItem)comboBox_CourseNames.SelectedValue).Content.ToString(), points);
                else if (((ComboBoxItem)comboBox_QuestionType.SelectedValue).Content.ToString() == "Text")
                    createInstance.CreatedQuestion = new Question(0, ((ComboBoxItem)comboBox_QuestionType.SelectedValue).Content.ToString(), textBox_questionText.Text, null, null, null, ((ComboBoxItem)comboBox_CourseNames.SelectedValue).Content.ToString(), points);
                createInstance.CreateQuestion();

            }
            catch (FormatException exc)
            {
                await new MessageDialog(exc.Message).ShowAsync();
            }
        }

        /// <summary>
        /// Controls the showing and hiding of the textboxes pertaining to a multiple choice question.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_QuestionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBoxItem)comboBox_QuestionType.SelectedValue).Content.ToString() == "Multiple Choice")
            {
                grid_MultipleChoiceAnswers.Visibility = Visibility.Visible;
            }
            else if (((ComboBoxItem)comboBox_QuestionType.SelectedValue).Content.ToString() == "Text")
            {
                grid_MultipleChoiceAnswers.Visibility = Visibility.Collapsed;
            }
        }
    }
}
