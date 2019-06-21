using System;
using NHibernate;
using NHibernate.Cfg;

namespace VS_LOAN.Core.Nhibernate
{
    public class LOANSessionManager
    {
        private readonly ISessionFactory sessionFactory;
        public static ISessionFactory SessionFactory
        {
            get { return Instance.sessionFactory; }
        }

        private ISessionFactory GetSessionFactory()
        {
            return sessionFactory;
        }

        public static LOANSessionManager Instance
        {
            get
            {
                return NestedSessionManager.sessionManager;
            }
        }

        public static ISession OpenSession()
        {
            return Instance.GetSessionFactory().OpenSession();
        }

        public static ISession CurrentSession
        {
            get
            {
                return Instance.GetSessionFactory().GetCurrentSession();
            }
        }

        public static ISession NewIndependentSession
        {
            get { return Instance.GetSessionFactory().OpenSession(); }
        }

        private LOANSessionManager()
        {
            try
            {
                //AppDomain.CurrentDomain.RelativeSearchPath web
                //AppDomain.CurrentDomain.BaseDirectory win
                var cfg = new Configuration();
                cfg.Configure(System.IO.Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath, DBConfig.DB_LOAN));
                string connectionString = cfg.GetProperty("connection.connection_string");
                string[] tempArray = connectionString.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                connectionString = "";
                for (int i = 0; i < tempArray.Length; i++)
                {
                    if (tempArray[i].Contains("Password"))
                    {
                        string password = tempArray[i].Substring(tempArray[i].IndexOf('=') + 1).TrimStart();
                        password = TripleDESProcessor.DecryptTripleDES("!123@", password);
                        //connectionString += "Password=" + password + ";";
                        connectionString += "Password=number8;";
                    }
                    else
                        connectionString += tempArray[i] + ";";
                }

                cfg.SetProperty("connection.connection_string", connectionString);
                //cfg.Configure();
                sessionFactory = cfg.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        class NestedSessionManager
        {
            internal static readonly LOANSessionManager sessionManager =
                new LOANSessionManager();
        }
    }
}
