﻿using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using TestApp.Model;
using Windows.ApplicationModel.Appointments.DataProvider;
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
                    await new MessageDialog("Provet har sparats").ShowAsync();
                }
                else
                {
                    Debug.WriteLine($"Http Error: {response.StatusCode}. {response.ReasonPhrase}");
                    throw new HttpRequestException("Ett fel har uppstått, kontakta administratör");
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

        public async void DeleteTest(int id)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync($"Tests/{id}");

            if (response.IsSuccessStatusCode)
            {
                await new MessageDialog("Provet har raderats").ShowAsync();
                
            }
            else
            {
                Debug.WriteLine($"Http Error: {response.StatusCode}. {response.ReasonPhrase}");
                throw new HttpRequestException("Ett fel har uppstått, kontakta administratör");
            }
        }
        public async void DeleteEmployee(int id)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync($"Employees/{id}");

            if (response.IsSuccessStatusCode)
            {
                await new MessageDialog("Den anställde har raderats").ShowAsync();
            }
            else
            {
                Debug.WriteLine($"Http Error: {response.StatusCode}. {response.ReasonPhrase}");
                throw new HttpRequestException("Ett fel har uppstått, kontakta administratör");
            }
        }
        public async void DeleteStudent(int id)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync($"Students/{id}");

            if (response.IsSuccessStatusCode)
            {
                await new MessageDialog("Eleven har raderats").ShowAsync();
            }
            else
            {
                Debug.WriteLine($"Http Error: {response.StatusCode}. {response.ReasonPhrase}");
                throw new HttpRequestException("Ett fel har uppstått, kontakta administratör");
            }
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


        /// <summary>
        /// Sends a JsonPatchDocument to the API with updated information for a Test object
        /// </summary>
        /// <param name="id"></param>
        /// <param name="jsonPatchTest"></param>
        public async void PatchTest(int id, JsonPatchDocument<Test> jsonPatchTest)
        {
            try
            {
                //httpClient.PatchAsync doesn't exist as a predefined method so we have to use SendAsync() which requires a HttpRequestMessage as a parameter
                
                //Define the method as a PATCH
                HttpMethod method = new HttpMethod("PATCH");
                //Serialize the JsonPatchDocument
                jsonString = JsonConvert.SerializeObject(jsonPatchTest);
                //Set the json as the content.
                HttpContent content = new StringContent(jsonString);
                //Specify that the content is a Json string
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                //constructn the request
                var request = new HttpRequestMessage(method, new Uri(httpClient.BaseAddress, $"tests/{id}"))
                {
                    Content = content
                };

                using (HttpResponseMessage response = await httpClient.SendAsync(request))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        Debug.Write("Test updated");
                    }
                    else
                    {
                        throw new HttpRequestException($"PutTest Status: {response.StatusCode}, {response.ReasonPhrase}");
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
        }

        public async Task<ObservableCollection<Question>> GetQuestion(string course)
        {
            //Get jsonString from API. Contacts correct API address using the httpClient's BaseAddress + "string"
            HttpResponseMessage response = await httpClient.GetAsync($"Questions/{course}");

            if (response.IsSuccessStatusCode)
            {
                jsonString = response.Content.ReadAsStringAsync().Result;
                //Convert jsonString to list of question objects
                var question = JsonConvert.DeserializeObject<ObservableCollection<Question>>(jsonString);
                return question;
            }
            else
            {
                throw new HttpRequestException("No questions retrieved from database. Contact an admin for help.");
            }
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

        public async void PostQuestionAnswers(List<StudentQuestionAnswer> questionAnswers)
        {
            //Convert the object to a json string.
            jsonString = JsonConvert.SerializeObject(questionAnswers);

            //Set this part of the code into a scope so we don't have to worry about it not getting disposed.
            using (HttpContent content = new StringContent(jsonString))
            {
                //Set the type of content
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                //Call the api and send the Json string.
                HttpResponseMessage response = await httpClient.PostAsync("StudentQuestionAnswers", content);

                //Check if it is successfull. In that case display a message telling the user.
                //Otherwise throw an error and tell the user that the question was not posted.
                if (response.IsSuccessStatusCode)
                {
                    await new MessageDialog("Resultatet har sparats").ShowAsync();
                }
                else
                {
                    Debug.WriteLine($"Http Error: {response.StatusCode}. {response.ReasonPhrase}");
                    throw new HttpRequestException("Ett fel har uppstått, kontakta administratör");
                }
            }
        }

        public void GetTestResult()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets the test results for all students taking one test (filtered by TestId)
        /// </summary>
        /// <returns>List<TestResult></TestResult></returns>
        public async Task<List<TestResult>> GetAllTestResults(int testId)
        {
            jsonString = await httpClient.GetStringAsync("TestResults/"+ testId);
            var testResults = JsonConvert.DeserializeObject<List<TestResult>>(jsonString);
            return testResults;
        }

        public void PostStudent()
        {
            throw new NotImplementedException();
        }

        public async Task<Student> GetStudent(string email)
        {
            jsonString = await httpClient.GetStringAsync("LogInStudents/" + email);
            var student = JsonConvert.DeserializeObject<Student>(jsonString);
            return student;
        }

        public async Task<List<Student>> GetAllStudents()
        {
            List<Student> studentList = new List<Student>();
            using (HttpResponseMessage response = await httpClient.GetAsync("students"))
            {
                if (response.IsSuccessStatusCode)
                {
                    jsonString = await response.Content.ReadAsStringAsync();

                    studentList = JsonConvert.DeserializeObject<List<Student>>(jsonString);
                }
            }
            return studentList;
        }
        

        public async void PostEmployee(Employee employee)
        {
            jsonString = JsonConvert.SerializeObject(employee);
            HttpContent httpContent = new StringContent(jsonString);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application / json");

            HttpResponseMessage respons = await httpClient.PostAsync("employees", httpContent);

            if (respons.IsSuccessStatusCode)
            {
                await new MessageDialog("Personal tillagd med id:" + employee.EmployeeId).ShowAsync();
            }
            else
            {
                Debug.WriteLine($"Http Error: {respons.StatusCode}. {respons.ReasonPhrase}");
                throw new HttpRequestException("Kunde inte sparas, vänlig felsök alternativt försök igen.");
            }
        }

        public async Task<Employee> GetEmployee(string email)
        {
            jsonString = await httpClient.GetStringAsync("LogInEmployees/" + email);
            var employee = JsonConvert.DeserializeObject<Employee>(jsonString);
            return employee;
        }
       public async Task<List<Employee>>GetAllEmployees()
        {
            using(HttpResponseMessage response = await httpClient.GetAsync("Employees"))
            {
                if (response.IsSuccessStatusCode)
                {
                    jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Employee>>(jsonString);
                }
                else
                  return null;
            }
        }
        
        public async Task<List<Course>> GetAllCourses()
        {
            //Get jsonString from API. Contacts correct API address using the httpClient's BaseAddress
            HttpResponseMessage response = await httpClient.GetAsync("Courses");

            if (response.IsSuccessStatusCode)
            {
                jsonString = response.Content.ReadAsStringAsync().Result;
                //Convert jsonString to list of courses objects
                var courses = JsonConvert.DeserializeObject<List<Course>>(jsonString);
                return courses;
            }
            else
            {
                throw new HttpRequestException("No courses retrieved from database. Contact an admin for help.");
            }
        }

        /// <summary>
        /// Takes a list of StudentQuestionAnswer objects and sends them off to the API.
        /// The list must have at least one object in it
        /// </summary>
        /// <param name=""></param>
        public async void UpdateStudentQuestionAnswer(List<StudentQuestionAnswer> sqaList)
        {
            try
            {
                //Serialize object into a json string
                jsonString = JsonConvert.SerializeObject(sqaList);

                //setup the content
                HttpContent content = new StringContent(jsonString);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                //send it off and display a message indicating success
                using (HttpResponseMessage response = await httpClient.PutAsync("studentquestionanswers", content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        await new MessageDialog("Rättningen sparad!").ShowAsync();
                    }
                    else
                    {
                        throw new HttpRequestException($"Code: {response.StatusCode}, {response.ReasonPhrase}");
                    }
                }
            }
            catch(Exception exc)
            {
                await new MessageDialog(exc.Message).ShowAsync();
            }
        }
        #endregion
    }
}
