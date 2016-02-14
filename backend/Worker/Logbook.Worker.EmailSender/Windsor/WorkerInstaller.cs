﻿using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Logbook.Worker.EmailSender.Windsor
{
    public class WorkerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<EmailSenderWorker>().LifestyleSingleton());
        }
    }
}