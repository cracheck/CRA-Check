
namespace CRA_Check.Data
{
    /// <summary>
    /// Database manager.
    /// </summary>
    public class DatabaseManager
    {
        /// <summary>
        /// Database filename
        /// </summary>
        public string DatabaseFilename { get; private set; }

        /// <summary>
        /// Get database context
        /// </summary>
        /// <returns>DbContext</returns>
        public DbContext GetContext()
        {
            return new DbContext(DatabaseFilename);
        }

        /// <summary>
        /// Change the database. Open a new one.
        /// </summary>
        /// <param name="databaseFilename">New database filename</param>
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
