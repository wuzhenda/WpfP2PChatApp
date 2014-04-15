using DataContract;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlDatabase
{
    public class DbHandler : IDisposable
    {
        #region Declaration(s)
        private const string _dbPath = @"db";
        #endregion

        #region Property(s)
        private static DbHandler _instance;
        public static DbHandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DbHandler();
                }
                return _instance;
            }
        }
        private Lazy<IDocumentStore> DocStore = new Lazy<IDocumentStore>(() =>
        {
            var documentStore = new EmbeddableDocumentStore()
            {
                DataDirectory = _dbPath
            };
            documentStore.Initialize();
            return documentStore;
        });
        private IDocumentStore DocumentStore
        {
            get { return DocStore.Value; }
        }
        #endregion

        #region Constructor(s)
        public DbHandler()
        {
            IndexCreation.CreateIndexes(typeof(UserMapReduceIndex).Assembly, DocumentStore);
            IndexCreation.CreateIndexes(typeof(CityMapReduceIndex).Assembly, DocumentStore);
        }
        ~DbHandler()
        {
            Dispose(false);
        }
        #endregion

        #region Method(s)
        public void SaveUserData(List<User> userCollection)
        {
            try
            {
                var stopWatch = Stopwatch.StartNew();
                using (var bulkInsert = DocumentStore.BulkInsert())
                {
                    userCollection.AsParallel().ForAll(p => bulkInsert.Store(p));
                }
                stopWatch.Stop();
                Console.WriteLine("Time elapsed: {0} milliseconds", stopWatch.ElapsedMilliseconds);
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
            }
        }
        public void SaveUserData(User user)
        {
            try
            {
                var stopWatch = Stopwatch.StartNew();
                using (var session = DocumentStore.OpenSession())
                {
                    session.Store(user);
                    session.SaveChanges();
                }
                stopWatch.Stop();
                Console.WriteLine("Time elapsed: {0} milliseconds", stopWatch.ElapsedMilliseconds);
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
            }
        }
        public void GetUserData()
        {
            try
            {
                using (var session = DocumentStore.OpenSession())
                {
                    RavenQueryStatistics statistics = new RavenQueryStatistics();
                    var response = session.Advanced.LuceneQuery<User>("UserMapReduceIndex").Where("Username: " + "Rabbi102").WaitForNonStaleResultsAsOfLastWrite().Statistics(out statistics).ToList();
                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
            }
        }
        public bool AuthenticateUser(string username, string password)
        {
            try
            {
                using (var session = DocumentStore.OpenSession()) 
                {
                    var response = session.Advanced.LuceneQuery<User>("UserMapReduceIndex").Where("Username: " + username).WaitForNonStaleResultsAsOfLastWrite().ToList();
                    return response != null && response.Any(p => p.Password.Equals(password));
                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
            }
            return false;
        }
        public static void ShutDownDatabase() 
        {
            if (_instance != null)
            {
                _instance.DocumentStore.Dispose();
                _instance.Dispose();
                _instance = null;
            }
        }
        #endregion

        #region IDisposeable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
        }
        #endregion
    }
}
