using System.Globalization;

namespace TTechEcommerceApi.Helper
{
    public class TTechException : Exception
    {
        public TTechException() : base() { }

        public TTechException(string message) : base(message) { }

        public TTechException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
