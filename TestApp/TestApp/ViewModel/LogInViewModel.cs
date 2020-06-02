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
        /// <summary>
        /// Get student based on inserted email when logging in
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task GetStudent (string email)
        {
            ActiveStudent = new Student();
            try
            {
                ActiveStudent = await ApiHelper.Instance.GetStudent(email);
            }
            catch (Exception exc)
            {
                await new MessageDialog(exc.Message).ShowAsync();
            }
            
        }
        /// <summary>
        /// Get employee based on inserted email when logging in
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task GetEmployee (string email)
        {
            ActiveEmployee = new Employee();
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
                // Password to byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                //Return encrypted string
                return builder.ToString();
            }
        }
        /// <summary>
        /// Comparing inserted password with the password of the employee trying to log in 
        /// </summary>
        /// <param name="insertedPassword"></param>
        /// <returns></returns>
        public bool CheckEmployeePassword(string insertedPassword)
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
        /// <summary>
        /// Comparing inserted password with the password of the student trying to log in
        /// </summary>
        /// <param name="insertedPassword"></param>
        /// <returns></returns>
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
