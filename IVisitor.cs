namespace AcyclicVisitor
{
    public interface IVisitor<in T>
    {
        void Visit(T to);
    }
}