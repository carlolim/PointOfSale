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
    public class StoreManager
    {
        private readonly RepositoryNgPinas _repositoryNgPinas = new RepositoryNgPinas();

        public async Task<Store> GetById(int id)
        {
            string sql = $@"SELECT TOP 1 * FROM Store WHERE StoreID = @StoreID";
            return await _repositoryNgPinas.QuerySingleAsync<Store>(sql, new { StoreID = id });
        }

        public async Task<Store> OpenStore(User user)
        {
            Store result = new Store();

            //get last store para sa move ng products
            var lastStoreQuery = $@"SELECT TOP 1 * FROM [Store] ORDER BY StoreID DESC";
            var lastStore = await _repositoryNgPinas.QuerySingleAsync<Store>(lastStoreQuery);

            //close all previous store
            var closeAllStoreQuery = $@"UPDATE [Store] SET IsOpen = 0";
            await _repositoryNgPinas.ExecuteAsync(closeAllStoreQuery);

            //open new store
            var tobeInserted = new Store()
            {
                DateCreated = DateTime.Now,
                DateOpen = DateTime.Now,
                IsDeleted = false,
                IsOpen = true,
                UserCreatedID = user.UserID,
                UserID = user.UserID
            };
            var inserted = await _repositoryNgPinas.Insert("Store", tobeInserted, "StoreID");
            result = _repositoryNgPinas.Select<Store>("Store", "StoreID", inserted.ID);

            if (lastStore != null)
            {
                //move all items ng last store to current store
                //pag-open ng store quantity = unitsleft, unitssold = 0
                string moveQuery = $@"INSERT INTO StoreProduct
                                        (StoreID, ProductID, [Quantity], UnitsSold, UnitsLeft, UserCreatedID, DateCreated, IsDeleted)
                                        SELECT
                                            @NewStoreID
                                            ,[ProductID]
                                            ,[UnitsLeft]
                                            ,0
                                            ,[UnitsLeft]
                                            ,@UserCreatedID
                                            ,GETDATE()
                                            ,0
                                        FROM [StoreProduct]
                                        WHERE
                                          StoreID = @OldStoreID
                                          AND IsDeleted = 0";
                await _repositoryNgPinas.ExecuteAsync(moveQuery,
                    new { NewStoreID = inserted.ID, OldStoreID = lastStore.StoreID, UserCreatedID = user.UserID });
            }

            return result;
        }

        public async Task<List<StoreProductDTO>> GetStoreProductsByStoreID(int id)
        {
            var sql = $@"SELECT
	                        sp.StoreProductID,
	                        p.Name,
	                        p.Code,
	                        CASE WHEN p.SalesPrice IS NULL THEN 0.0 ELSE p.SalesPrice END AS SalesPrice,
	                        sp.Quantity,
	                        sp.UnitsSold,
	                        CASE WHEN (sp.UnitsSold * p.SalesPrice) IS NULL THEN 0.0 ELSE (sp.UnitsSold * p.SalesPrice) END AS TotalPriceSold,
	                        sp.UnitsLeft,
	                        CASE WHEN (sp.UnitsLeft * p.SalesPrice) IS NULL THEN 0.0 ELSE (sp.UnitsLeft * p.SalesPrice) END AS TotalPriceLeft,
	                        sp.ProductID,
	                        sp.StoreID
                        FROM
	                        StoreProduct sp
	                        INNER JOIN Product p
	                        ON sp.ProductID = p.ProductID
                        WHERE
	                        sp.StoreID = @StoreID
                            AND p.IsDeleted = 0
                        ORDER BY p.Name";
            return await _repositoryNgPinas.QueryMultipleAsync<StoreProductDTO>(sql, new { StoreID = id });
        }

        public async Task<Store> CloseStore(Store store)
        {
            Store result = new Store();

            var sql = $@"UPDATE [Store] SET IsOpen = 0 WHERE StoreID = @StoreID";
            await _repositoryNgPinas.ExecuteAsync(sql, new { StoreID = store.StoreID });
            var sql1 = $@"SELECT * FROM [Store] WHERE StoreID = @StoreID";
            result = await _repositoryNgPinas.QuerySingleAsync<Store>(sql1, new { StoreID = store.StoreID });

            return result;
        }

        public async Task<Store> GetLastOpenStore()
        {
            var sql = $@"SELECT TOP 1 * FROM [Store] WHERE IsOpen = 1 ORDER BY StoreID DESC";
            return await _repositoryNgPinas.QuerySingleAsync<Store>(sql);
        }

        public async Task<Result> AddProduct(StoreProduct product)
        {
            Result result = new Result();

            var existingStoreProductQuery = $@"SELECT TOP 1 * FROM StoreProduct WHERE [ProductID] = @ProductID AND [StoreID] = @StoreID";
            var existingStoreProduct = await _repositoryNgPinas.QuerySingleAsync<StoreProduct>(existingStoreProductQuery, new { ProductID = product.ProductID, StoreID = product.StoreID });

            if (existingStoreProduct != null)
            {
                var sql = $@"UPDATE StoreProduct SET [Quantity] = @Quantity, [UnitsLeft] = @UnitsLeft WHERE StoreProductID = @StoreProductID";
                var r = await _repositoryNgPinas.ExecuteAsync(sql, new { Quantity = existingStoreProduct.Quantity + product.Quantity, UnitsLeft = existingStoreProduct.UnitsLeft + product.Quantity, StoreProductID = existingStoreProduct.StoreProductID });
                result.IsSuccess = r > 0;
            }
            else
            {
                result = await _repositoryNgPinas.Insert("StoreProduct", product, "StoreProductID");
            }

            return result;
        }

        public async Task<Result> UpdateProductQuantity(int storeProductID, int quantity)
        {
            Result result = new Result();
            var sql = $@"UPDATE StoreProduct SET [Quantity] = @Quantity WHERE StoreProductID = @StoreProductID";
            var r = await _repositoryNgPinas.ExecuteAsync(sql, new { Quantity = quantity, StoreProductID = storeProductID });
            result.IsSuccess = r > 0;
            return result;
        }

        public async Task<Result> DecreaseProductLeft(int storeID, List<SaleProduct> saleProducts)
        {
            Result result = new Result();

            foreach (var saleProduct in saleProducts)
            {
                var sql = $@"UPDATE StoreProduct SET [UnitsLeft] = [UnitsLeft] - @Quantity, UnitsSold = UnitsSold + @Quantity WHERE StoreID = @StoreID AND ProductID = @ProductID";
                var r = await _repositoryNgPinas.ExecuteAsync(sql, new { Quantity = saleProduct.Quantity, StoreID = storeID, ProductID = saleProduct.ProductID });
                result.IsSuccess = r > 0;
            }

            return result;
        }

        public async Task<Result> RefillStoreProduct(int storeProductID, int quantityToBeAdded)
        {
            Result result = new Result();

            var storeProductQuery = $@"SELECT TOP * FROM StoreProduct WHERE StoreProductID = @StoreProductID";
            var storeProduct = await _repositoryNgPinas.QuerySingleAsync<StoreProduct>(storeProductQuery, new { StoreProductID = storeProductID });
            if (storeProduct != null)
            {
                var quantity = Convert.ToInt32(quantityToBeAdded) + Convert.ToInt32(storeProduct.Quantity);
                var left = Convert.ToInt32(quantityToBeAdded) + Convert.ToInt32(storeProduct.UnitsLeft);

                var sql = $@"UPDATE StoreProduct SET [Quantity] = @Quantity, UnitsLeft = @UnitsLeft WHERE StoreProductID = @StoreProductID";
                var r = await _repositoryNgPinas.ExecuteAsync(sql, new { Quantity = quantity, UnitsLeft = left, StoreProductID = storeProductID });
                result.IsSuccess = r > 0;
            }

            return result;
        }

        public StoreProduct GetByStoreProductID(int id)
        {
            return _repositoryNgPinas.Select<StoreProduct>("StoreProduct", "StoreProductID", id);
        }

        public async Task<List<ProductDTO>> GetStoreProducts(int storeID)
        {
            List<ProductDTO> result = new List<ProductDTO>();

            try
            {
                string sql = $@"SELECT
                                    *,
                                    CONVERT(nvarchar(100), p.SalesPrice) AS SalesPriceStr,
                                    CONVERT(nvarchar(100), p.Cost) AS CostStr,
                                    CONVERT(nvarchar(100), p.MarkUp) AS MarkUpStr,
                                    c.CategoryName AS Category,
                                    b.BrandName AS Brand

                                FROM
                                    Product p
                                    LEFT JOIN Brand b
                                    ON p.BrandID = b.BrandID
                                    LEFT JOIN Category c
                                    ON p.CategoryID = c.CategoryID
                                    INNER JOIN StoreProduct sp
                                    ON p.ProductID = sp.ProductID
                                WHERE
                                    p.IsDeleted = 0
                                    AND sp.StoreID = @StoreID
                                ORDER BY p.[Name]";

                result = await _repositoryNgPinas.QueryMultipleAsync<ProductDTO>(sql, new { StoreID = storeID });

                var productAuditSql =
                    $@"SELECT ProductID, SUM(Quantity) AS Quantity FROM ProductAudit GROUP BY ProductID ";
                var productQuantities = await _repositoryNgPinas.QueryMultipleAsync<ProductAudit>(productAuditSql);
                foreach (var r in result)
                {
                    var p = productQuantities.FirstOrDefault(m => m.ProductID == r.ProductID);
                    if (p != null) r.AvailableStockStr = p.Quantity.ToString();
                }
            }
            catch (Exception ex)
            {
                result = new List<ProductDTO>();
            }

            return result;
        }

        public async Task<List<StoreSummaryDTO>> GetStoresAll()
        {
            string sql = $@"select
                                store.storeid,
                                store.DateOpen,
                                [user].FirstName,
                                [user].LastName,
                                SUM(sale.total) AS TotalSales,
                                SUM(sale.Total) - SUM(sp.ProductCost * sp.Quantity) as NetSales
                            from
                                store inner join [user]
                                on store.userid = [user].userid
                                left join sale
                                on sale.storeid = store.storeid
                                left join saleproduct sp
                                on sale.saleid = sp.saleid
                            group by 
                                store.storeid, 
                                store.DateOpen,
                                FirstName,
                                LastName
                            order by
                                store.DateOpen DESC";
            return await _repositoryNgPinas.QueryMultipleAsync<StoreSummaryDTO>(sql);
        }

        public async Task<List<StoreSummaryDTO>> GetStoresByDateRange(DateTime? from, DateTime? to)
        {
            string sql = $@"select
	                            store.storeid,
	                            store.DateOpen,
	                            [user].FirstName,
	                            [user].LastName,
	                            SUM(sale.total) AS TotalSales,
                                SUM(sale.Total) - SUM(sp.ProductCost * sp.Quantity) as NetSales
                            from
	                            store inner join [user]
	                            on store.userid = [user].userid
	                            left join sale
	                            on sale.storeid = store.storeid
                                left join saleproduct sp
                                on sale.saleid = sp.saleid  ";
            if (from != null && to != null)
            {
                sql += $@" where  store.DateOpen between '{ Convert.ToDateTime(from).ToString("yyyy-MM-dd 00:00:00") }' and '{ Convert.ToDateTime(to).ToString("yyyy-MM-dd 23:59:59") }'";
            }
            sql += $@" group by 
	                            store.storeid, 
	                            store.DateOpen,
	                            FirstName,
	                            LastName
                            order by
	                            store.DateOpen DESC";
            return await _repositoryNgPinas.QueryMultipleAsync<StoreSummaryDTO>(sql);
        }
    }
}
