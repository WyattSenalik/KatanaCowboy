using System;
using System.Collections.Generic;
using System.Reflection;

namespace GameEventSystemAttributes
{
    /// <summary>
    /// Gets information about the Assemblies and Types in the project.
    /// </summary>
    public static class AssemblyManager
    {
        /// <summary>
        /// List of all the Assemblies in the current domain.
        /// </summary>
        public static Assembly[] GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
        /// <summary>
        /// List of all Types in the current domain.
        /// </summary>
        public static List<Type> GetTypes()
        {
            List<Type> types = new List<Type>();
            foreach (Assembly a in GetAssemblies())
            {
                types.AddRange(a.GetTypes());
            }

            return types;
        }
        /// <summary>
        /// List of all the Names of the given Types.
        /// </summary>
        public static List<string> GetTypeNames(IReadOnlyList<Type> typeList)
        {
            List<string> names = new List<string>(typeList.Count);

            foreach (Type t in typeList)
            {
                names.Add(t.Name);
            }

            return names;
        }
        /// <summary>
        /// List of all the Names of the Types in the current domain.
        /// </summary>
        public static List<string> GetTypeNames()
        {
            return GetTypeNames(GetTypes());
        }
        /// <summary>
        /// List of all the FullNames of the given Types.
        /// </summary>
        public static List<string> GetTypeFullNames(IReadOnlyList<Type> typeList)
        {
            List<string> fullNames = new List<string>(typeList.Count);

            foreach (Type t in typeList)
            {
                fullNames.Add(t.FullName);
            }

            return fullNames;
        }
        /// <summary>
        /// List of all the FullNames of the Types in the current domain.
        /// </summary>
        public static List<string> GetTypeFullNames()
        {
            return GetTypeFullNames(GetTypes());
        }
    }
}
