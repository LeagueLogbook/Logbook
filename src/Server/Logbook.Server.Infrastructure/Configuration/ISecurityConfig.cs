namespace Logbook.Server.Infrastructure.Configuration
{
    public interface ISecurityConfig
    {
        string MicrosoftClientId { get; }
        string MicrosoftClientSecret { get; }

        string FacebookAppId { get; }
        string FacebookAppSecret { get; }

        string GoogleClientId { get; }
        string GoogleClientSecret { get; }

        string TwitterConsumerKey { get; }
        string TwitterConsumerSecret { get; }

        int IterationCountForPasswordHashing { get; }

        string ConfirmEmailKeyPhrase { get; }
        string PasswordResetKeyPhrase { get; }
        string AuthenticationKeyPhrase { get; }
        string TwitterLoginKeyPhrase { get; }

        int LoginIsValidForMinutes { get; }
        int ConfirmEmailIsValidForMinutes { get; }
        int PasswordResetIsValidForMinutes { get; }

        int PasswordResetNewPasswordLength { get; }
    }
}