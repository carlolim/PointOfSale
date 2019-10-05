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
    public class ProductManager
    {
        RepositoryNgPinas _repositoryNgPinas = new RepositoryNgPinas();

        public async Task<Result> Add(ProductDTO product)
        {
            Result result = new Result() { IsSuccess = true };

            decimal outDecimal = 0;
            int outInt = 0;

            try
            {
                #region Validate required fields
                if (string.IsNullOrWhiteSpace(product.Name))
                {
                    result.IsSuccess = false;
                    result.Message = "Product name is required.";
                }
                else if (string.IsNullOrWhiteSpace(product.Code) && !product.AutomaticCode)
                {
                    result.IsSuccess = false;
                    result.Message = "Product code is required.";
                }
                else if (string.IsNullOrWhiteSpace(product.AvailableStockStr))
                {
                    result.IsSuccess = false;
                    result.Message = "Available stock is required.";
                }
                else if (!decimal.TryParse(product.AvailableStockStr, out outDecimal))
                {
                    result.IsSuccess = false;
                    result.Message = "Invalid available stock value.";
                }
                else if (!decimal.TryParse(product.SalesPriceStr, out outDecimal))
                {
                    result.IsSuccess = false;
                    result.Message = "Invalid sales price value.";
                }
                else if (!decimal.TryParse(product.CostStr, out outDecimal))
                {
                    result.IsSuccess = false;
                    result.Message = "Invalid cost value.";
                }

                if (result.IsSuccess)
                {
                    if (product.AutomaticSalesPrice && string.IsNullOrWhiteSpace(product.MarkUpStr))
                    {
                        result.IsSuccess = false;
                        result.Message = "Markup % is required.";
                    }
                    else if (product.AutomaticSalesPrice)
                    {
                        if (!decimal.TryParse(product.MarkUpStr, out outDecimal))
                        {
                            result.IsSuccess = false;
                            result.Message = "Invalid Markup % value.";
                        }
                    }
                }
                #endregion

                if (result.IsSuccess)
                {
                    Product productToBeAdded = new Product();

                    //productToBeAdded.AvailableStock = Convert.ToInt32(product.AvailableStockStr);
                    productToBeAdded.BrandID = string.IsNullOrWhiteSpace(product.Brand) ? 0 : Convert.ToInt32(product.Brand);
                    productToBeAdded.CategoryID = string.IsNullOrWhiteSpace(product.Category) ? 0 : Convert.ToInt32(product.Category);
                    productToBeAdded.Code = product.Code;
                    productToBeAdded.Cost = Convert.ToDecimal(product.CostStr);
                    productToBeAdded.DateCreated = DateTime.Now;
                    productToBeAdded.IsDeleted = false;
                    productToBeAdded.MarkUp = string.IsNullOrWhiteSpace(product.MarkUpStr) ? 0.0 : Convert.ToDouble(product.MarkUpStr);
                    productToBeAdded.Name = product.Name;
                    productToBeAdded.SalesPrice = Convert.ToDecimal(product.SalesPriceStr);

                    result = await _repositoryNgPinas.Insert("Product", productToBeAdded, "ProductID");
                    
                }
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "An error occurred, please try again later.";
            }

            return result;
        }

        public async Task<Result> Update(ProductDTO product)
        {
            Result result = new Result() { IsSuccess = true };

            double outDouble = 0.0;
            int outInt = 0;

            try
            {
                #region Validate required fields
                if (string.IsNullOrWhiteSpace(product.Name))
                {
                    result.IsSuccess = false;
                    result.Message = "Product name is required.";
                }
                else if (string.IsNullOrWhiteSpace(product.Code) && !product.AutomaticCode)
                {
                    result.IsSuccess = false;
                    result.Message = "Product code is required.";
                }
                //else if (string.IsNullOrWhiteSpace(product.AvailableStockStr))
                //{
                //    result.IsSuccess = false;
                //    result.Message = "Available stock is required.";
                //}
                //else if (!int.TryParse(product.AvailableStockStr, out outInt))
                //{
                //    result.IsSuccess = false;
                //    result.Message = "Invalid available stock value.";
                //}
                else if (!double.TryParse(product.SalesPriceStr, out outDouble))
                {
                    result.IsSuccess = false;
                    result.Message = "Invalid sales price value.";
                }
                else if (!double.TryParse(product.CostStr, out outDouble))
                {
                    result.IsSuccess = false;
                    result.Message = "Invalid cost value.";
                }

                if (result.IsSuccess)
                {
                    if (product.AutomaticSalesPrice && string.IsNullOrWhiteSpace(product.MarkUpStr))
                    {
                        result.IsSuccess = false;
                        result.Message = "Markup % is required.";
                    }
                    else if (product.AutomaticSalesPrice)
                    {
                        if (!double.TryParse(product.MarkUpStr, out outDouble))
                        {
                            result.IsSuccess = false;
                            result.Message = "Invalid Markup % value.";
                        }
                    }
                }
                #endregion

                if (result.IsSuccess)
                {
                    Product productToBeUpdated = new Product();

                    productToBeUpdated.ProductID = product.ProductID;
                    //productToBeUpdated.AvailableStock = Convert.ToInt32(product.AvailableStockStr);
                    productToBeUpdated.BrandID = string.IsNullOrWhiteSpace(product.Brand) ? 0 : Convert.ToInt32(product.Brand);
                    productToBeUpdated.CategoryID = string.IsNullOrWhiteSpace(product.Category) ? 0 : Convert.ToInt32(product.Category);
                    productToBeUpdated.Code = product.Code;
                    productToBeUpdated.Cost = Convert.ToDecimal(product.CostStr);
                    productToBeUpdated.DateCreated = DateTime.Now;
                    productToBeUpdated.IsDeleted = false;
                    productToBeUpdated.MarkUp = string.IsNullOrWhiteSpace(product.MarkUpStr) ? 0.0 : Convert.ToDouble(product.MarkUpStr);
                    productToBeUpdated.Name = product.Name;
                    productToBeUpdated.SalesPrice = Convert.ToDecimal(product.SalesPriceStr);
                    
                    result = await _repositoryNgPinas.Update("Product", productToBeUpdated, new List<string>() { "ProductID" }, new List<object>() { product.ProductID }, "ProductID");
                    
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "An error occurred, please try again later.";
            }

            return result;
        }

        public async Task<List<ProductDTO>> GetListDTOAsync()
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
                                WHERE
                                    p.IsDeleted = 0
                                ORDER BY p.[Name]";

                result = await _repositoryNgPinas.QueryMultipleAsync<ProductDTO>(sql);

                var productAuditSql =
                    $@"SELECT ProductID, SUM(Quantity) AS Quantity FROM ProductAudit GROUP BY ProductID ";
                var productQuantities = await _repositoryNgPinas.QueryMultipleAsync<ProductAudit>(productAuditSql);
                foreach (var r in result)
                {
                    var p = productQuantities.FirstOrDefault(m => m.ProductID == r.ProductID);
                    if (p != null)
                    {
                        r.AvailableStockStr = p.Quantity.ToString();
                        r.AvailableStock = p.Quantity;
                    }
                }
            }
            catch(Exception ex)
            {
                result = new List<ProductDTO>();
            }

            return result;
        }

        public async Task<List<ProductDTO>> SearchByNameOrCodeAsync(string str)
        {
            List<ProductDTO> result = new List<ProductDTO>();

            try
            {
                string sql = $@"
                                SELECT
	                                *,
	                                CONVERT(nvarchar(100), p.AvailableStock) AS AvailableStockStr,
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
                                WHERE
	                                p.[Name] LIKE '%' + @str + '%'
	                                OR p.Code LIKE '%' + @str + '%'
                                ORDER BY p.[Name]";
                var param = new { str = str };

                result = await _repositoryNgPinas.QueryMultipleAsync<ProductDTO>(sql, param);
            }
            catch (Exception ex)
            {
                result = new List<ProductDTO>();
            }

            return result;
        }

        public async Task<ProductDTO> GetDTOByIDAsync(int id)
        {
            string sql = $@"SELECT TOP 1
                                p.ProductID,
                                p.Code,
                                p.Name,
                                CONVERT(nvarchar(100), SUM(pa.Quantity)) AS AvailableStockStr,
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
                                INNER JOIN ProductAudit pa
                                ON p.ProductID = pa.ProductID
                            WHERE
                                p.ProductID = @ProductID
                            GROUP BY
	                            p.SalesPrice, 
	                            p.Cost, 
	                            p.MarkUp, 
	                            c.CategoryName, 
	                            b.BrandName,
	                            p.ProductID,
                                p.Code,
                                p.Name";
            return await _repositoryNgPinas.QuerySingleAsync<ProductDTO>(sql, new { ProductID = id });
        }

        public async Task<Result> Update(Product product)
        {
            return await _repositoryNgPinas.Update("Product", product, new List<string>() { "ProductID" }, new List<object>() { product.ProductID }, "ProductID");
        }

        public async Task<Result> Delete(int productid)
        {
            Result result = new Result();
            var sql = $@"UPDATE Product SET IsDeleted = 1 WHERE ProductID = @ProductID";
            var r = await _repositoryNgPinas.ExecuteAsync(sql, new { ProductID = productid });
            result.IsSuccess = r > 0;
            return result;
        }

        public async Task<Result> UpdateQuantity(int productID, decimal quantity, int userCreatedID)
        {
            Result result = new Result();
            var currentQuantityQuery =
                $@"SELECT SUM(Quantity) AS Quantity FROM ProductAudit WHERE ProductID=@ProductID";
            var currentQuantity =
                await _repositoryNgPinas.QuerySingleAsync<decimal>(currentQuantityQuery, new {ProductID = productID});

            var tobeInsertedValue = quantity - currentQuantity;
            var sql = $@"INSERT INTO ProductAudit 
                         (ProductID, Quantity, DateCreated, UserCreatedID)
                         VALUES (@ProductID, @Quantity, GETDATE(), @UserCreatedID)";
            var r = await _repositoryNgPinas.ExecuteAsync(sql, new { Quantity = tobeInsertedValue, ProductID = productID, UserCreatedID = userCreatedID });
            //var sql = $@"UPDATE [Product] SET AvailableStock = @Quantity WHERE ProductID = @ProductID";
            //var r = await _repositoryNgPinas.ExecuteAsync(sql, new { Quantity = quantity, ProductID = productID });
            result.IsSuccess = r > 0;
            return result;
        }

        public async Task<List<ProductDTO>> GetProductsByStoreID(int storeID)
        {
            List<ProductDTO> result = new List<ProductDTO>();

            var sql = $@"SELECT
	                        p.ProductID,
	                        p.Name,
	                        CASE WHEN p.Code IS NULL THEN '' ELSE p.Code END AS Code,
	                        SUM(pa.Quantity) as AvailableStock,
                            CASE 
    	                        WHEN InStore.Quantity IS NULL THEN 0 
    	                        ELSE InStore.Quantity 
                            END AS InStore,
                            CASE 
    	                        WHEN SUM(pa.Quantity) IS NULL THEN 0 
    	                        ELSE SUM(pa.Quantity)
                            END + 
                            CASE 
    	                        WHEN InStore.Quantity IS NULL THEN 0 
    	                        ELSE InStore.Quantity 
                            END AS TotalQuantity,
                            p.SalesPrice,
                            p.Cost,
                            c.CategoryName as Category,
                            b.BrandName as Brand
                        FROM
	                        Product p
	                        INNER JOIN ProductAudit pa
	                        ON p.ProductID = pa.ProductID
	                        LEFT JOIN Category c
	                        ON p.CategoryID = c.CategoryID
	                        LEFT JOIN Brand b
	                        ON p.BrandID = b.BrandID
	                        LEFT JOIN 
	                        (
		                        SELECT
			                        ProductID,
			                        CASE
				                        WHEN Quantity IS NULL THEN 0
				                        ELSE Quantity
			                        END as Quantity
		                        FROM
			                        StoreProduct
		                        WHERE
			                        StoreID = @StoreID
	                        ) as InStore
	                        ON p.ProductID = InStore.ProductID
                        WHERE
	                        p.IsDeleted = 0
                        GROUP BY
	                        p.Name, p.ProductID, p.Code, InStore.Quantity, p.SalesPrice, p.Cost, c.CategoryName, b.BrandName
                        ORDER BY
	                        p.[Name]";

            result = await _repositoryNgPinas.QueryMultipleAsync<ProductDTO>(sql, new { StoreID = storeID });

            return result;
        }
    }
}
