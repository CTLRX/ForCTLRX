using Common.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Helper
{
   public  class Log
    {
		private static List<ILog> logs;
		private static bool IsDebug;
		private static List<ILog> Instances
		{
			get
			{
				if (Log.logs == null)
				{

				
				}
				return Log.logs;
			}
		}
		static Log()
		{
		}
		public static void Info(string message, params string[] tags)
		{
			using (List<ILog>.Enumerator enumerator = Log.Instances.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.Info(message, null, tags);
				}
			}
		}
		public static void Info(string message, object data, params string[] tags)
		{
			using (List<ILog>.Enumerator enumerator = Log.Instances.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.Info(message, data, tags);
				}
			}
		}
		public static void Info(Exception ex, params string[] tags)
		{
			using (List<ILog>.Enumerator enumerator = Log.Instances.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.Info(ex, null, tags);
				}
			}
		}
		public static void Info(Exception ex, object data, params string[] tags)
		{
			using (List<ILog>.Enumerator enumerator = Log.Instances.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.Info(ex, data, tags);
				}
			}
		}
		public static void Debug(string message, object data, params string[] tags)
		{
			if (Log.IsDebug)
			{
				using (List<ILog>.Enumerator enumerator = Log.Instances.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.Debug(message, data, tags);
					}
				}
			}
		}
		public static void Debug(string message, params string[] tags)
		{
			if (Log.IsDebug)
			{
				using (List<ILog>.Enumerator enumerator = Log.Instances.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.Debug(message, null, tags);
					}
				}
			}
		}
		public static void Debug(Exception ex, object data, params string[] tags)
		{
			if (Log.IsDebug)
			{
				using (List<ILog>.Enumerator enumerator = Log.Instances.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.Debug(ex, data, tags);
					}
				}
			}
		}
		public static void Debug(Exception ex, params string[] tags)
		{
			if (Log.IsDebug)
			{
				using (List<ILog>.Enumerator enumerator = Log.Instances.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.Debug(ex, null, tags);
					}
				}
			}
		}
		public static void Error(string message, params string[] tags)
		{
			using (List<ILog>.Enumerator enumerator = Log.Instances.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.Error(message, null, tags);
				}
			}
		}
		public static void Error(Exception ex, params string[] tags)
		{
			using (List<ILog>.Enumerator enumerator = Log.Instances.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.Error(ex, null, tags);
				}
			}
		}
		public static void Error(string message, object data, params string[] tags)
		{
			using (List<ILog>.Enumerator enumerator = Log.Instances.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.Error(message, data, tags);
				}
			}
		}
		public static void Error(Exception ex, object data, params string[] tags)
		{
			using (List<ILog>.Enumerator enumerator = Log.Instances.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.Error(ex, data, tags);
				}
			}
		}
	}
}
