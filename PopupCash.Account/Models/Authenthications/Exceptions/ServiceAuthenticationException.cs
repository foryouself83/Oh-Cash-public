namespace PopupCash.Account.Models.Authenthications.Exceptions
{
    public class ServiceAuthenticationException : Exception
    {
        public string Content { get; }

        public ServiceAuthenticationException()
        {
            Content = string.Empty;
        }

        public ServiceAuthenticationException(string content)
        {
            Content = content;
        }
    }
}
