using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Model
{
    public class Teacher : Person
    {
        #region Constant Fields
        #endregion

        #region Fields
        
        private static Teacher instance = null;
       
        #endregion

        #region Constructors
        public Teacher (int teacherId, string firstName, string lastName, string email, string password) :base(firstName, lastName, email, password)
        {
            TeacherId = teacherId;
            Students = new List<Student>();
        }

        public Teacher() : base()
        {
            //Used for Singleton
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
