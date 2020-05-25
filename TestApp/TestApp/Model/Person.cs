using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Model
{
    public abstract class Person : INotifyPropertyChanged
    {
        #region Constant Fields
        #endregion

        #region Fields
        private string firstName;
        private string lastName;
        private string email;
        private string password;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructors
        public Person (string firstName, string lastName, string email, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
        }

        public Person()
        {

        }

        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        public string FirstName { get => firstName; set { firstName = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FirstName")); } }
        public string LastName { get => lastName; set { lastName = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LastName")); } }
        public string Email { get => email; set { email = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Email")); } }
        public string Password { get => password; set { password = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Password")); } }


        #endregion

        #region Methods
        public bool ShouldSerializeId ()
        {
            throw new Exception();
        }
        #endregion
    }
}
