using NB.Core.Web.Interfaces;
using NB.Core.Web.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Models
{
    public class YCurrencyID : IID, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private CurrencyInfo mBaseCurrency;
        private CurrencyInfo mDepCurrency;

        /// <summary>
        ///The Yahoo ID of the relation
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ID
        {
            get
            {
                if (mBaseCurrency != null && mDepCurrency != null)
                {
                    return string.Format("{0}{1}=X", mBaseCurrency.ID, mDepCurrency.ID);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// The currency with the ratio value of 1
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CurrencyInfo BaseCurrency
        {
            get { return mBaseCurrency; }
            set { mBaseCurrency = value; this.OnPropertyChanged(); }
        }
        /// <summary>
        /// The currency of the dependent value
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CurrencyInfo DepCurrency
        {
            get { return mDepCurrency; }
            set { mDepCurrency = value; this.OnPropertyChanged(); }
        }
        /// <summary>
        /// The display name of the relation
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string DisplayName
        {
            get
            {
                if (mBaseCurrency != null && mDepCurrency != null)
                {
                    return string.Format("{0} to {1}", mBaseCurrency.Description, mDepCurrency.Description);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks></remarks>
        public YCurrencyID()
        {
            mBaseCurrency = null;
            mDepCurrency = null;
        }
        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="baseCur"></param>
        /// <param name="depCur"></param>
        /// <remarks></remarks>
        public YCurrencyID(CurrencyInfo baseCur, CurrencyInfo depCur)
        {
            this.BaseCurrency = baseCur;
            this.DepCurrency = depCur;
        }
        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="id"></param>
        /// <remarks></remarks>
        //public YCurrencyID(string id)
        //{
        //    YCurrencyID newRel = FinanceHelper.YCurrencyIDFromString(id);
        //    if (newRel != null)
        //    {
        //        this.BaseCurrency = newRel.BaseCurrency;
        //        this.DepCurrency = newRel.DepCurrency;
        //    }
        //    else
        //    {
        //        throw new ArgumentException("The id is not valid", "id");
        //    }
        //}

        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) 
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Returns the ID of the currency relation
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return this.ID;
        }
    }
}
