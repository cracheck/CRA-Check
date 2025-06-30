
namespace CRA_Check.Data
{
    public class DatabaseManager
    {
        public string DatabaseFilename { get; private set; }

        public DbContext GetContext()
        {
            return new DbContext(DatabaseFilename);
        }

        public void ChangeDatabase(string databaseFilename)
        {
            DatabaseFilename = databaseFilename;

            using (DbContext dbContext = GetContext())
            {
                dbContext.Database.EnsureCreated();
            }
        }
    }
}
