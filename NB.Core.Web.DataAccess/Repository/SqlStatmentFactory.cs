using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grace.DependencyInjection;
using MicroOrm.Pocos.SqlGenerator;
using NB.Core.Web.Models;
namespace NB.Core.Web.DataAccess.Repository
{
    public class SqlStatmentFactory
    {
        static DependencyInjectionContainer container = new DependencyInjectionContainer();
        static SqlStatmentFactory()
        {
            container.Configure(c => c.Export <SqlGenerator<NasdaqEarningForecastData>>()
                .As <ISqlGenerator<NasdaqEarningForecastData>>());
            
            //DI.Container.RegisterType<SqlGenerator<User>>().As<ISqlGenerator<User>>().Singleton();
        }

        public static ISqlGenerator<NasdaqEarningForecastData> NasdaqEarningForecastDataSqlStatment
        {
            get
            {
                return container.Locate<ISqlGenerator<NasdaqEarningForecastData>>();
            }
        }

        //public static ISqlGenerator<T> GetSqlStatment<T>() where T : new()
        //{
        //    return _container.Locate<ISqlGenerator<T>>();
        //}
    }
}
