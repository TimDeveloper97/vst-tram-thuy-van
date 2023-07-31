using vst_api.Data;

namespace vst_api.Contracts
{
    public interface IGenericDynamicRepository<T>  where T : Document, new()
    {
        T? Get(string? key);
        List<T> GetAll();
        PagedResult<T> GetAll(QueryParameters queryParameters);
        T Add(T entity);
        void Delete(string key);
        void Update(T entity);
        bool Exist(string key);
    }

    public interface IGenericStaticRepository<T> : IGenericDynamicRepository<T>
        where T : Document, new()
    {

    }    
}
