
namespace CRA_Check.Data
{
    public class DatabaseManager
    {
        public string DatabaseFilename { get; set; }

        public DbContext GetContext()
        {
            return new DbContext(DatabaseFilename);
        }
    }
}
