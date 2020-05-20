using System;
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
        public ObservableCollection<Test> myTests { get; set; }

        public List<Test> testList { get; set; } //Transfer to AdminViewModel

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
            testList = new List<Test>();
            myTests = new ObservableCollection<Test>();
        }
        public async void DisplayTests()//Transfer this to AdminViewModel?
        {
            testList = await ApiHelper.Instance.GetAllTests(); //Populating List with Test from DB
            foreach (Test t in testList)
            {
                if(!myTests.Contains(t))
                {
                    myTests.Add(t);
                }
                
            }
        }
        public void FilterTests(string course, int grade)
        {
            foreach (Test subject in testList)
            {
                if (!myTests.Contains(subject)) //We check if our filtered list contains Test from original
                {
                    if (course == "" && subject.Grade == grade) //Make sure that it also matches our choosen course
                    {
                        myTests.Add(subject); //If not, we add it to the list.
                    }
                    else if (grade == 0 && subject.CourseName == course)
                    {
                        myTests.Add(subject);
                    }
                    else if (subject.Grade == grade && subject.CourseName == course)
                    {
                        myTests.Add(subject);
                    }
                }
            }
            foreach (Test filterTest in myTests.ToList()) //This is if our list is populated with all tests
            {
                if (course != "" && filterTest.CourseName != course)
                {
                    myTests.Remove(filterTest);
                }
                if (grade != 0 && filterTest.Grade != grade)
                {
                    myTests.Remove(filterTest);
                }
            }
        }
        public void FilterListByCourse(string course)
        {
            foreach (Test subject in testList)
            {
                if (!myTests.Contains(subject)) //We check if our filtered list contains Test from original
                {
                    if (subject.CourseName == course) //Make sure that it also matches our choosen course
                    {
                        myTests.Add(subject); //If not, we add it to the list.

                    }
                }
            }

        }
        public void DeleteTest(int id)
        {
            ApiHelper.Instance.DeleteTest(id);
            DisplayTests();
        }
    }
}
