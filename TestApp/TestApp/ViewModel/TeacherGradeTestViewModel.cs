﻿using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Model;
using TestApp.View;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TestApp.ViewModel

{
    public class TeacherGradeTestViewModel
    {
        #region Constant Fields
        #endregion

        #region Fields
        private static TeacherGradeTestViewModel instance = null;
        private static readonly object padlock = new object();
        private ContentDialog loadScreen = new LoadDataView();
        #endregion

        #region Constructors
        public TeacherGradeTestViewModel()
        {
            //Created but left empty intentionally in case it will be used in the future
        }
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        public static TeacherGradeTestViewModel Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance==null)
                    {
                        instance = new TeacherGradeTestViewModel();
                    }
                    return instance;
                }
            }
        }

        public List<Student> allStudents { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Calls the GetAllStudents Method in the APIHelper class
        /// </summary>
        /// <returns></returns>
        public async Task DownloadStudents()
        {

            loadScreen.ShowAsync();
            allStudents = await ApiHelper.Instance.GetAllStudentsTestsQuestions();
            loadScreen.Hide();

        }

        /// <summary>
        /// Finds the tests that contain questions that havn't been graded yet and populates a list with them.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Test>> GetUngradedTests()
        {
            List<Test> ungradedTests = new List<Test>();

            foreach (var student in allStudents)
            {
                foreach(var studentTest in student.Tests)
                {
                    if((studentTest.Questions.Select(x => x.QuestionAnswer)).Any(x => x.IsCorrect == null))
                    {
                        if (!ungradedTests.Any(test => test.TestId == studentTest.TestId))
                            ungradedTests.Add(studentTest);
                    }
                }
            }
            
            return ungradedTests;
        }

        /// <summary>
        /// After clicking a test this method populates the list with students who've take
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="studentsWithTestList"></param>
        public void PopulateStudentsWithTestList(int testId, ObservableCollection<Student> studentsWithTestList)
        {
            studentsWithTestList.Clear();

            var tempStudentList = allStudents
                .Where(student => 
                    student.Tests
                           .Any(test => 
                               test.TestId == testId && (test.Questions
                                                              .Select(x => x.QuestionAnswer))
                                                              .Any(x => x.IsCorrect == null)))
                .Select(x => x)
                .ToList();
            foreach(var student in tempStudentList)
            {
                studentsWithTestList.Add(student);
            }
        }

        /// <summary>
        /// Finds the questions based on a student and test and populates a list that is bound to the Listview for grading
        /// </summary>
        /// <param name="chosenTestId"></param>
        /// <param name="chosenStudent"></param>
        /// <param name="questionsForStudentAndTestList"></param>
        public void PopulateUngradedQuestionsForStudent(int chosenTestId, Model.Student chosenStudent, ObservableCollection<Question> questionsForStudentAndTestList)
        {
            questionsForStudentAndTestList.Clear();
            var tempQuestionList = chosenStudent.Tests.Where(test => test.TestId == chosenTestId).Select(test => test.Questions).FirstOrDefault();

            foreach (var question in tempQuestionList)
            {
                if (question.QuestionAnswer.IsCorrect == null)
                    questionsForStudentAndTestList.Add(question);
            }
        }
        /// <summary>
        /// When the user has finished grading the questions they want to grade,
        /// This method compiles the questions into a list and sends it off to the API for writing to the database.
        /// If all the questions have been graded it also sends a PATCH request to update the test for that student.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void FinishGradingTest(ListView listView_QuestionsForStudentAndTest, Student chosenStudent, int chosenTestId)
        {
            List<StudentQuestionAnswer> gradedQuestions = new List<StudentQuestionAnswer>();
            Question question;
            int numberOfQuestionItems = listView_QuestionsForStudentAndTest.Items.Count;

            //Go through all the items inside the listview. This retrieves question objects
            foreach (var item in listView_QuestionsForStudentAndTest.Items)
            {
                //Cast it to a variable of the type.
                question = (Question)item;

                //Get the container that holds the question object.
                var container = listView_QuestionsForStudentAndTest.ContainerFromItem(item);

                //find all it's children of the 'control' type. (in this case it will find all the radiobuttons attached to the container of the question object.)
                var children = AllChildren(container);

                //go through the radiobuttons and find which one is clicked. 
                foreach (var control in children)
                {
                    RadioButton button = (RadioButton)control;

                    //if the question is marked with correct or false. the changes are staged for sending to the database.
                    //alternatively they can be marked as "ungraded" this will keep the questions in the list so the teacher can come back at a later date and grade it.
                    if (button.Name == "radioButton_QuestionCorrect" && button.IsChecked == true)
                    {
                        question.QuestionAnswer.IsCorrect = true;
                        gradedQuestions.Add(new StudentQuestionAnswer(chosenStudent.StudentId, chosenTestId, question.QuestionID, question.QuestionAnswer.Answer, true)); 
                    }
                    else if (button.Name == "radioButton_QuestionIncorrect" && button.IsChecked == true)
                    {
                        question.QuestionAnswer.IsCorrect = false;
                        gradedQuestions.Add(new StudentQuestionAnswer(chosenStudent.StudentId, chosenTestId, question.QuestionID, question.QuestionAnswer.Answer, false) { });
                    }
                }
            }
            
            var success = false;

            //Send the questions staged for changing to the api
            if (gradedQuestions.Count > 0)
                success = await ApiHelper.Instance.UpdateStudentQuestionAnswer(gradedQuestions);

            //if the changes were saved on the database and we find that all the questions for a student and test have been saved,
            //we send another POST to the testresult bridge table where we store the points for the students test.
            if (success &&
                gradedQuestions.Count == numberOfQuestionItems)
            {
                //Send another request to the API to get all the corrected questions
                //(since this can be done in multiple sessions of grading)
                var points = await GetTotalPoints(chosenStudent, chosenTestId);

                TestResult result = new TestResult(chosenStudent.StudentId, chosenTestId, points);
                ApiHelper.Instance.PostTestResult(result);
            }
        }

        /// <summary>
        /// Sends a request to the api to get all the results of a students written test and counts up the aquired points value
        /// </summary>
        /// <param name="chosenStudent"></param>
        /// <param name="chosenTestId"></param>
        /// <returns></returns>
        private async Task<int> GetTotalPoints(Model.Student chosenStudent, int chosenTestId)
        {
            List<StudentQuestionAnswer> studentQuestionAnswerList = await ApiHelper.Instance.GetAllStudentQuestionAnswers();
            var filteredSQAList = studentQuestionAnswerList.Where(sqa => sqa.StudentId == chosenStudent.StudentId && sqa.TestId == chosenTestId).Select(sqa => sqa).ToList();

            var questionList = chosenStudent.Tests.Where(t => t.TestId == chosenTestId).Select(t => t.Questions).FirstOrDefault();

            var totalPoints = 0;
            foreach(var sqa in filteredSQAList)
            {
                Question q = questionList.FirstOrDefault(x => x.QuestionID == sqa.QuestionId);
                if(q != null)
                {
                    if (sqa.IsCorrect == true)
                        totalPoints += q.PointValue;
                }
            }
            return totalPoints;
        }

        /// <summary>
        /// Goes through a UI element and gets all the children of it that are labeled as 'Controlls'
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public List<Control> AllChildren(DependencyObject parent)
        {
            var list = new List<Control>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is Control)
                    list.Add(child as Control);
                list.AddRange(AllChildren(child));
            }
            return list;
        }

        #endregion
    }
}
