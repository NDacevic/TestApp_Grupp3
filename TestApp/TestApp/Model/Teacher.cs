using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Model
{
    public class Teacher
    {
        #region Constant Fields
        #endregion

        #region Fields
        private int TeacherId { get; set; }
        private static Teacher instance = null;
        private List<Student> Students { get; set; }
        #endregion

        #region Constructors
        public Teacher ()
        {

        }
        
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
 
        public static Teacher Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Teacher();
                }
                return instance;
            }
        }

        #endregion

        #region Methods
        #endregion
    }
}
