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
        //MCA = MultipleChoiceAnswer. Multiple MCA properties to allow the radiobuttons to end up on different positions
        public DataTemplate MCA1 { get; set; }
        public DataTemplate MCA2 { get; set; }
        public DataTemplate MCA3 { get; set; }
        public DataTemplate MCA4 { get; set; }
        public DataTemplate MCA5 { get; set; }
        public DataTemplate MCA6 { get; set; }
        public DataTemplate TextAnswer { get; set; }

        /// <summary>
        /// Method that returns the correct DataTemplate according to specific conditions
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override DataTemplate SelectTemplateCore(object item)
        {
            Random r = new Random();

            if (((Question)item).QuestionType == "Flerval")
            {
                switch (r.Next(1, 7))
                {
                    case 1:
                        return MCA1;
                    case 2:
                        return MCA2;
                    case 3:
                        return MCA3;
                    case 4:
                        return MCA4;
                    case 5:
                        return MCA5;
                    case 6:
                        return MCA6;
                    default:
                        return null; //Can never happen
                }
            }
            else
            {
                return TextAnswer;
            }
        }
    }
}
