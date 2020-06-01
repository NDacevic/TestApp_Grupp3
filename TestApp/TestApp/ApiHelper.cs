using Microsoft.AspNetCore.JsonPatch;
using Microsoft.VisualBasic.CompilerServices;
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
        /// <summary>
        /// Converts the test object and sends it to our API.
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        public async Task PostCreatedTestAsync(Test test)
        {
            try
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
                        throw new HttpRequestException();
                    }
                }
            }
            catch(Exception exc)
            {
                BasicNoConnectionMessage(exc);
            }        
        }

        /// <summary>
        /// Gets Json string from API and converts it to List of Test objects
        /// </summary>
        /// <returns></returns>
        public async Task<List<Test>> GetAllTests()
        {
            try
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
                    throw new HttpRequestException();
                }
            }
            catch(Exception exc)
            {
                BasicNoConnectionMessage(exc);
                return new List<Test>();
            }
        }

        /// <summary>
        /// Sends the test ID to the API for deletion
        /// </summary>
        /// <param name="id"></param>
        public async void DeleteTest(int id)
        {
            try
            {
                HttpResponseMessage response = await httpClient.DeleteAsync($"Tests/{id}");

                if (response.IsSuccessStatusCode)
                {
                    await new MessageDialog("Provet har raderats").ShowAsync();

                }
                else
                {
                    Debug.WriteLine($"Http Error: {response.StatusCode}. {response.ReasonPhrase}");
                    throw new HttpRequestException();
                }
            }
            catch (Exception exc)
            {
                BasicNoConnectionMessage(exc);
            }
        }
        /// <summary>
        /// Sends the employee ID to the API for deletion
        /// </summary>
        /// <param name="id"></param>
        public async void DeleteEmployee(int id)
        {
            try
            {
                HttpResponseMessage response = await httpClient.DeleteAsync($"Employees/{id}");

                if (response.IsSuccessStatusCode)
                {
                    await new MessageDialog("Den anställde har raderats").ShowAsync();
                }
                else
                {
                    Debug.WriteLine($"Http Error: {response.StatusCode}. {response.ReasonPhrase}");
                    throw new HttpRequestException();
                }
            }
            catch (Exception exc)
            {
                BasicNoConnectionMessage(exc);
            }
        }
        /// <summary>
        /// Sends the student ID to the API for deletion
        /// </summary>
        /// <param name="id"></param>
        public async void DeleteStudent(int id)
        {
            try
            {
                HttpResponseMessage response = await httpClient.DeleteAsync($"Students/{id}");

                if (response.IsSuccessStatusCode)
                {
                    await new MessageDialog("Eleven har raderats").ShowAsync();
                }
                else
                {
                    Debug.WriteLine($"Http Error: {response.StatusCode}. {response.ReasonPhrase}");
                    throw new HttpRequestException("");
                }
            }
            catch (Exception exc)
            {
                BasicNoConnectionMessage(exc);
            }
        }

        /// <summary>
        /// Converts the question object to a Json string and posts it to the API for writing into the database
        /// </summary>
        /// <param name="question"></param>
        public async Task<bool> PostCreatedQuestion(Question question)
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
                        await new MessageDialog("Frågan är sparad").ShowAsync();
                        return true;
                    }
                    else
                    {
                        Debug.WriteLine($"Http Error: {response.StatusCode}. {response.ReasonPhrase}");
                        throw new HttpRequestException("Frågan blev ej sparad. Kontakta administratör");
                    }
                }
                
            }
            catch (Exception exc)
            {
                BasicNoConnectionMessage(exc);
                return false;
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

                //construct the request
                var request = new HttpRequestMessage(method, new Uri(httpClient.BaseAddress, $"tests/{id}"))
                {
                    Content = content
                };

                using (HttpResponseMessage response = await httpClient.SendAsync(request))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        Debug.Write("Provet är uppdaterat");
                    }
                    else
                    {
                        throw new HttpRequestException($"PutTest Status: {response.StatusCode}, {response.ReasonPhrase}");
                    }
                }
            }
            catch (Exception exc)
            {
                BasicNoConnectionMessage(exc);
            }
        }

        /// <summary>
        /// Get´s a list of questions back from API depending on our course.
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public async Task<ObservableCollection<Question>> GetQuestion(string course)
        {
            try
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
            catch (Exception exc)
            {
                BasicNoConnectionMessage(exc);
                return new ObservableCollection<Question>();
            }
        }

        /// <summary>
        /// Sends a question ID to API for deletion.
        /// </summary>
        /// <param name="id"></param>
        public async void DeleteQuestion(int id)
        {
            try
            {
                HttpResponseMessage response = await httpClient.DeleteAsync($"Questions/{id}");

                if (response.IsSuccessStatusCode)
                {
                    await new MessageDialog("Frågan har raderats").ShowAsync();
                }
                else
                {
                    Debug.WriteLine($"Http Error: {response.StatusCode}. {response.ReasonPhrase}");
                    throw new HttpRequestException("Ett fel har uppstått, kontakta administratör");
                }
            }
            catch (Exception exc)
            {
                BasicNoConnectionMessage(exc);
            }
        }

        /// <summary>
        /// Contacts the API and writes a specific test result to the database
        /// </summary>
        /// <param name="testResult"></param>
        public async void PostTestResult(TestResult testResult)
        {
            try
            {
                //Convert the object to a json string.
                jsonString = JsonConvert.SerializeObject(testResult);

                //Set this part of the code into a scope so we don't have to worry about it not getting disposed.
                using (HttpContent content = new StringContent(jsonString))
                {
                    //Set the type of content
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    //Call the api and send the Json string.
                    HttpResponseMessage response = await httpClient.PostAsync("TestResults", content);

                    //Check if it is successfull. In that case display a message telling the user.
                    //Otherwise throw an error and tell the user that the question was not posted.
                    if (response.IsSuccessStatusCode)
                    {
                        await new MessageDialog("Resultatet för testet har sparats").ShowAsync();
                    }
                    else
                    {
                        Debug.WriteLine($"Http Error: {response.StatusCode}. {response.ReasonPhrase}");
                        throw new HttpRequestException("Ett fel har uppstått, kontakta administratör");
                    }
                }
            }
            catch (Exception exc)
            {
                BasicNoConnectionMessage(exc);
            }
        }

        /// <summary>
        /// Contacts the API and gets all student's answers to all questions
        /// </summary>
        /// <returns></returns>
        public async Task<List<StudentQuestionAnswer>> GetAllStudentQuestionAnswers()
        {
            try
            {
                //Get jsonString from API. Contacts correct API address using the httpClient's BaseAddress + "string"
                HttpResponseMessage response = await httpClient.GetAsync("StudentQuestionAnswers");

                if (response.IsSuccessStatusCode)
                {
                    jsonString = response.Content.ReadAsStringAsync().Result;
                    //Convert jsonString to list of SQA objects
                    var studentQuestionAnswers = JsonConvert.DeserializeObject<List<StudentQuestionAnswer>>(jsonString);
                    return studentQuestionAnswers;
                }
                else
                {
                    throw new HttpRequestException("No test results retrieved from database. Contact an admin for help.");
                }
            }
            catch (Exception exc)
            {
                BasicNoConnectionMessage(exc);
                return new List<StudentQuestionAnswer>();
            }
        }

        /// <summary>
        /// Contacts the API and writes a all answers to all questions on a specific test for a specific student
        /// </summary>
        /// <param name="questionAnswers"></param>
        public async void PostQuestionAnswers(List<StudentQuestionAnswer> questionAnswers)
        {
            try
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
                        await new MessageDialog("Resultatet för varje individuell fråga har sparats").ShowAsync();
                    }
                    else
                    {
                        Debug.WriteLine($"Http Error: {response.StatusCode}. {response.ReasonPhrase}");
                        throw new HttpRequestException("Ett fel har uppstått, kontakta administratör");
                    }
                }
            }
            catch (Exception exc)
            {
                BasicNoConnectionMessage(exc);
            }
        }

        /// <summary>
        /// Gets the test results for all students taking one test (filtered by TestId)
        /// </summary>
        /// <returns>List<TestResult></TestResult></returns>
        public async Task<List<TestResult>> GetAllTestResults(int testId)
        {
            try
            {
                jsonString = await httpClient.GetStringAsync("TestResults/" + testId);
                var testResults = JsonConvert.DeserializeObject<List<TestResult>>(jsonString);
                return testResults;
            }
            catch (Exception exc)
            {
                BasicNoConnectionMessage(exc);
                return new List<TestResult>();
            }

        }

        /// <summary>
        /// Todo: Comments!
        /// </summary>
        /// <param name="student"></param>
        public async void PostStudent(Student student)
        {
            try
            {
                jsonString = JsonConvert.SerializeObject(student);
                HttpContent httpContent = new StringContent(jsonString);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage respons = await httpClient.PostAsync("students", httpContent);

                //Check if succesfull
                if (respons.IsSuccessStatusCode)
                {
                    await new MessageDialog("Student tillagd").ShowAsync();
                }
                else
                {
                    Debug.WriteLine($"Http Error: {respons.StatusCode}. {respons.ReasonPhrase}");
                    throw new HttpRequestException("Kunde inte sparas, vänlig felsök alternativt försök igen.");
                }
            }
            catch (Exception exc)
            {
                BasicNoConnectionMessage(exc);
            }
        }
        
        /// <summary>
        /// Todo: Comments!
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Student> GetStudent(string email)
        {
            try
            {
                jsonString = await httpClient.GetStringAsync("LogInStudents/" + email);
                var student = JsonConvert.DeserializeObject<Student>(jsonString);
                return student;
            }
            catch (Exception exc)
            {
                BasicNoConnectionMessage(exc);
                return new Student();
            }
        }

        /// <summary>
        /// Calls API and get a list of students in return
        /// </summary>
        /// <returns></returns>
        public async Task <List<Student>> GetAllStudents()
        {
            List<Student> studentList = new List<Student>();
            try
            {
                using (HttpResponseMessage response = await httpClient.GetAsync("Students"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        jsonString = await response.Content.ReadAsStringAsync();

                        studentList = JsonConvert.DeserializeObject<List<Student>>(jsonString);
                        return studentList;
                    }
                    else
                        throw new HttpRequestException("Ingen uppkoppling till servern. Kontakta administratör");
                }
            }
            catch (Exception exc)
            {
                BasicNoConnectionMessage(exc);
                return new List<Student>();
            }
        }

        /// <summary>
        /// Todo: Comments!
        /// </summary>
        /// <returns></returns>
        public async Task<List<Student>> GetAllStudentsTestsQuestions()
        {            
            try
            {
                List<Student> studentList = new List<Student>();
                using (HttpResponseMessage response = await httpClient.GetAsync("FullStudentsTestsQuestions"))
                 {
                    if (response.IsSuccessStatusCode)
                    {
                        jsonString = await response.Content.ReadAsStringAsync();

                        studentList = JsonConvert.DeserializeObject<List<Student>>(jsonString);
                        return studentList;
                    }
                    else
                        throw new HttpRequestException("Ingen uppkoppling till servern. Kontakta administratör");
                }
            }
            catch (Exception exc)
            {
                BasicNoConnectionMessage(exc);
                return new List<Student>();
            }
        }
        
        /// <summary>
        /// Post new employee created by Admin 
        /// </summary>
        /// <param name="employee"></param>
        public async void PostEmployee(Employee employee)
        {
            try
            {
                jsonString = JsonConvert.SerializeObject(employee);
                HttpContent httpContent = new StringContent(jsonString);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage respons = await httpClient.PostAsync("employees", httpContent);

                //Check if succesfull
                if (respons.IsSuccessStatusCode)
                {
                    await new MessageDialog("Personal tillagd").ShowAsync();
                }
                else
                {
                    Debug.WriteLine($"Http Error: {respons.StatusCode}. {respons.ReasonPhrase}");
                    throw new HttpRequestException("Kunde inte sparas, vänlig felsök alternativt försök igen.");
                }
            }
            catch (Exception exc)
            {
                BasicNoConnectionMessage(exc);
            }
        }
        /// <summary>
        /// Todo: Comments!
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDocStudent"></param>
        /// <returns></returns>
        public async Task<bool> PatchStudentAsync(int id, JsonPatchDocument<Person> patchDocStudent)
        {
            //httpClient.PatchAsync doesn't exist as a predefined method so we have to use SendAsync() which requires a HttpRequestMessage as a parameter
            try
            {
                //define method as PATCH
                HttpMethod method = new HttpMethod("PATCH");

                //Make the json
                jsonString = JsonConvert.SerializeObject(patchDocStudent);

                //Configure the request by inputting the request method and the url.
                HttpRequestMessage request = new HttpRequestMessage(method, new Uri(httpClient.BaseAddress, $"students/{id}"));

                //Set the jsonString as the content. 
                HttpContent content = new StringContent(jsonString);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                request.Content = content;

                //Send it off to the API and wait for the response
                using (HttpResponseMessage response = await httpClient.SendAsync(request))
                {
                    if (response.IsSuccessStatusCode)
                        return true;
                    else
                        throw new HttpRequestException($"Status: {response.StatusCode}, {response.ReasonPhrase}");
                }
            }
            catch (Exception exc)
            {
                BasicNoConnectionMessage(exc);
                return false;
            }
        }
        /// <summary>
        /// Todo: Comments!
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDocEmployee"></param>
        /// <returns></returns>
        public async Task<bool> PatchEmployeeAsync(int id, JsonPatchDocument<Person> patchDocEmployee)
        {
            //httpClient.PatchAsync doesn't exist as a predefined method so we have to use SendAsync() which requires a HttpRequestMessage as a parameter
            try
            {
                //define method as PATCH
                HttpMethod method = new HttpMethod("PATCH");

                //Make the json
                jsonString = JsonConvert.SerializeObject(patchDocEmployee);

                //Configure the request by inputting the request method and the url.
                HttpRequestMessage request = new HttpRequestMessage(method, new Uri(httpClient.BaseAddress, $"employees/{id}"));

                //Set the jsonString as the content. 
                HttpContent content = new StringContent(jsonString);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                request.Content = content;

                //Send it off to the API and wait for the response
                using (HttpResponseMessage response = await httpClient.SendAsync(request))
                {
                    if (response.IsSuccessStatusCode)
                        return true;
                    else
                        throw new HttpRequestException($"Status: {response.StatusCode}, {response.ReasonPhrase}");
                }
            }
            catch (Exception exc)
            {
                BasicNoConnectionMessage(exc);
                return false;
            }
        }
        /// <summary>
        /// Todo: Comments!
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Employee> GetEmployee(string email)
        {
            try
            {
                HttpResponseMessage httpResponse = await httpClient.GetAsync("LogInEmployees/" + email);
                jsonString = await httpResponse.Content.ReadAsStringAsync();
                var employee = JsonConvert.DeserializeObject<Employee>(jsonString);
                return employee;
            }
            catch (Exception exc)
            {
                BasicNoConnectionMessage(exc);
                return new Employee();
            }
        }

        /// <summary>
        /// Calls API and get´s a list of employees in return
        /// </summary>
        /// <returns></returns>
        public async Task<List<Employee>> GetAllEmployees()
        {
            try
            {
                using (HttpResponseMessage response = await httpClient.GetAsync("Employees"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        jsonString = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<List<Employee>>(jsonString);
                    }
                    else
                        throw new HttpRequestException("");
                }
            }
            catch (Exception exc)
            {
                BasicNoConnectionMessage(exc);
                return new List<Employee>();
            }
        }

        /// <summary>
        /// Contacts API and get´s alist of TestResults in return
        /// </summary>
        /// <returns></returns>
        public async Task<ObservableCollection<TestResult>> GetTestResults() 
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync("TestResults");
                if (response.IsSuccessStatusCode)
                {
                    jsonString = response.Content.ReadAsStringAsync().Result;
                    //Convert jsonString to list of courses objects
                    var testResult = JsonConvert.DeserializeObject<ObservableCollection<TestResult>>(jsonString);
                    return testResult;
                }
                else
                {
                    throw new HttpRequestException("No results retrieved from database. Contact an admin for help.");
                }
            }
            catch (Exception exc)
            {
                BasicNoConnectionMessage(exc);
                return new ObservableCollection<TestResult>();
            }
        }

        /// <summary>
        /// Calls API and get´s a list of Course in return
        /// </summary>
        /// <returns></returns>
        public async Task<List<Course>> GetAllCourses()
        {
            try
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
            catch (Exception exc)
            {
                BasicNoConnectionMessage(exc);
                return new List<Course>();
            }
        }

        /// <summary>
        /// Takes a list of StudentQuestionAnswer objects and sends them off to the API.
        /// The list must have at least one object in it
        /// </summary>
        /// <param name=""></param>
        public async Task<bool> UpdateStudentQuestionAnswer(List<StudentQuestionAnswer> sqaList)
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
                        return true;
                    }
                    else
                    {
                        throw new HttpRequestException($"Code: {response.StatusCode}, {response.ReasonPhrase}");
                    }
                }
            }
            catch(Exception exc)
            {
                BasicNoConnectionMessage(exc);
                return false;
            }
        }

        /// <summary>
        /// Gets all roles from DB
        /// </summary>
        /// <returns></returns>
        public async Task<List<Role>> GetRoles ()
        {
            try
            {
                jsonString = await httpClient.GetStringAsync("Roles");
                var roles = JsonConvert.DeserializeObject<List<Role>>(jsonString);
                return roles;
            }
            catch (Exception exc)
            {
                BasicNoConnectionMessage(exc);
                return new List<Role>();
            }
        }

        /// <summary>
        /// Todo: Comments!
        /// </summary>
        /// <param name="exc"></param>
        private async void BasicNoConnectionMessage(Exception exc)
        {
            Debug.WriteLine(exc.Message);
            await new MessageDialog("Ingen kontakt med servern. Kontakta admin").ShowAsync();
        }
        #endregion
    }
}
