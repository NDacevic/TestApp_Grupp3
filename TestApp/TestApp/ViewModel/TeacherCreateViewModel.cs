using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Model;
using Windows.UI.Popups;

namespace TestApp.ViewModel
{
    public class TeacherCreateViewModel
    {
        #region Constant Fields
        #endregion

        #region Fields
        private static TeacherCreateViewModel instance = null;
        private static readonly object padlock = new object();
        #endregion

        #region Constructors
        public TeacherCreateViewModel()
        {
            //Created but left empty intentionally in case it will be used in the future
        }
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        public static TeacherCreateViewModel Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance==null)
                    {
                        instance = new TeacherCreateViewModel();
                    }
                    return instance;
                }
            }
        }
        public Test CreatedTest { get; set; }
        public Question CreatedQuestion { get; set; }
        public List<Question> SubjectQuestions { get; set; }

        #endregion

        #region Methods
        public void CreateTest()
        {
            throw new NotImplementedException();
        }

        public async void CreateQuestion()
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception exc)
            {
                await new MessageDialog(exc.Message).ShowAsync();
            }
        }

        public void GetQuestionsForTestCreation()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
