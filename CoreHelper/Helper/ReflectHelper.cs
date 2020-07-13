using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Omipay.Core
{
    public static class ReflectHelper
    {
        public static List<Type> FindTypesByAttribute(List<Type> types, Type attrType)
        {
            if (!(types.Count != 0 && attrType.IsSubclassOf(typeof(Attribute))))
            {
                return new List<Type>();
            }
            return (from p in types
                    where IsTypeHasAttribute(p, attrType)
                    select p).ToList();
        }

        public static bool IsTypeHasAttribute(MemberInfo referMember, Type attrType)
        {
            if (!attrType.IsSubclassOf(typeof(Attribute)))
            {
                return false;
            }
            object[] customAttributes = referMember.GetCustomAttributes(attrType, true);
            return ((customAttributes != null) && (customAttributes.Length > 0));
        }

        public static void ExistsDirectory(string directoryName, Action<DirectoryInfo> action)
        {
            if (Directory.Exists(directoryName))
            {
                DirectoryInfo info = new DirectoryInfo(directoryName);
                action(info);
            }
        }

        public static List<Type> GetTypes(string dirPath, string searchPattern = "*.dll")
        {

            List<Type> list = new List<Type>();
            List<Assembly> asms = new List<Assembly>();
            if (Directory.Exists(dirPath))
            {
                var directoryInfo = new DirectoryInfo(dirPath);


                directoryInfo.GetFiles(searchPattern).Each((Action<FileInfo>)(fileInfo => asms.Add(fileInfo.FullName.LoadAssembly())));
            }
            list.AddRange(GetTypes(asms));
            return list;
        }

        private static void Each<TSource>(this IEnumerable<TSource> enumer, Action<TSource> action)
        {
            if (enumer != null)
            {
                foreach (TSource local in enumer)
                {
                    action(local);
                }
            }
        }

        private static Assembly LoadAssembly(this string asmFileName)
        {
            try
            {
                return Assembly.LoadFrom(asmFileName);
            }
            catch
            {
                return null;
            }
        }

        public static List<Type> GetTypes(List<Assembly> asms)
        {
            List<Type> list = new List<Type>();
            asms.ForEach(delegate (Assembly asm)
           {
               if (asm != null)
               {
                   list.AddRange(asm.GetTypes());
               }
           });
            return list;
        }


    }
}
