using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Logbook.Server.Infrastructure.Configuration;
using NHibernate;
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
            var factory = Fluently.Configure()
                .Mappings(f => f.FluentMappings.AddFromAssembly(this.GetType().Assembly))
                .Database(() => MsSqlConfiguration.MsSql2012.ShowSql().FormatSql().ConnectionString(Config.Database.SqlServerConnectionString))
                .ExposeConfiguration(this.WorkWithConfiguration)
                .BuildSessionFactory();

            return factory;
        }

        private void WorkWithConfiguration(global::NHibernate.Cfg.Configuration configuration)
        {
            if (Config.Database.RecreateDatabase)
            {
                new SchemaExport(configuration)
                    .Execute(false, true, false);
            }

            if (Config.Database.UpdateDatabase)
            {
                new SchemaUpdate(configuration)
                    .Execute(false, true);
            }
        }
    }
}