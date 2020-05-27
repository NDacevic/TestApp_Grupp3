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
        public Employee ActiveEmployee { get; set
                ; }
        public Student ActiveStudent { get; set; }
        #endregion

        #region Methods
        public async void GetStudent (string email)
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
        public async void GetEmployee (string email)
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
        public async string GetEmployeeRole ()
        {
            try
            {
                string role = await ApiHelper.Instance.GetEmployeeRole(ActiveEmployee.EmployeeId);
                return role;
            }
            catch
            {
                await new MessageDialog("Felaktig inmatning, försök igen alternativt kontakta admin").ShowAsync();
            }
        }
        /// <summary>
        /// Encrypting the password using SHA256
        /// </summary>
        /// <param name="input"></param>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        //public string EncryptedPassword(string inputPassword, HashAlgorithm algorithm)
        //{
        //    Byte[] passwordToBytes = Encoding.UTF8.GetBytes(inputPassword);

        //    Byte[] hashedPasswordBytes = algorithm.ComputeHash(passwordToBytes);

        //    return BitConverter.ToString(hashedPasswordBytes);
        //}
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
