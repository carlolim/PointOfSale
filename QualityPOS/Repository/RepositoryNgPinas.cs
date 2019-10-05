using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using QualityPOS.Objects;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ErikEJ.SqlCe;

namespace QualityPOS.Repository
{

    public class RepositoryNgPinas
    {
        /// <summary>
        /// isang where lang. Eg: select * from somewhere where something = something
        /// </summary>
        public T Select<T>(string tableName, string column, object columnValue)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add($"@{ column }", columnValue);

            StringBuilder query = new StringBuilder();
            query.Append($"SELECT * FROM [{ tableName }] WHERE [{ column }] = @{ column }");

            using (var con = new DatabaseConnection().Connection)
            {
                return con.Query<T>(query.ToString(), p, commandType: CommandType.Text).FirstOrDefault();
            }
        }

        /// <summary>
        /// multiple where. Eg: select * from somewhere where something = something and fuck = this and shit = shot
        /// </summary>
        public T Select<T>(string tableName, List<string> columns, List<object> columnValues)
        {
            DynamicParameters p = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append($"SELECT * FROM [{ tableName }] WHERE ");

            int ctr = 1;
            foreach (var column in columns)
            {
                p.Add($"@{ column }", columnValues[ctr - 1]);
                query.Append($" [{ column }] = @{ column }");
                if (ctr < columns.Count())
                    query.Append($" AND ");
                ctr++;
            }

            using (var con = new DatabaseConnection().Connection)
            {
                con.Open();
                var result = con.Query<T>(query.ToString(), p, commandType: CommandType.Text).FirstOrDefault();
                con.Close();
                return result;
            }
        }

        /// <summary>
        /// isang where lang. Eg: Select * from somewhere where something = something
        /// </summary>
        public List<T> SelectList<T>(string tableName, string column, object columnValue)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add($"@{ column }", columnValue);

            StringBuilder query = new StringBuilder();
            query.Append($"SELECT * FROM [{ tableName }] WHERE [{ column }] = @{ column }");

