using System;
using System.Text;

namespace Omipay.Core
{
    public class ExceptionHelper 
    {
        public static string GetErrorMessage( Exception e , string msg = null )
        {
            StringBuilder builder = new StringBuilder();
            if(msg != null)
            {
                builder.Append( msg );
            }
            builder.Append( e.Message );
            builder.AppendLine();
            for( Exception exception = e.InnerException ; exception != null ; exception = exception.InnerException )
            {
                builder.AppendLine( exception.Message );
            }
            builder.AppendLine( e.StackTrace );
            return builder.ToString();
        }
    }
}
