using Dapper.DataRepositories;
using MicroOrm.Pocos.SqlGenerator;
using NB.Core.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
//Tutorial: https://github.com/Yoinbol/Dapper.DataRepositories

namespace NB.Core.Web.DataAccess.Repository
{
    //DataRepository<NasdaqEarningForecastData>, IDataRepository<NasdaqEarningForecastData>
    public class EarningForecastRepository : DataConnection, IEventSourceRepository<NasdaqEarningForecastData>
    {
        //public EarningForecastRepository(IDbConnection connection, ISqlGenerator<NasdaqEarningForecastData> sqlGenerator)
        //    : base (connection, sqlGenerator)
        public EarningForecastRepository(IDbConnection connection) : base(connection)	
        {

        }

        public void Insert (NasdaqEarningForecastData data)
        {

        }

        public IQueryable<NasdaqEarningForecastData> GetAll()
        {
            var sql = SqlStatmentFactory.SqlStatment.GetSelectAll();
            return Connection.Query<NasdaqEarningForecastData>(sql).AsQueryable();
        }

        public IEnumerable<NasdaqEarningForecastData> GetByTicker(string ticker)
        {
            var sql = SqlStatmentFactory.SqlStatment.GetSelect(new { Ticker = ticker });
            return Connection.Query<NasdaqEarningForecastData>(sql);
        }

        public bool Add(NasdaqEarningForecastData entity)
        {
            var sql = SqlStatmentFactory.SqlStatment.GetInsert();
            var newId = Connection.Query<int>(sql, entity).Single();
            var inserted = newId > 0;

            if (inserted)
            {
                entity.Id = (int)newId;
            }

            return inserted;

        }

    }
}
