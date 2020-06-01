using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Model
{
   public class Course
   {
        #region Constant Fields
        #endregion

        #region Fields
        #endregion

        #region Constructors
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        public int CourseId { get; set; }

        public string CourseName { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Tells the Json converter that it shouldn't use the ID property when serializing.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeCourseId() => false;
        #endregion

   }
}
