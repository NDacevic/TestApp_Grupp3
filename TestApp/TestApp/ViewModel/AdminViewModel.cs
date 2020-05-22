﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Model;

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

        public List<Test> testList { get; set; }

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
            testList = new List<Test>(); //Storing tests from DB
            MyTests = new ObservableCollection<Test>(); //Display Tests and used for filtering
            TestQuestions = new ObservableCollection<Question>(); //Used to display questions on test
            TempEmployee = new Employee();
            AllStudents = new List<Student>(); //Store all students from DB
            AllEmployees = new List<Employee>(); //Store all Employees from DB
            AllUsers = new ObservableCollection<Person>();
            FillList();
        }
        public async void FillList()
        {
            AllStudents = await ApiHelper.Instance.GetAllStudents();
        }
        public async void DisplayTests()
        {
            testList = await ApiHelper.Instance.GetAllTests(); //Populating List with Test from DB
            foreach (Test t in testList)
            {
                if(!MyTests.Contains(t))
                {
                    MyTests.Add(t);
                }
                
            }
        }
        public void FilterTests(string course, int grade)
        {
            foreach (Test subject in testList)
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
            foreach (Test subject in testList)
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
        }
        public void DisplayQuestionsOnTest(Test test) //We go through the choosen Test and displays all the questions.
        {
            TestQuestions.Clear();

            foreach(Question q in test.Questions)
            {
                TestQuestions.Add(q);
            }
           
        }
        public void DisplayStudents() //Displays all students. DONE
        {
            foreach(Person p in AllStudents)
            {
                if(!AllUsers.Contains(p))
                AllUsers.Add(p);
            }
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
        public void DisplayEmployeeById(int id) //Displays employee by searched Id ----UNDER CONSTRUCTION
        {
            foreach(Employee e in AllUsers.ToList())
            {
                if(e.EmployeeId!=id)
                {
                    AllUsers.Remove(e);
                }
            }
        }
        public async void DisplayEmployees() //Displays all employees----UNDER CONSTRUCTION
        {
            //CALL GETALLEMPLOYEES from apihelper
        }


    }
}
