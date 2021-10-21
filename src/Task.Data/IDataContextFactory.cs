namespace TaskManager.Data
{
    public interface IDataContextFactory
    {
        IDataContext Build();
    }
}
