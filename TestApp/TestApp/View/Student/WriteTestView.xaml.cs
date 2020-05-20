using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace TestApp.View.Student
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WriteTestView : Page
    {
        #region Constant Fields
        #endregion

        #region Fields
        private Test selectedTest; //For storing the Test object received from the previous page
        #endregion

        #region Constructors
        public WriteTestView()
        {
            this.InitializeComponent();
        }
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        #endregion

        #region Methods
        /// <summary>
        /// Executes when this page is navigated to
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            selectedTest = (Test)e.Parameter;
        }
        #endregion
    }

    /// <summary>
    /// Class used to declare several different DataTemplates
    /// </summary>
    public class MyDataTemplateSelector : DataTemplateSelector
    {
        //One property per DataTemplate
        public DataTemplate MultipleChoiceAnswer { get; set; }
        public DataTemplate TextAnswer { get; set; }

        /// <summary>
        /// Method that returns the correct DataTemplate according to specific conditions
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (((Question)item).QuestionType == "Flerval")
            {
                return MultipleChoiceAnswer;
            }
            else
            {
                return TextAnswer;
            }
        }
    }

}
