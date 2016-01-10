using System;
using System.Reflection;
using System.Threading.Tasks;
using Castle.Windsor;
using JetBrains.Annotations;
using LiteGuard;
using Logbook.Server.Contracts.Commands;
using Logbook.Shared.Extensions;
using Metrics;

namespace Logbook.Server.Infrastructure.Commands
{
    public class CommandScope : ICommandScope
    {
        #region Fields
        private readonly Counter _commandsCounter = Metric.Counter("Concurrent Commands", Unit.Calls);

        private readonly IWindsorContainer _container;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandScope"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public CommandScope([NotNull]IWindsorContainer container)
        {
            Guard.AgainstNullArgument(nameof(container), container);

            this._container = container;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="command">The command.</param>
        public async Task<TResult> Execute<TResult>(ICommand<TResult> command)
        {
            Guard.AgainstNullArgument(nameof(command), command);

            this._commandsCounter.Increment();
            
            Type handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
            object actualCommandHandler = this._container.Resolve(handlerType);

            var method = actualCommandHandler.GetType().GetMethod(nameof(ICommandHandler<ICommand<object>, object>.Execute));
            var result = await ((Task<TResult>)method.Invoke(actualCommandHandler, new object[] {command, this})).WithCurrentCulture();

            this._commandsCounter.Decrement();

            return result;
        }
        #endregion
    }
}