using System.Collections.Generic;

namespace TaskSample.Domain
{
    public class DataResult<TEntity> where TEntity : class
    {
        public IEnumerable<TEntity> FilteredRecords { get; set; }
        public int TotalRecords { get; set; }
    }
}
