using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class GradeTestView : Page
    {
        TeacherGradeTestViewModel gradeInstance = TeacherGradeTestViewModel.Instance;
        Model.Employee teacherInstance = Model.Employee.Instance;
        ObservableCollection<Test> ungradedTests = new ObservableCollection<Test>();

        public GradeTestView()
        {
            this.InitializeComponent();

            GetTests();
        }

        private async void GetTests()
        {
            List<Test> tempTests = await gradeInstance.GetUngradedTests();

            foreach (var x in tempTests)
                ungradedTests.Add(x);
        }
    }
}
