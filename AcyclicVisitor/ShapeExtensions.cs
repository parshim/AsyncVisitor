
namespace AcyclicVisitor
{
    public static class ShapeExtensions
    {
        public static void Accept<T>(this T to, IVisitor<T> visitor) where T: Shape
        {
            visitor.Visit(to);
        }
        
        public static void Accept<T>(this T to, params IVisitor<T>[] visitorsChain) where T: Shape
        {
            foreach (IVisitor<T> visitor in visitorsChain)
            {
                visitor.Visit(to);   
            }
        }
    }
}
