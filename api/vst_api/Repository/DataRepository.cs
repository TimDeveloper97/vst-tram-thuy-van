using BsonData;
using Model;
using Models.User;
using vst_api.Contracts;

namespace vst_api.Repository
{
    public class DataRepository : GenericDynamicRepository<User>, IDataRepository
    {
        public DataRepository(string database) : base(database)
        {
        }
    }
}
