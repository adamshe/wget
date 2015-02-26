using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Interfaces
{
    public interface IID
    {
        /// <summary>
        /// The valid Yahoo! ID.
        /// </summary>
        /// <value></value>
        /// <returns>The full ID built by the implementing class.</returns>
        /// <remarks></remarks>
        string ID { get; }
    }

    /// <summary>
    /// Interface for Yahoo! ID. Inherits from IID and provides a settable ID.
    /// </summary>
    /// <remarks></remarks>
    public interface ISettableID : IID
    {
        /// <summary>
        /// Provides the possibility to set the ID from outside of the class.
        /// </summary>
        /// <param name="id">A valid Yahoo! ID</param>
        /// <remarks></remarks>
        void SetID(string id);
    }
}
