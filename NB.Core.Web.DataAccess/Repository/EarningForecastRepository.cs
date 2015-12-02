using Dapper.DataRepositories;
using MicroOrm.Pocos.SqlGenerator;
using NB.Core.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
//Tutorial: https://github.com/Yoinbol/Dapper.DataRepositories

namespace NB.Core.Web.DataAccess.Repository
{
    //DataRepository<NasdaqEarningForecastData>, IDataRepository<NasdaqEarningForecastData>
    public class EarningForecastRepository : IEventSourceRepository<NasdaqEarningForecastData>
    {
        private DbProviderFactory _factory;
        private string _connectionString;

        public EarningForecastRepository(string database)
        {
            var setting = ConfigurationManager.ConnectionStrings[database];
            _factory = DbProviderFactories.GetFactory(setting.ProviderName);
            _connectionString = setting.ConnectionString;
        }

        private IDbConnection GetConnection()
        {
            IDbConnection connection = _factory.CreateConnection();
            connection.ConnectionString = _connectionString;
            connection.Open();

            return connection;
        }      

        public IQueryable<NasdaqEarningForecastData> GetAll()
        {
            var sql = SqlStatmentFactory.NasdaqEarningForecastDataSqlStatment.GetSelectAll();
            using (var connection = GetConnection())
            {
                return connection.Query<NasdaqEarningForecastData>(sql).AsQueryable();
            }
        }

        public IEnumerable<NasdaqEarningForecastData> GetByTicker(string ticker)
        {
            var sql = SqlStatmentFactory.NasdaqEarningForecastDataSqlStatment.GetSelect(new { Ticker = ticker });
            using (var connection = GetConnection())
            {
                return connection.Query<NasdaqEarningForecastData>(sql);
            }
        }

        public bool Save(IEnumerable<NasdaqEarningForecastData> forecasts)
        {
            var sql = SqlStatmentFactory.NasdaqEarningForecastDataSqlStatment.GetInsert();
            using (var connection = GetConnection())
            {
                var num = connection.Execute(sql, forecasts);
                return -1 < num;
            }
        }

        public bool Add(NasdaqEarningForecastData entity)
        {
            var sql = SqlStatmentFactory.NasdaqEarningForecastDataSqlStatment.GetInsert();
            using (var connection = GetConnection())
            {
                var newId = connection.Query<int>(sql, entity).Single();
                var inserted = newId > 0;

                if (inserted)
                {
                    entity.Id = (int) newId;
                }

                return inserted;
            }

        }

    }
}
