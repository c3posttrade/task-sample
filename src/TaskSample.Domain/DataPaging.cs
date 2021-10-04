namespace TaskSample.Domain
{
    public class DataPaging
    {
        public DataPaging()
        {

        }
        public DataPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
