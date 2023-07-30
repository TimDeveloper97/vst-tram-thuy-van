using BsonData;
using vst_api.Contracts;
using vst_api.Core;
using vst_api.Data;

namespace vst_api.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        readonly DB _dB;
        readonly Collection _collection;

        public GenericRepository(string collection)
        {
            
        }

        public Task<T> AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> AddAsync<TSource, TResult>(TSource source)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<TResult>> GetAllAsync<TResult>()
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<TResult?> GetAsync<TResult>(int? id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync<TSource>(int id, TSource source)
        {
            throw new NotImplementedException();
        }
    }
}
