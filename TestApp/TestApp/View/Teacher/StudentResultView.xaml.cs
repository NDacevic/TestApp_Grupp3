using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TestApp.ViewModel;
using TestApp.Model;
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
    public sealed partial class StudentResultView : Page
    {
        TeacherStudentViewModel teacherStudentViewModel { get; set; }
        public StudentResultView()
        {
            this.InitializeComponent();
            GetAllTests();
        }
        private void GetAllTests()
        {
            TeacherStudentViewModel.Instance.DisplayAllTests();
        }
        private void GetTestResult()
        {
            Test test = (Test)Lv_AllTests.SelectedItem;
            TeacherStudentViewModel.Instance.DisplayStudentResult(test.TestId);
        }
    }
}
