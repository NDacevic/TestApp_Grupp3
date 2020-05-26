using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace TestApp
{
    class NavigationHelper
    {
        #region Constant Fields
        #endregion

        #region Fields
        private NavigationHelper instance;
        #endregion

        #region Constructors
        private NavigationHelper()
        {

        }
        #endregion

        #region Delegates
        #endregion

        #region Events
        #endregion

        #region Properties
        public NavigationHelper Instance { get
            {
                if (instance == null)
                {
                    instance = new NavigationHelper();
                }
                return instance;
            } 
        }

        public Frame GlobalFrame { get; set; }
        #endregion

        #region Methods
        #endregion
    }
}
