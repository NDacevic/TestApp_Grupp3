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
        Model.Employee teacherInstance = new Employee(); //TODO: Denna är ändrad av JS 
        TeacherCreateViewModel createInstance = TeacherCreateViewModel.Instance;

        public CreateQuestionView()
        {
            this.InitializeComponent();

            foreach (var x in createInstance.QuestionPoint)
                if(x.All(c => char.IsDigit(c)))
                    comboBox_QuestionPoints.Items.Add(x);

            comboBox_QuestionPoints.SelectedIndex = 0;
            comboBox_CourseNames.SelectedIndex = 0;
            comboBox_QuestionType.SelectedIndex = 0;
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

                int selectedPoint = int.Parse(comboBox_QuestionPoints.SelectedValue.ToString());
                string selectedQuestionType = ((ComboBoxItem)comboBox_QuestionType.SelectedValue).Content.ToString();
                string selectedCourse = comboBox_CourseNames.SelectedValue.ToString();

                if (((ComboBoxItem)comboBox_QuestionType.SelectedValue).Content.ToString() == "Flerval")
                    createInstance.CreatedQuestion = new Question(0, selectedQuestionType, textBox_questionText.Text, textBox_CorrectAnswer.Text,textBox_IncorrectAnswer1.Text, textBox_IncorrectAnswer2.Text, selectedCourse, selectedPoint);
                else if (((ComboBoxItem)comboBox_QuestionType.SelectedValue).Content.ToString() == "Fritext")
                    createInstance.CreatedQuestion = new Question(0, selectedQuestionType, textBox_questionText.Text, null, null, null, selectedCourse, selectedPoint);
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
            Debug.WriteLine(((ComboBoxItem)comboBox_QuestionType.SelectedValue).Content.ToString());

            if (((ComboBoxItem)comboBox_QuestionType.SelectedValue).Content.ToString() == "Flerval")
            {
                grid_MultipleChoiceAnswers.Visibility = Visibility.Visible;
            }
            else if (((ComboBoxItem)comboBox_QuestionType.SelectedValue).Content.ToString() == "Fritext")
            {
                grid_MultipleChoiceAnswers.Visibility = Visibility.Collapsed;
            }
        }
    }
}