            using (var con = new DatabaseConnection().Connection)
            {
                return con.Query<T>(query.ToString(), p, commandType: CommandType.Text).ToList();
            }
        }

        public List<T> SelectList<T>(string tableName, List<string> columns, List<object> columnValues)
        {
            DynamicParameters p = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append($"SELECT * FROM [{ tableName }] WHERE ");

            int ctr = 1;
            foreach (var column in columns)
            {
                p.Add($"@{ column }", columnValues[ctr - 1]);
                query.Append($" [{ column }] = @{ column }");
                if (ctr < columns.Count())
                    query.Append($" AND ");
                ctr++;
            }

            using (var con = new DatabaseConnection().Connection)
            {
                con.Open();
                var result = con.Query<T>(query.ToString(), p, commandType: CommandType.Text).ToList();
                con.Close();
                return result;
            }
        }

        public List<T> SelectAll<T>(string tableName)
        {
            using (var con = new DatabaseConnection().Connection)
            {
                return con.Query<T>($"SELECT * FROM [{ tableName }]").ToList();
            }
        }

        public async Task<Result> Insert(string tableName, object entity, string primaryKey = null)
        {
            Result result = new Result();

            var fields = GetAllProperties(entity).ToList();
            if (!string.IsNullOrWhiteSpace(primaryKey))
            {
                var pk = fields.Where(m => m.Name.ToLower() == primaryKey.ToLower()).FirstOrDefault();
                if (pk != null)
                    fields.Remove(pk);
            }

            StringBuilder query = new StringBuilder();
            query.Append($"INSERT INTO [{ tableName }] (");

            int ctr = 1;
            foreach (var field in fields)
            {
                query.Append($"[{ field.Name }]");
                if (ctr < fields.Count())
                    query.Append(", ");
                ctr++;
            }

            query.Append(") VALUES (");

            ctr = 1;
            foreach (var field in fields)
            {
                query.Append($"@{ field.Name }");
                if (ctr < fields.Count())
                    query.Append(", ");
                ctr++;
            }

            query.Append(")");

            
            using (var con = new DatabaseConnection().Connection)
            {
                con.Open();
                int r = await con.ExecuteAsync(query.ToString(), entity);

                if (!string.IsNullOrWhiteSpace(primaryKey))
                {
                    var query1 = $@"SELECT TOP 1 [{ primaryKey }] FROM [{ tableName }] ORDER BY [{ primaryKey }] DESC";
                    r = await con.QueryFirstOrDefaultAsync<int>(query1);
                }

                con.Close();

                result.IsSuccess = r > 0;
                result.ID = r;
            }

            return result;
        }

        public Result InsertBulk(string tableName, IEnumerable<object> entity)
        {
            Result result = new Result();

            try
            {
                using (var con = new DatabaseConnection().Connection)
                {
                    con.Open();
                    using (SqlCeBulkCopy copy = new SqlCeBulkCopy(con))
                    {
                        copy.DestinationTableName = tableName;
                        var table = DataTableHelper.ToDataTable(entity.ToList(), tableName);
                        copy.WriteToServer(table);
                        result.IsSuccess = true;
                    }
                    con.Close();
                }
            }
            catch(Exception ex)
            {

            }

            return result;
        }

        public async Task<Result> Update(string tableName, object entity, List<string> whereColumns, List<object> whereColumnValues, string primaryKey = null)
        {
            Result result = new Result();

            var fields = GetAllProperties(entity).ToList();
            if (!string.IsNullOrWhiteSpace(primaryKey))
            {
                var pk = fields.Where(m => m.Name.ToLower() == primaryKey.ToLower()).FirstOrDefault();
                if (pk != null)
                    fields.Remove(pk);
            }

            StringBuilder query = new StringBuilder();
            query.Append($"UPDATE [{ tableName }] SET ");
            int ctr = 1;
            foreach (var field in fields)
            {
                query.Append($" [{ field.Name }] = @{ field.Name }");
                if (ctr < fields.Count())
                    query.Append(", ");
                ctr++;
            }
            query.Append(" WHERE ");
            ctr = 1;
            DynamicParameters p = new DynamicParameters();

            foreach (var whereColumn in whereColumns)
            {
                query.Append($" [{ whereColumn }] = @{ whereColumn }");
                if (ctr < whereColumns.Count())
                    query.Append(" AND ");
                ctr++;
            }

            using (var con = new DatabaseConnection().Connection)
            {
                con.Open();
                int r = await con.ExecuteAsync(query.ToString(), entity);
                con.Close();

                result.IsSuccess = r > 0;
                result.ID = r;
            }

            return result;
        }

        public async Task<Result> Delete<T>(string tableName, object entity, List<string> whereColumns, List<object> whereColumnValues)
        {
            Result result = new Result();
            var fields = GetAllProperties(entity).ToList();
            StringBuilder query = new StringBuilder();
            int ctr = 1;
            DynamicParameters p = new DynamicParameters();

            query.Append($"DELETE FROM [{ tableName }] WHERE ");

            foreach (var whereColumn in whereColumns)
            {
                p.Add($"@{ whereColumn }", whereColumnValues[ctr - 1]);
                query.Append($" [{ whereColumn }] = @{ whereColumns[ctr - 1] }");
                if (ctr < whereColumns.Count())
                    query.Append(" AND ");
                ctr++;
            }

            using (var con = new DatabaseConnection().Connection)
            {
                con.Open();
                int r = await con.QueryFirstAsync<int>(query.ToString(), p, commandType: CommandType.Text);
                con.Close();

                result.IsSuccess = r > 0;
                result.ID = r;
            }

            return result;
        }

        private string GetPrimaryKey(object entity)
        {
            var type = entity.GetType();
            var primary = type.GetProperties().Where(m => m.GetCustomAttributes(true).Any(attr => attr.GetType().Name == typeof(KeyAttribute).Name)).FirstOrDefault();
            return primary.Name;
        }

        public async Task<List<TEntity>> QueryMultipleAsync<TEntity>(string sql, object parameter = null) where TEntity : new()
        {
            var result = new List<TEntity>();
            try
            {
                using (var con = new DatabaseConnection().Connection)
                {
                    result = (await con.QueryAsync<TEntity>(sql, parameter)).ToList();
                }
            }
            catch(Exception ex)
            {

            }
            return result;
        }

        public async Task<TEntity> QuerySingleAsync<TEntity>(string sql, object parameter = null) where TEntity : new()
        {
            var result = new TEntity();

            using (var con = new DatabaseConnection().Connection)
            {
                result = await con.QueryFirstOrDefaultAsync<TEntity>(sql, parameter);
            }
            return result;
        }

        public async Task<int> ExecuteAsync(string sql, object parameter = null)
        {
            int result = 0;

            using (var con = new DatabaseConnection().Connection)
            {
                result = await con.ExecuteAsync(sql, parameter);
            }
            return result;
        }

        private IEnumerable<PropertyInfo> GetAllProperties(object entity)
        {
            if (entity == null) entity = new { };
            return entity.GetType().GetProperties();
        }
    }

    public static class DataTableHelper
    {
        public static DataTable ToDataTable<T>(this IList<T> data, string tableName)
        {
            DataTable table = new DataTable(tableName);
            try
            {
                var properties = data.FirstOrDefault().GetType().GetProperties();
                foreach (var property in properties)
                {
                    table.Columns.Add(property.Name, property.PropertyType);
                }

                object[] values = new object[properties.Count()];
                foreach (T item in data)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = properties[i].GetValue(item);
                    }
                    table.Rows.Add(values);
                }
            }
            catch(Exception ex)
            {

            }
            return table;
        }
    }
}
