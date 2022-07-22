using System;

namespace May25.API.Core.Extensions
{
    public static class GuardExtensions
    {
        /// <summary>
        /// Throws an exception if the argument is null. If not, returns the argument.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argument"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T ThrowIfNull<T>(this T argument, string name)
        {
            if (argument == null)
                throw new ArgumentNullException(name);

            return argument;
        }
    }
}
