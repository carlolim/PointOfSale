using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QualityPOS.Repository;
using QualityPOS.Objects;
using QualityPOS.Objects.DTO;

namespace QualityPOS.Manager
{
    public class SaleManager
    {
        private RepositoryNgPinas _repositoryNgPinas = new RepositoryNgPinas();

        public Sale GetByID(int id)
        {
            return _repositoryNgPinas.Select<Sale>("Sale", "SaleID", id);
        }

        public async Task<List<SaleDTO>> GetTransactionsByStoreID(int id)
        {
            string saleByStoreIDQuery = $@"SELECT
                                                u.Firstname,
                                                u.Lastname,
                                                s.*
                                            FROM
                                                Sale s
                                                INNER JOIN [User] u
                                                ON s.UserID = u.UserID
                                            WHERE StoreID = @StoreID";
            var sales = await _repositoryNgPinas.QueryMultipleAsync<SaleDTO>(saleByStoreIDQuery, new { StoreID = id });

            var saleProductQuery = $@"SELECT 
	                                        sp.Quantity, 
	                                        p.Name,
	                                        sp.SaleID,
                                            sp.ProductPrice,
                                            sp.ProductCost,
                                            (sp.ProductPrice - sp.ProductCost) * sp.Quantity AS Net
                                        FROM
	                                        SaleProduct sp
	                                        INNER JOIN Product p
	                                        ON sp.ProductID = p.ProductID
                                        WHERE 
	                                        sp.SaleID IN ({ string.Join(", ", sales.Select(m => m.SaleID).ToList()) })";
            var saleProducts = await _repositoryNgPinas.QueryMultipleAsync<SaleDTO>(saleProductQuery);
            
            foreach (var sale in sales)
            {
                var saleItems = saleProducts.Where(m => m.SaleID == sale.SaleID);
                sale.Items = string.Join(", ", saleItems.Select(m => $"{ m.Quantity } { m.Name } ").ToList());
                sale.DateStr = sale.DateTime.ToString("MMM dd,yyyy");
                sale.TimeStr = sale.DateTime.ToString("hh:mm tt");
                sale.Net = saleItems.Sum(m => m.Net);
            }

            return sales.OrderByDescending(m => m.DateTime).ToList();
        }


        public async Task<List<SaleDTO>> GetTransactions()
        {
            string saleByStoreIdQuery = $@"SELECT
                                                u.Firstname,
                                                u.Lastname,
                                                s.*
                                            FROM
                                                Sale s
                                                INNER JOIN [User] u
                                                ON s.UserID = u.UserID";
            var sales = await _repositoryNgPinas.QueryMultipleAsync<SaleDTO>(saleByStoreIdQuery);

            var saleProductQuery = $@"SELECT 
	                                        sp.Quantity, 
	                                        p.Name,
	                                        sp.SaleID,
                                            sp.ProductPrice,
                                            sp.ProductCost,
                                            (sp.ProductPrice - sp.ProductCost) * sp.Quantity AS Net
                                        FROM
	                                        SaleProduct sp
	                                        INNER JOIN Product p
	                                        ON sp.ProductID = p.ProductID
                                        WHERE 
	                                        sp.SaleID IN ({ string.Join(", ", sales.Select(m => m.SaleID).ToList()) })";
            var saleProducts = await _repositoryNgPinas.QueryMultipleAsync<SaleDTO>(saleProductQuery);

            foreach (var sale in sales)
            {
                var saleItems = saleProducts.Where(m => m.SaleID == sale.SaleID);
                sale.Items = string.Join(", ", saleItems.Select(m => $"{ m.Quantity } { m.Name } ").ToList());
                sale.DateStr = sale.DateTime.ToString("MMM dd,yyyy");
                sale.TimeStr = sale.DateTime.ToString("hh:mm tt");
            }

            return sales.OrderByDescending(m => m.DateTime).ToList();
        }


        public async Task<Result> AddSale(Sale sale)
        {
            Result result = new Result();

            result = await _repositoryNgPinas.Insert("Sale", sale, "SaleID");

            return result;
        }

