using Dapper;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.DAL.SqlQueries;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    public class OrderDetailRepository : BaseRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(IDatabaseHelper databaseHelper)
            : base(databaseHelper, "OrderDetails", "order_detail_id")
        {
        }

        public async Task<IEnumerable<OrderDetail>> GetByOrderAsync(int orderId)
        {
            try
            {
                return await _databaseHelper.QueryAsync<OrderDetail>(
                    OrderDetailQueries.GetByOrder,
                    new { OrderId = orderId }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByOrderAsync error - OrderId: {orderId} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsWithProductsAsync(int orderId)
        {
            try
            {
                return await _databaseHelper.QueryAsync<OrderDetail>(
                    OrderDetailQueries.GetOrderDetailsWithProducts,
                    new { OrderId = orderId }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetOrderDetailsWithProductsAsync error - OrderId: {orderId} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> AddRangeAsync(IEnumerable<OrderDetail> orderDetails)
        {
            using (var connection = await _databaseHelper.CreateConnectionAsync())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var detail in orderDetails)
                        {
                            await connection.ExecuteAsync(
                                OrderDetailQueries.InsertOrderDetail,
                                new
                                {
                                    detail.OrderId,
                                    detail.ProductId,
                                    detail.Quantity,
                                    detail.UnitPrice
                                },
                                transaction
                            );
                        }

                        transaction.Commit();
                        _logger.Info($"Đã thêm {orderDetails.Count()} chi tiết đơn hàng");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.Error($"AddRangeAsync error - {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public async Task<bool> DeleteByOrderAsync(int orderId)
        {
            try
            {
                var affectedRows = await _databaseHelper.ExecuteAsync(
                    OrderDetailQueries.DeleteByOrder,
                    new { OrderId = orderId }
                );

                _logger.Info($"Đã xóa {affectedRows} chi tiết đơn hàng cho OrderId: {orderId}");
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"DeleteByOrderAsync error - OrderId: {orderId} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateRangeAsync(IEnumerable<OrderDetail> orderDetails)
        {
            using (var connection = await _databaseHelper.CreateConnectionAsync())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var detail in orderDetails)
                        {
                            await connection.ExecuteAsync(
                                OrderDetailQueries.UpdateOrderDetail,
                                new
                                {
                                    detail.OrderDetailId,
                                    detail.ProductId,
                                    detail.Quantity,
                                    detail.UnitPrice
                                },
                                transaction
                            );
                        }

                        transaction.Commit();
                        _logger.Info($"Đã cập nhật {orderDetails.Count()} chi tiết đơn hàng");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.Error($"UpdateRangeAsync error - {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public async Task<bool> UpdateQuantityAsync(int orderDetailId, decimal quantity)
        {
            try
            {
                var affectedRows = await _databaseHelper.ExecuteAsync(
                    OrderDetailQueries.UpdateQuantity,
                    new { OrderDetailId = orderDetailId, Quantity = quantity }
                );

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"UpdateQuantityAsync error - OrderDetailId: {orderDetailId}, Quantity: {quantity} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateUnitPriceAsync(int orderDetailId, decimal unitPrice)
        {
            try
            {
                var affectedRows = await _databaseHelper.ExecuteAsync(
                    OrderDetailQueries.UpdateUnitPrice,
                    new { OrderDetailId = orderDetailId, UnitPrice = unitPrice }
                );

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"UpdateUnitPriceAsync error - OrderDetailId: {orderDetailId}, UnitPrice: {unitPrice} - {ex.Message}");
                throw;
            }
        }

        public async Task<decimal> GetTotalSoldQuantityAsync(int productId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var sql = OrderDetailQueries.GetTotalSoldQuantity;
                var parameters = new DynamicParameters();
                parameters.Add("ProductId", productId);

                if (fromDate.HasValue && toDate.HasValue)
                {
                    sql += " AND o.last_updated BETWEEN @FromDate AND @ToDate";
                    parameters.Add("FromDate", fromDate.Value);
                    parameters.Add("ToDate", toDate.Value);
                }

                return await _databaseHelper.ExecuteScalarAsync<decimal>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetTotalSoldQuantityAsync error - ProductId: {productId} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<ProductSales>> GetTopSellingProductsAsync(int limit = 10, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var sql = OrderDetailQueries.GetTopSellingProducts;
                var parameters = new DynamicParameters();

                if (fromDate.HasValue && toDate.HasValue)
                {
                    sql += " AND o.last_updated BETWEEN @FromDate AND @ToDate";
                    parameters.Add("FromDate", fromDate.Value);
                    parameters.Add("ToDate", toDate.Value);
                }

                sql += " GROUP BY p.product_id, p.name, p.sku ORDER BY total_quantity DESC LIMIT @Limit";
                parameters.Add("Limit", limit);

                return await _databaseHelper.QueryAsync<ProductSales>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetTopSellingProductsAsync error - Limit: {limit} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<ProductRevenue>> GetProductRevenueAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var sql = OrderDetailQueries.GetProductRevenue;
                var parameters = new DynamicParameters();

                if (fromDate.HasValue && toDate.HasValue)
                {
                    sql += " AND o.last_updated BETWEEN @FromDate AND @ToDate";
                    parameters.Add("FromDate", fromDate.Value);
                    parameters.Add("ToDate", toDate.Value);
                }

                sql += " GROUP BY p.product_id, p.name ORDER BY total_revenue DESC";

                return await _databaseHelper.QueryAsync<ProductRevenue>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetProductRevenueAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> IsProductInAnyOrderAsync(int productId)
        {
            try
            {
                var result = await _databaseHelper.ExecuteScalarAsync<int?>(
                    OrderDetailQueries.IsProductInAnyOrder,
                    new { ProductId = productId }
                );

                return result.HasValue;
            }
            catch (Exception ex)
            {
                _logger.Error($"IsProductInAnyOrderAsync error - ProductId: {productId} - {ex.Message}");
                throw;
            }
        }
    }
}
