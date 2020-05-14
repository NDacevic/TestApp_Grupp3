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
        private string FirstName { get; set; }
        private string LastName { get; set; }
        private string Email { get; set; }
        private string Password { get; set; }
        private int ClassId { get; set; }
        #endregion

        #region Constructors
        public Person ()
        {
            
        }
       
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        
        #endregion

        #region Methods
        public bool ShouldSerializeId ()
        {
            throw new Exception();
        }
        #endregion
    }
}
