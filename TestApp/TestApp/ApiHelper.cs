using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
            httpClient.BaseAddress = new Uri(@"https://localhost:5001/api/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
        public async Task PostCreatedTestAsync(Test test)
        {
            //Convert the object to a json string.
            jsonString = JsonConvert.SerializeObject(test);
            Debug.WriteLine(test);

            //Set this part of the code into a scope so we don't have to worry about it not getting disposed.
            using (HttpContent content = new StringContent(jsonString))
            {
                //Set the type of content
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                //Call the api and send the Json string.
                HttpResponseMessage response = await httpClient.PostAsync("Tests", content);

                //Check if it is successfull. In that case display a message telling the user.
                //Otherwise throw an error and tell the user that the question was not posted.
                if (response.IsSuccessStatusCode)
                {
                    await new MessageDialog("Test saved successfully").ShowAsync();
                }
                else
                {
                    Debug.WriteLine($"Http Error: {response.StatusCode}. {response.ReasonPhrase}");
                    throw new HttpRequestException("Test was not saved. Contact an admin for help");
                }
            }
        
        }

        public void GetTest()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets Json string from API and converts it to List of Test objects
        /// </summary>
        /// <returns></returns>
        public async Task<List<Test>> GetAllTests()
        {
            //Get jsonString from API. Contacts correct API address using the httpClient's BaseAddress + "string"
            HttpResponseMessage response = await httpClient.GetAsync("Tests");

            if (response.IsSuccessStatusCode)
            {
                jsonString = response.Content.ReadAsStringAsync().Result;
                //Convert jsonString to list of Test objects
                var tests = JsonConvert.DeserializeObject<List<Test>>(jsonString);
                return tests;
            }
            else
            {
                throw new HttpRequestException("No tests retrieved from database. Contact an admin for help.");
            }            
        }

        public void DeleteTest()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts the question object to a Json string and posts it to the API for writing into the database
        /// </summary>
        /// <param name="question"></param>
        public async void PostCreatedQuestion(Question question)
        {
            try
            {
                //Convert the object to a json string.
                jsonString = JsonConvert.SerializeObject(question);
                Debug.WriteLine(jsonString);
                //Set this part of the code into a scope so we don't have to worry about it not getting disposed.
                using (HttpContent content = new StringContent(jsonString))
                {
                    //Set the type of content
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    //Call the api and send the Json string.
                    HttpResponseMessage response = await httpClient.PostAsync("questions", content);

                    //Check if it is successfull. In that case display a message telling the user.
                    //Otherwise throw an error and tell the user that the question was not posted.
                    if (response.IsSuccessStatusCode)
                    {
                        await new MessageDialog("Question saved successfully").ShowAsync();
                    }
                    else
                    {
                        Debug.WriteLine($"Http Error: {response.StatusCode}. {response.ReasonPhrase}");
                        throw new HttpRequestException("Question was not saved. Contact an admin for help");
                    }
                }
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
