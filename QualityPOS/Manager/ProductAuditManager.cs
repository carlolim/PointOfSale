using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QualityPOS.Objects;
using QualityPOS.Repository;

namespace QualityPOS.Manager
{
    public class ProductAuditManager
    {
        RepositoryNgPinas _repositoryNgPinas = new RepositoryNgPinas();

        public async Task<Result> Add(ProductAudit productAudit)
        {
            var result = new Result();

            var sql =
                $@"INSERT INTO ProductAudit (ProductID, Quantity, DateCreated, UserCreatedID) VALUES (@ProductID, @Quantity, GETDATE(), @UserCreatedID)";
            var r = await _repositoryNgPinas.ExecuteAsync(sql,
                new
                {
                    ProductID = productAudit.ProductID,
                    Quantity = productAudit.Quantity,
                    UserCreatedID = productAudit.UserCreatedID
                });
            result.IsSuccess = r > 0;
            return result;
        }
    }
}
