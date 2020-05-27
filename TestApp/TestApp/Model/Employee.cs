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
        #endregion

        #region Constructors
        public Employee (int employeeId, string firstName, string lastName, string email, string password) :base(firstName, lastName, email, password)
        {
            EmployeeId = employeeId;
            Students = new List<Student>();
            Role = new Role();
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
        public Role Role { get; set; } 
        public int EmployeeId { get; set; }
        public List<Student> Students { get; set; }
        #endregion

        #region Methods
        #endregion
    }
}
