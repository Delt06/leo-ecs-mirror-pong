using System;
using DELTation.DIFramework;

namespace Composition
{
    public class DiExt
    {
        public static T Resolve<T>() where T : class
        {
            if (Di.TryResolveGlobally(out T result))
                return result;
            throw new ArgumentException($"Could not resolve {typeof(T)}.");
        }
    }
}