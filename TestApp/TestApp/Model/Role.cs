using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Model
{
    public class Role
    {
        #region Constant Fields
        #endregion

        #region Fields
        #endregion

        #region Constructors
        public Role (int roleId, string roleName)
        {
            RoleId = roleId;
            RoleName = roleName;
        }
        public Role ()
        {

        }
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Tells the Json converter that it shouldn't use the ID property when serializing.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeRoleId() => false;
        #endregion
    }
}
