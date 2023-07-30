namespace vst_api.Data
{
    public class QueryParameters
    {
        private int _pageSize = 15;
        public int PageSize { get { return _pageSize; } set { _pageSize = value; } }
        public int StartIndex { get; set; }
        public int PageNumber { get; set; }
    }
}
