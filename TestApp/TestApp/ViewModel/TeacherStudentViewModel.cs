﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Model;

namespace TestApp.ViewModel
{
    public class TeacherStudentViewModel
    {
        #region Constant Fields
        #endregion

        #region Fields
        private static TeacherStudentViewModel instance = null;
        private static readonly object padlock = new object();
        #endregion

        #region Constructors
        public TeacherStudentViewModel()
        {
            AllTests = new ObservableCollection<Test>();
            StudentTestResults = new ObservableCollection<TestResult>();
        }
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        public static TeacherStudentViewModel Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance==null)
                    {
                        instance = new TeacherStudentViewModel();
                    }
                    return instance;
                }
            }
        }
        public ObservableCollection<Test> AllTests { get; set; }
        public ObservableCollection<TestResult> StudentTestResults { get; set; }
        #endregion

        #region Methods
        public void GetStudents()
        {
            throw new NotImplementedException();
        }
        public async void DisplayAllTests()
        {
            try
            {
                var tests = await ApiHelper.Instance.GetAllTests();

                foreach (Test t in tests)
                {
                    AllTests.Add(t);
                }
            }
            catch
            {

            }
        }

        public async void DisplayStudentResult(int testId)
        {
            try
            {
                var testResults = await ApiHelper.Instance.GetAllTestResults(testId);
                foreach (TestResult ts in testResults)
                {
                    StudentTestResults.Add(ts);
                }
            }
            catch
            {

            }
        }
        #endregion

    }
}
