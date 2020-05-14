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
        
        private static Teacher instance = null;
       
        #endregion

        #region Constructors
        public Teacher ()
        {
            TeacherId = 0;
            Students = new List<Student>();
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
        public int TeacherId { get; set; }
        public List<Student> Students { get; set; }
        #endregion

        #region Methods
        #endregion
    }
}
