using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class RemoveQuestionView : Page
    {
        AdminViewModel adminVM = AdminViewModel.Instance;
        TeacherCreateViewModel teacherCVM = TeacherCreateViewModel.Instance;
        public RemoveQuestionView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// We send the choosen course from the combobox further to display all questions related to that course.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseCourseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            teacherCVM.GetQuestionsForTest(ChooseCourseComboBox.SelectedValue.ToString());
        }

        /// <summary>
        /// Sends the picked questions further to be deleted from DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteQuestion_btn_Click(object sender, RoutedEventArgs e)
        {
            var selectedQuestion = DisplayQuestionsLV.SelectedItems;
            foreach(Model.Question question in selectedQuestion)
            {
                adminVM.DeleteQuestion(question);

            }
        }
        /// <summary>
        /// Clearing list of questions when navigated this page.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            teacherCVM.QuestionsToFilter.Clear();
        }
    }
}
