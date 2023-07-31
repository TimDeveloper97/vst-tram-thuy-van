using BsonData;
using share;
using vst_api.Contracts;
using vst_api.Data;

namespace vst_api.Repository
{
    public class GenericDynamicRepository<T> : IGenericDynamicRepository<T> where T : Document, new()
    {
        readonly SequenceDB _dB;

        public GenericDynamicRepository(string database)
        {
            switch (database)
            {
                case "S10":
                    _dB = DB.S10;
                    break;
                case "SEQ":
                    _dB = DB.SEQ;
                    break;
            }

        }

        public T Add(T entity)
        {
            _dB.Current.Insert(entity);
            return entity;
        }

        public void Delete(string key)
        {
            var exist = Get(key);
            if (exist is not null)
                _dB.Current.Delete(exist);
        }

        public bool Exist(string key)
        {
            var exist = _dB.Current.Find(key);
            if (exist is not null)
                return true;
            return false;
        }

        public T? Get(string? key)
        {
            if(key is not null)
            {
                var exist = _dB.Current.Find(key);
                return exist.ChangeType<T>();
            }

            return null;
        }

        public List<T> GetAll()
        {
            var all = _dB.Current.Select().Cast<T>().ToList();
            return all;
        }

        public PagedResult<T> GetAll(QueryParameters queryParameters)
        {
            var totalSize = GetAll().Count;
            var items = GetAll()
                .Skip(queryParameters.StartIndex)
                .Take(queryParameters.PageSize)
                .ToList();

            return new PagedResult<T>
            {
                Items = items,
                PageNumber = queryParameters.PageNumber,
                RecordNumber = queryParameters.PageSize,
                TotalCount = totalSize
            };
        }

        public void Update(T entity)
        {
            _dB.Current.FindAndUpdate(entity.ObjectId, doc =>
            {
                doc = entity;
            });
        }
    }
}