        public async Task<Result> UpdateSale(Sale sale)
        {
            Result result = new Result();

            result = await _repositoryNgPinas.Update("Sale", sale, new List<string>() { "SaleID" }, new List<object>() { sale.SaleID }, "SaleID");

            return result;
        }

        public async Task<Result> AddSaleProducts(List<SaleProduct> saleProducts)
        {
            Result result = new Result();

            result = _repositoryNgPinas.InsertBulk("SaleProduct", saleProducts);

            return result;
        }

        public async Task<Result> UpdateSaleProduct(List<SaleProduct> saleProducts, int saleID)
        {
            Result result = new Result();

            var sale = GetByID(saleID);

            if (sale != null)
            {
                #region Return all products to store
                var originalSaleProductsQuery = $@"SELECT * FROM SaleProduct WHERE SaleID = @SaleID";
                var originalSaleProducts = await _repositoryNgPinas.QueryMultipleAsync<SaleProduct>(originalSaleProductsQuery, new { SaleID = saleID });

                foreach(var origSaleProduct in originalSaleProducts)
                {
                    var sql = $@"UPDATE StoreProduct SET UnitsSold = UnitsSold - @Quantity, UnitsLeft = UnitsLeft + @Quantity WHERE StoreID = @StoreID AND ProductID = @ProductID";
                    var r = await _repositoryNgPinas.ExecuteAsync(sql, new { Quantity = origSaleProduct.Quantity, StoreID = sale.StoreID, ProductID = origSaleProduct.ProductID });
                    result.IsSuccess = r > 0;
                }

                if (result.IsSuccess)
                {
                    var deleteSaleProductQuery = $@"DELETE FROM SaleProduct WHERE SaleID = @SaleID";
                    var r = await _repositoryNgPinas.ExecuteAsync(deleteSaleProductQuery, new { SaleID = saleID });
                    result.IsSuccess = r > 0;
                }
                #endregion

                if (result.IsSuccess)
                {
                    foreach (var saleProduct in saleProducts)
                    {
                        result = await AddSaleProduct(saleProduct);
                    }
                }

            }

            return result;
        }

        public async Task<Result> AddSaleProduct(SaleProduct saleProduct)
        {
            return await _repositoryNgPinas.Insert("SaleProduct", saleProduct, "SaleProductID");
        }

        public async Task<Result> DeleteSale(int id)
        {
            Result result = new Result();
            var sql = $@"DELETE FROM [Sale] WHERE SaleID = @SaleID";
            var r = await _repositoryNgPinas.ExecuteAsync(sql, new { SaleID = id });
            result.IsSuccess = r > 0;

            return result;
        }

        public async Task<Result> DeleteSaleProductBySaleID(int id)
        {
            Result result = new Result();
            var sql = $@"DELETE FROM [SaleProduct] WHERE SaleID = @SaleID";
            var r = await _repositoryNgPinas.ExecuteAsync(sql, new { SaleID = id });
            result.IsSuccess = r > 0;

            return result;
        }

        public async Task<double> GetTotalSaleByStoreID(int id)
        {
            var sql = $@"SELECT CASE WHEN SUM(total) IS NULL THEN 0 ELSE SUM(total) END FROM Sale WHERE StoreID = @StoreID";
            return await _repositoryNgPinas.QuerySingleAsync<double>(sql, new { StoreID = id });
        }

        public async Task<List<SaleProductDTO>> GetSaleProductsBySaleID(int id)
        {
            List<SaleProductDTO> result = new List<SaleProductDTO>();

            var sql = $@"SELECT
                            p.ProductID,
	                        p.Name as ProductName,
	                        sp.Quantity,
	                        p.SalesPrice
                        FROM
	                        SaleProduct sp
	                        INNER JOIN Product p
	                        ON sp.ProductID = p.ProductID
                        WHERE
	                        sp.SaleID = @SaleID";
            result = await _repositoryNgPinas.QueryMultipleAsync<SaleProductDTO>(sql, new { SaleID = id });

            return result;
        }
    }
}
