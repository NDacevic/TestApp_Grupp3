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

namespace TestApp.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DeleteTestView : Page
    {
        AdminViewModel adminViewModel = new AdminViewModel();
        TeacherCreateViewModel teacherCVM = TeacherCreateViewModel.Instance;
        public DeleteTestView()
        {
            this.InitializeComponent();
            adminViewModel.DisplayTests();
        }

        private void ChooseCourseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (ChooseGradeForTest.SelectedValue!= null && ChooseGradeForTest.SelectedValue!="Alla")
            {
                adminViewModel.FilterTests(ChooseCourseComboBox.SelectedValue.ToString(), int.Parse(ChooseGradeForTest.SelectedValue.ToString()));
            }
            else
            {
                adminViewModel.FilterTests(ChooseCourseComboBox.SelectedValue.ToString(), 0);
            }

        }

        private void ChooseGradeForTest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ChooseGradeForTest.SelectedValue.ToString()=="Alla")
            {
                adminViewModel.FilterListByCourse(ChooseCourseComboBox.SelectedValue.ToString());
            }
            else if(ChooseCourseComboBox.SelectedValue!=null)
            {
                adminViewModel.FilterTests(ChooseCourseComboBox.SelectedValue.ToString(), int.Parse(ChooseGradeForTest.SelectedValue.ToString()));
            }
            else
            {
                adminViewModel.FilterTests("", int.Parse(ChooseGradeForTest.SelectedValue.ToString()));
            }
        }

        private void DeleteTestButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = DisplayTestsLV.SelectedItems;
            foreach (Test selectedQuestion in selected)
            {
                adminViewModel.DeleteTest(selectedQuestion.TestId);
            }

        }

        private void PickTestButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
