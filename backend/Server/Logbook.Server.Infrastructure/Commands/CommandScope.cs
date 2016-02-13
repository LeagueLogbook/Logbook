using System;
using System.Reflection;
using System.Threading.Tasks;
using Castle.Windsor;
using JetBrains.Annotations;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Mapping;
using Logbook.Shared;
using Logbook.Shared.Extensions;

namespace Logbook.Server.Infrastructure.Commands
{
    public class CommandScope : ICommandScope
    {
        #region Fields
        private readonly IWindsorContainer _container;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandScope"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public CommandScope(IWindsorContainer container)
        {
            Guard.NotNull(container, nameof(container));

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
            Guard.NotNull(command, nameof(command));
            
            Type handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
            object actualCommandHandler = this._container.Resolve(handlerType);

            var method = actualCommandHandler.GetType().GetMethod(nameof(ICommandHandler<ICommand<object>, object>.Execute));
            var result = await ((Task<TResult>)method.Invoke(actualCommandHandler, new object[] {command, this})).WithCurrentCulture();
            

            return result;
        }
        /// <summary>
        /// Maps the specified source element to a element of type <typeparamref name="TTarget"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source.</param>
        public async Task<TTarget> Map<TSource, TTarget>(TSource source)
        {
            Guard.NotNull(source, nameof(source));

            Type mapperType = typeof (IMapper<,>).MakeGenericType(typeof (TSource), typeof (TTarget));
            var actualMapper = this._container.Resolve(mapperType);

            var method = actualMapper.GetType().GetMethod(nameof(IMapper<object, object>.MapAsync));
            var result = await ((Task<TTarget>)method.Invoke(actualMapper, new object[] {source})).WithCurrentCulture();

            return result;
        }
        #endregion
    }
}