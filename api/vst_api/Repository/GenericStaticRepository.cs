﻿using share;
using vst_api.Contracts;
using vst_api.Data;

namespace vst_api.Repository
{
    public class GenericStaticRepository<T> : IGenericStaticRepository<T> where T : Document, new()
    {
        readonly DynamicDB _dB;
        string _name = typeof(T).ToString();


        public GenericStaticRepository()
        {
            _dB = DB.Context;
        }

        public T Add(T entity)
        {
            _dB.GetCollection(_name).Insert(entity);
            return entity;
        }

        public void Delete(string key)
        {
            var exist = Get(key);
            if (exist is not null)
                _dB.GetCollection(_name).Delete(exist);
        }

        public bool Exist(string key)
        {
            var exist = _dB.GetCollection(_name).Find(key);
            if (exist is not null)
                return true;
            return false;
        }

        public T? Get(string? key)
        {
            if (key is not null)
            {
                var exist = _dB.GetCollection(_name).Find(key);
                return exist.ChangeType<T>();
            }

            return null;
        }

        public List<T> GetAll()
        {
            var all = _dB.GetCollection(_name).Select().Cast<T>().ToList();
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
            _dB.GetCollection(_name).FindAndUpdate(entity.ObjectId, doc =>
            {
                doc = entity;
            });
        }
    }
}
