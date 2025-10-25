using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcoStationManagerApplication.Core.Interfaces;

namespace EcoStationManagerApplication.Core.Services
{
    public abstract class BaseService<T> : IService<T> where T : class
    {
        protected readonly IRepository<T> _repository;

        protected BaseService(IRepository<T> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID phải lớn hơn 0", nameof(id));

                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    throw new KeyNotFoundException($"Không tìm thấy {typeof(T).Name} với ID: {id}");

                return entity;
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Lỗi khi lấy {typeof(T).Name} theo ID: {ex.Message}", ex);
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Lỗi khi lấy danh sách {typeof(T).Name}: {ex.Message}", ex);
            }
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "Entity không được null");

                var createdEntity = await _repository.CreateAsync(entity);
                return createdEntity;
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Lỗi khi tạo {typeof(T).Name}: {ex.Message}", ex);
            }
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "Entity không được null");

                var updatedEntity = await _repository.UpdateAsync(entity);
                return updatedEntity;
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Lỗi khi cập nhật {typeof(T).Name}: {ex.Message}", ex);
            }
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID phải lớn hơn 0", nameof(id));

                var exists = await _repository.GetByIdAsync(id);
                if (exists == null)
                    throw new KeyNotFoundException($"Không tìm thấy {typeof(T).Name} với ID: {id}");

                return await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Lỗi khi xóa {typeof(T).Name}: {ex.Message}", ex);
            }
        }

        public virtual async Task<bool> SoftDeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID phải lớn hơn 0", nameof(id));

                var exists = await _repository.GetByIdAsync(id);
                if (exists == null)
                    throw new KeyNotFoundException($"Không tìm thấy {typeof(T).Name} với ID: {id}");

                return await _repository.SoftDeleteAsync(id);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("does not support soft delete"))
            {
                throw new ServiceException($"{typeof(T).Name} không hỗ trợ xóa mềm", ex);
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Lỗi khi xóa mềm {typeof(T).Name}: {ex.Message}", ex);
            }
        }
    }

    // Custom Exception cho Service Layer
    public class ServiceException : Exception
    {
        public ServiceException(string message) : base(message) { }
        public ServiceException(string message, Exception innerException) : base(message, innerException) { }
    }
}