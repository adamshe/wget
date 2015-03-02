using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NB.Core.Web.Models
{
    public class SharesSplitFactor
    {

        /// <summary>
        /// Old relational value.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int OldShares { get; set; }
        /// <summary>
        /// New relational value.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int NewShares { get; set; }


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="newShares">The new number of shares after splitting (relative)</param>
        /// <param name="forOldShares">The old number of shares before splitting (relative)</param>
        /// <remarks></remarks>
        public SharesSplitFactor(int newShares, int forOldShares)
        {
            this.OldShares = forOldShares;
            this.NewShares = newShares;
        }

        public override string ToString()
        {
            return string.Format("{0} : {1}", this.NewShares, this.OldShares);
        }

    }
}
