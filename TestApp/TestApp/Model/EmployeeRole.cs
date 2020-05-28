using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Model
{
    public class EmployeeRole
    {
        #region Constant Fields
        #endregion

        #region Fields
        #endregion

        #region Constructors
        public EmployeeRole()
        {

        }
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        #endregion

        #region Methods
        #endregion
    }
}
