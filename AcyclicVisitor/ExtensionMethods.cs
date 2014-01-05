
using System.Collections.Generic;

namespace AcyclicVisitor
{
    public static class ExtensionMethods
    {
        public static object Accept<T>(this T to, IVisitor<T> visitor) where T: class 
        {
            return visitor.Visit(to);
        }

        public static object[] Accept<T>(this T to, params IVisitor<T>[] visitorsChain) where T : class
        {
            List<object> collectedResult = new List<object>();

            foreach (IVisitor<T> visitor in visitorsChain)
            {
                object result = visitor.Visit(to);

                collectedResult.Add(result);
            }

            return collectedResult.ToArray();
        }

        public static IEnumerable<object> Accept<T>(this T to, IEnumerable<IVisitor<T>> visitorsChain) where T : class
        {
            foreach (IVisitor<T> visitor in visitorsChain)
            {
                object result = visitor.Visit(to);

                yield return result;
            }
        }
    }
}
