using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QualityPOS.Objects;
using QualityPOS.Objects.DTO;
using QualityPOS.Repository;


namespace QualityPOS.Manager
{
    public class CategoryManager
    {
        RepositoryNgPinas _repositoryNgPinas = new RepositoryNgPinas();

        public async Task<Result> Add(Category category)
        {
            return await _repositoryNgPinas.Insert("Category", category, "CategoryID");
        }

        public async Task<Category> GetByName(string name)
        {
            var sql = $@"SELECT TOP 1 * FROM Category WHERE CategoryName = @CategoryName";
            return await _repositoryNgPinas.QuerySingleAsync<Category>(sql, new { CategoryName = name });
        }

        public async Task<List<Category>> GetAll()
        {
            var sql = $@"SELECT * FROM Category ORDER BY CategoryName";
            return await _repositoryNgPinas.QueryMultipleAsync<Category>(sql);
        }
    }
}
