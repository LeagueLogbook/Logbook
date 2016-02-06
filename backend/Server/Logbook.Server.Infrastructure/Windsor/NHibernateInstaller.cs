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
            var factory = Fluently.Configure()
                .Mappings(f => f.FluentMappings.AddFromAssembly(this.GetType().Assembly))
                .Database(() => MsSqlConfiguration.MsSql2012.ConnectionString(f => f.Is(Config.SqlServerConnectionString)))
                .ExposeConfiguration(this.TryRecreateDatabase)
                .BuildSessionFactory();

            return factory;
        }

        private void TryRecreateDatabase(Configuration configuration)
        {
            if (Config.RecreateDatabase)
            {
                new SchemaExport(configuration)
                    .Execute(false, true, false);
            }
        }
    }
}