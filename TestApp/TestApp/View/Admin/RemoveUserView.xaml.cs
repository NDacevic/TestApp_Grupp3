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

namespace TestApp.View.Admin
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RemoveUserView : Page
    {
        AdminViewModel adminViewModel = AdminViewModel.Instance;
        public RemoveUserView()
        {
            this.InitializeComponent();
        }

        private void EmployeeRadioBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void StudentRadioBtn_Click(object sender, RoutedEventArgs e)
        {
            adminViewModel.DisplayStudents();
        }

        private void SearchIdTxtBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key==Windows.System.VirtualKey.Enter)
            {
                if (StudentRadioBtn.IsChecked == true)
                {
                    if(SearchIdTxtBox.Text=="")
                    {
                        adminViewModel.DisplayStudents();
                    }
                    else
                    adminViewModel.DisplayStudentById(int.Parse(SearchIdTxtBox.Text));
                }
            }
           
        }
    }
}
