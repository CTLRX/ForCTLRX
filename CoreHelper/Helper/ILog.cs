using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Helper
{
   public  interface ILog
    {
		void Info(string message, object data = null, params string[] tags);
		void Info(Exception ex, object data = null, params string[] tags);
		void Debug(string message, object data = null, params string[] tags);
		void Debug(Exception ex, object data = null, params string[] tags);
		void Error(string message, object data = null, params string[] tags);
		void Error(Exception ex, object data = null, params string[] tags);
	}
}
