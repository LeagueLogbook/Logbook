using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Logbook.Server.Infrastructure.Windsor
{
    public class NHibernateInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var factory = this.CreateSessionFactory();

            container.Register(
                Component.For<ISessionFactory>().Instance(factory).LifestyleSingleton(),
                Component.For<ISession>().UsingFactoryMethod((kernel, context) => kernel.Resolve<ISessionFactory>().OpenSession()).LifestyleScoped());
        }

        private ISessionFactory CreateSessionFactory()
        {
            var connectionString = Config.SqlServerConnectionString.GetValue();

            var factory = Fluently.Configure()
                .Mappings(f => f.FluentMappings.AddFromAssembly(this.GetType().Assembly))
                .Database(() => MsSqlConfiguration.MsSql2012.ShowSql().FormatSql().ConnectionString(connectionString))
                .ExposeConfiguration(this.WorkWithConfiguration)
                .BuildSessionFactory();

            return factory;
        }

        private void WorkWithConfiguration(Configuration configuration)
        {
            if (Config.RecreateDatabase)
            {
                new SchemaExport(configuration)
                    .Execute(false, true, false);
            }

            if (Config.UpdateDatabase)
            {
                new SchemaUpdate(configuration)
                    .Execute(false, true);
            }
        }
    }
}