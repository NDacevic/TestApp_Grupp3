using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Model
{
    public class Employee : Person
    {
        #region Constant Fields
        #endregion

        #region Fields
        
        private static Employee instance = null;
       
        #endregion

        #region Constructors
        public Employee (int EmployeeId, string firstName, string lastName, string email, string password) :base(firstName, lastName, email, password)
        {
            EmployeId = EmployeeId;
            Students = new List<Student>();
        }

        public Employee() : base()
        {
            //Used for Singleton
        }
        
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
 
        public static Employee Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Employee();
                }
                return instance;
            }
        }
        public int EmployeId { get; set; }
        public List<Student> Students { get; set; }
        #endregion

        #region Methods
        #endregion
    }
}
