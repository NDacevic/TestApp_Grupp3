using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TestApp.Model;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace TestApp.ViewModel
{
    public class AdminViewModel
    {
        private static AdminViewModel instance = null;

        public ObservableCollection<Test> MyTests { get; set; }
        public ObservableCollection<Question> TestQuestions { get; set; }
        public List<Student> AllStudents { get; set; }
        public ObservableCollection<Person> AllUsers { get; set; }
        public List<Employee> AllEmployees { get; set; }
        public Employee TempEmployee { get; set; }
        public Student TempStudent { get; set; }

        public List<Test> TestList { get; set; }

        public static AdminViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AdminViewModel();
                }
                return instance;
            }
        }

        public AdminViewModel()
        {
            TestList = new List<Test>(); //Storing tests from DB
            MyTests = new ObservableCollection<Test>(); //Display Tests and used for filtering
            TestQuestions = new ObservableCollection<Question>(); //Used to display questions on test
            TempEmployee = new Employee(); //Used when adding a new employee
            AllStudents = new List<Student>(); //Store all students from DB
            AllEmployees = new List<Employee>(); //Store all Employees from DB
            AllUsers = new ObservableCollection<Person>();
            TempStudent = new Student();
            
        }

        public async void DisplayTests()
        {
            TestList = await ApiHelper.Instance.GetAllTests(); //Populating List with Test from DB
            foreach (Test t in TestList)
            {
                if(!MyTests.Contains(t))
                {
                    MyTests.Add(t);
                }
                
            }
        }
        public void FilterTests(string course, int grade)
        {
            foreach (Test subject in TestList)
            {
                if (!MyTests.Contains(subject)) //We check if our filtered list contains Test from original
                {
                    if (course == "" && subject.Grade == grade) //Make sure that it also matches our choosen course
                    {
                        MyTests.Add(subject); //If not, we add it to the list.
                    }
                    else if (grade == 0 && subject.CourseName == course)
                    {
                        MyTests.Add(subject);
                    }
                    else if (subject.Grade == grade && subject.CourseName == course)
                    {
                        MyTests.Add(subject);
                    }
                }
            }
            foreach (Test filterTest in MyTests.ToList()) //This is if our list is populated with all tests
            {
                if (course != "" && filterTest.CourseName != course)
                {
                    MyTests.Remove(filterTest);
                }
                if (grade != 0 && filterTest.Grade != grade)
                {
                    MyTests.Remove(filterTest);
                }
            }
        }
        public void FilterListByCourse(string course)
        {
            foreach (Test subject in TestList)
            {
                if (!MyTests.Contains(subject)) //We check if our filtered list contains Test from original
                {
                    if(course=="")
                    {
                        MyTests.Add(subject); //If not, we add it to the list.
                    }
                    else if(subject.CourseName==course)
                    {
                        MyTests.Add(subject);
                    }
                }
            }

        }
     
    
        public void DeleteTest(int id) 
        {

            ApiHelper.Instance.DeleteTest(id); //Send Test.Id of the test to ApiHelper to delete it from db
            foreach(Test t in MyTests.ToList())
            {
                if(t.TestId==id)
                {
                    MyTests.Remove(t);
                    TestList.Remove(t);
                }
            }
        }
        public void DisplayQuestionsOnTest(Test test) //We go through the choosen Test and displays all the questions.
        {
            TestQuestions.Clear();

            foreach(Question q in test.Questions)
            {
                TestQuestions.Add(q);
            }
           
        }
        public async void DisplayStudents() //Displays all students. DONE
        {
            if(AllStudents.Count==0)
            {
                AllStudents = await ApiHelper.Instance.GetAllStudents();
            }
            foreach(Person p in AllStudents)
            {
                if(!AllUsers.Contains(p))
                AllUsers.Add(p);
            }
        }


        /// <summary>
        /// Overloaded method.
        /// It handles the creating of the patchdoc and sends it to the APIHelper class.
        /// </summary>
        /// <param name="chosenStudent"></param>
        /// <param name="id"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        public async void EditUserInfo(Student chosenStudent, int id, string firstName, string lastName, string email)
        {
            try
            {
                JsonPatchDocument<Person> patchDoc = new JsonPatchDocument<Person>();
                CreatePersonPatchDoc(chosenStudent, firstName, lastName, email, patchDoc);

                ApiHelper.Instance.PatchStudent(id, patchDoc);
            }
            catch (FormatException exc)
            {
                await new MessageDialog(exc.Message).ShowAsync();
            }
        }

        /// <summary>
        /// /// Overloaded method.
        /// It handles the creating of the patchdoc and sends it to the APIHelper class.
        /// </summary>
        /// <param name="chosenEmployee"></param>
        /// <param name="id"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        public async void EditUserInfo(Employee chosenEmployee, int id, string firstName, string lastName, string email)
        {
            try
            {
                JsonPatchDocument<Person> patchDoc = new JsonPatchDocument<Person>();
                CreatePersonPatchDoc(chosenEmployee, firstName, lastName, email, patchDoc);

                ApiHelper.Instance.PatchEmployee(id, patchDoc );
            }
            catch (FormatException exc)
            {
                await new MessageDialog(exc.Message).ShowAsync();
            }
            
        }

        /// <summary>
        /// Used to streamline creation of the patchdoc for updating information on a student or employee
        /// </summary>
        /// <param name="person"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        private void CreatePersonPatchDoc(Person person, string firstName, string lastName, string email, JsonPatchDocument<Person> patchDoc)
        {

            //We check for what is different and add that to the patch doc.
            if (person.FirstName != firstName)
                patchDoc.Replace(x => x.FirstName, firstName);

            if (person.LastName != lastName)
                patchDoc.Replace(x => x.LastName, lastName);

            if (person.Email != email)
                patchDoc.Replace(x => x.Email, email);

            string test = JsonConvert.SerializeObject(patchDoc);

            if (test == "[]")
                throw new FormatException("Inga personuppgifter har ändrats!");

        }

        public void DisplayStudentById(int id) //Displays student by searched Id. DONE
        {
            AllUsers.Clear();
            foreach(Student p in AllStudents.ToList())
            {
                if(p.StudentId==id)
                {
                    AllUsers.Add(p);
                }
            }
        }
        public void DisplayEmployeeById(int id) //Displays employee by searched Id
        {
            AllUsers.Clear();
            foreach(Employee e in AllEmployees.ToList())
            {
                if(e.EmployeeId==id)
                {
                    AllUsers.Add(e);
                }
            }
        }
        public async void DisplayEmployees() //Displays all employees
        {
            if(AllEmployees.Count==0)
            {
                AllEmployees = await ApiHelper.Instance.GetAllEmployees();

            }
            foreach (Person p in AllEmployees)
             {
                if (!AllUsers.Contains(p))
                {
                    AllUsers.Add(p);
                }
            }
        }
       
        public void DeleteEmployee(Employee employee)
        {
            ApiHelper.Instance.DeleteEmployee(employee.EmployeeId);

            foreach (Employee e in AllEmployees.ToList())
            {
                if (e.EmployeeId == employee.EmployeeId)
                {
                    AllEmployees.Remove(e);
                    AllUsers.Remove(e);
                }
            }
        }
        public void DeleteStudent(Student student)
        {
            ApiHelper.Instance.DeleteStudent(student.StudentId);
            foreach (Student s in AllStudents.ToList())
            {
                if (s.StudentId == student.StudentId)
                {
                    AllStudents.Remove(s);
                    AllUsers.Remove(s);
                }
            }
        }



    }
}
