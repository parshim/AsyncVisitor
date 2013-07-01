
namespace AcyclicVisitor
{
    public static class ExtensionMethods
    {
        public static void Accept<T>(this T to, IVisitor<T> visitor) where T: class 
        {
            visitor.Visit(to);
        }

        public static void Accept<T>(this T to, params IVisitor<T>[] visitorsChain) where T : class
        {
            foreach (IVisitor<T> visitor in visitorsChain)
            {
                visitor.Visit(to);   
            }
        }
    }
}
