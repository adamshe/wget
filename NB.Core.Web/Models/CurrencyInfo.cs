using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NB.Core.Web.Models
{
    public class CurrencyInfo
    {

        private string mID = string.Empty;
        /// <summary>
        /// The currency ID.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ID
        {
            get { return mID; }
            set { mID = value.ToUpper(); }
        }
        /// <summary>
        /// The currency name/description.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Description { get; set; }

        /// <summary>
        /// Defaul constructor
        /// </summary>
        /// <remarks></remarks>
        public CurrencyInfo(string curID, string curDesc)
        {
            this.ID = curID;
            this.Description = curDesc;
        }

        /// <summary>
        /// Returns Description and ID.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            if (this.Description != string.Empty)
            {
                return this.Description + " (" + this.ID.ToString() + ")";
            }
            else
            {
                return this.ID.ToString();
            }
        }

    }
}
