using System;
using System.Linq;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Authentication;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Infrastructure.Configuration;
using Logbook.Server.Infrastructure.Exceptions;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Extensions;

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, object>
    {
        private readonly ISecretGenerator _secretGenerator;
        private readonly ISaltCombiner _saltCombiner;

        public ChangePasswordCommandHandler(ISecretGenerator secretGenerator, ISaltCombiner saltCombiner)
        {
            this._secretGenerator = secretGenerator;
            this._saltCombiner = saltCombiner;
        }

        public async Task<object> Execute(ChangePasswordCommand command, ICommandScope scope)
        {
            var logbookAuthentication = command.User.Authentications
                .OfType<LogbookAuthenticationKind>()
                .FirstOrDefault();
            
            if (logbookAuthentication == null)
            {
                logbookAuthentication = new LogbookAuthenticationKind();

                command.User.Authentications.Add(logbookAuthentication);
                logbookAuthentication.User = command.User;
            }

            var salt = this._secretGenerator.Generate();

            logbookAuthentication.IterationCount = Config.Security.IterationCountForPasswordHashing;
            logbookAuthentication.Salt = salt;
            logbookAuthentication.Hash = this._saltCombiner.Combine(salt, logbookAuthentication.IterationCount, command.NewPasswordSHA256Hash);

            return new object();
        }
    }
}