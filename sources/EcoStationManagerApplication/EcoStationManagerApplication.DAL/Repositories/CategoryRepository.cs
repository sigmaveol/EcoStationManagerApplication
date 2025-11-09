using Dapper;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IDatabaseHelper databaseHelper)
            : base(databaseHelper, "Categories", "category_id")
        { }

        public async Task<IEnumerable<Category>> GetByTypeAsync(CategoryType? categoryType)
        {
            try
            {
                var sql = "SELECT * FROM Categories WHERE is_active = TRUE";
                var parameters = new DynamicParameters();

                if (categoryType.HasValue)
                {
                    sql += " AND category_type = @CategoryType";
                    parameters.Add("CategoryType", categoryType.Value.ToString()); // enum → string
                }

                sql += " ORDER BY name";

                return await _databaseHelper.QueryAsync<Category>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByTypeAsync error - Type: {categoryType} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            try
            {
                var sql = "SELECT * FROM Categories WHERE is_active = TRUE ORDER BY name";
                return await _databaseHelper.QueryAsync<Category>(sql);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetActiveCategoriesAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> IsNameExistsAsync(string name, int? excludeCategoryId = null)
        {
            try
            {
                var sql = "SELECT 1 FROM Categories WHERE name = @Name";
                var parameters = new DynamicParameters();
                parameters.Add("Name", name);

                if (excludeCategoryId.HasValue)
                {
                    sql += " AND category_id != @ExcludeCategoryId";
                    parameters.Add("ExcludeCategoryId", excludeCategoryId.Value);
                }

                var result = await _databaseHelper.ExecuteScalarAsync<int?>(sql, parameters);
                return result.HasValue;
            }
            catch (Exception ex)
            {
                _logger.Error($"IsNameExistsAsync error - Name: {name}, ExcludeId: {excludeCategoryId} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ToggleActiveAsync(int categoryId, bool isActive)
        {
            try
            {
                var sql = "UPDATE Categories SET is_active = @IsActive WHERE category_id = @CategoryId";
                var affectedRows = await _databaseHelper.ExecuteAsync(sql, new
                {
                    CategoryId = categoryId,
                    IsActive = isActive
                });

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"ToggleActiveAsync error - CategoryId: {categoryId}, IsActive: {isActive} - {ex.Message}");
                throw;
            }
        }

        public async Task<int> CountProductsInCategoryAsync(int categoryId)
        {
            try
            {
                var sql = "SELECT COUNT(*) FROM Products WHERE category_id = @CategoryId AND is_active = TRUE";
                return await _databaseHelper.ExecuteScalarAsync<int>(sql, new { CategoryId = categoryId });
            }
            catch (Exception ex)
            {
                _logger.Error($"CountProductsInCategoryAsync error - CategoryId: {categoryId} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Category>> SearchAsync(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return await GetActiveCategoriesAsync();

                var sql = @"
                    SELECT * FROM Categories 
                    WHERE is_active = TRUE 
                    AND (name LIKE @Keyword OR category_type LIKE @Keyword)
                    ORDER BY name";

                return await _databaseHelper.QueryAsync<Category>(sql, new { Keyword = $"%{keyword}%" });
            }
            catch (Exception ex)
            {
                _logger.Error($"SearchAsync error - Keyword: {keyword} - {ex.Message}");
                throw;
            }
        }

    }
}
