namespace Trill.Shared.Infrastructure.Mongo
{
    internal class MongoOptions
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public bool DisableTransactions { get; set; }
    }
}