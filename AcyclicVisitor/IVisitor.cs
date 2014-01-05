namespace AcyclicVisitor
{
    public interface IVisitor<in T>
    {
        object Visit(T to);
    }
}