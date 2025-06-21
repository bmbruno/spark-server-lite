namespace SparkServerLite.Interfaces
{
    public interface IAnalyticsRepository<T>
    {
        public T Get(int ID);

        public IEnumerable<T> GetAll();
        
        void Create(T newItem);
    }
}
