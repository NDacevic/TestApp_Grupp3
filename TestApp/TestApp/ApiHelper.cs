﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestApp.Model;
using Windows.UI.Popups;

namespace TestApp
{
    public class ApiHelper
    {
        #region Constant Fields
        #endregion

        #region Fields
        private static ApiHelper instance = null;
        private static readonly object padlock = new object();

        private HttpClient httpClient = new HttpClient();
        private string url;
        private string jsonString;

        #endregion

        #region Constructors
        public ApiHelper()
        {
            //Created but left empty intentionally in case it will be used in the future
        }
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        public static ApiHelper Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance==null)
                    {
                        instance = new ApiHelper();
                    }
                    return instance;
                }
            }
        }
        #endregion

        #region Methods
        public void PostCreatedTest()
        {
            throw new NotImplementedException();
        }

        public void GetTest()
        {
            throw new NotImplementedException();
        }

        public void GetAllTests()
        {
            throw new NotImplementedException();
        }

        public void DeleteTest()
        {
            throw new NotImplementedException();
        }

        public async void PostCreatedQuestion(Question question)
        {
            jsonString = JsonConvert.SerializeObject(question);

            try
            {
                throw new NotImplementedException();
            }
            catch (Exception exc)
            {
                await new MessageDialog(exc.Message).ShowAsync();
            }
        }

        public void GetQuestion()
        {
            throw new NotImplementedException();
        }

        public void GetAllQuestions()
        {
            throw new NotImplementedException();
        }

        public void DeleteQuestion()
        {
            throw new NotImplementedException();
        }

        public void PostTestResult()
        {
            throw new NotImplementedException();
        }

        public void GetTestResult()
        {
            throw new NotImplementedException();
        }

        public void GetAllTestResults()
        {
            throw new NotImplementedException();
        }

        public void PostStudent()
        {
            throw new NotImplementedException();
        }

        public void GetStudent()
        {
            throw new NotImplementedException();
        }

        public void GetAllStudents()
        {
            throw new NotImplementedException();
        }

        public void PostEmployee()
        {
            throw new NotImplementedException();
        }

        public void GetEmployee()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
