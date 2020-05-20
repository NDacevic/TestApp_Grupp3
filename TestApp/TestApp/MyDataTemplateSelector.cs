using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TestApp
{
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
