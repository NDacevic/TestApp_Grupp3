using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TestApp.Model;
using Windows.UI.Popups;

namespace TestApp.ViewModel
{
    public class LogInViewModel
    {
        #region Constant Fields
        #endregion

        #region Fields
        private static LogInViewModel instance = null;

        #endregion

        #region Constructors
        private LogInViewModel ()
        {
            
        }
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        public static LogInViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LogInViewModel();
                }
                return instance;
            }
        }
        public Employee ActiveEmployee { get; set; }
        public Student ActiveStudent { get; set; }
        #endregion

        #region Methods
        public async void GetStudent (string email)
        {
            try
            {
                ActiveStudent = await ApiHelper.Instance.GetStudent(email);
            }
            catch (Exception exc)
            {
                await new MessageDialog(exc.Message).ShowAsync();
            }
            
        }
        public async void GetEmployee (string email)
        {
            try
            {
                ActiveEmployee = await ApiHelper.Instance.GetEmployee(email);
            }
            catch (Exception exc)
            {
                await new MessageDialog(exc.Message).ShowAsync();
            }
        }
        /// <summary>
        /// Encrypting the password using SHA256
        /// </summary>
        /// <param name="input"></param>
        /// <param name="algorithm"></param>
        /// <returns></returns>

        public static string EncryptPassword(string password)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Return byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public bool CheckEmployeePassword(string insertedPassword) //Get ()
        {
            if (ActiveEmployee.Password == insertedPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CheckStudentPassword(string insertedPassword)
        {
            if (ActiveStudent.Password == insertedPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
