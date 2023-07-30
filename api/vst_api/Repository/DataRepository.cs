using BsonData;
using Model;
using vst_api.Contracts;

namespace vst_api.Repository
{
    public class DataRepository : GenericRepository<ModelS10>, IDataRepository
    {
        public DataRepository(Collection collection) : base("")
        {
        }
    }
}
