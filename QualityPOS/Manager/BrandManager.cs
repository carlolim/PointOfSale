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
    public class BrandManager
    {
        RepositoryNgPinas _repositoryNgPinas = new RepositoryNgPinas();

        public async Task<Result> Add(Brand brand)
        {
            return await _repositoryNgPinas.Insert("Brand", brand, "BrandID");
        }

        public async Task<Brand> GetByName(string name)
        {
            var sql = $@"SELECT TOP 1 * FROM Brand WHERE BrandName = @BrandName";
            return await _repositoryNgPinas.QuerySingleAsync<Brand>(sql, new { BrandName = name });
        }

        public async Task<List<Brand>> GetAll()
        {
            var sql = $@"SELECT * FROM Brand ORDER BY BrandName";
            return await _repositoryNgPinas.QueryMultipleAsync<Brand>(sql);
        }
    }
}
