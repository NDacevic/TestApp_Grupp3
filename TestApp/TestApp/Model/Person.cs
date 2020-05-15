using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Model
{
    public abstract class Person
    {
        #region Constant Fields
        #endregion

        #region Fields

        #endregion

        #region Constructors
        public Person (string firstName, string lastName, string email, string password, int classId)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            ClassId = classId;
        }

        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int ClassId { get; set; }

        #endregion

        #region Methods
        public bool ShouldSerializeId ()
        {
            throw new Exception();
        }
        #endregion
    }
}
