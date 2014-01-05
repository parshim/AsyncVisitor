using System;

namespace AcyclicVisitor.Samples
{
    class CircleSquareVisitor : IVisitor<Circle>
    {
        public object Visit(Circle to)
        {
            return Math.PI*Math.Pow(to.Radius, 2);
        }
    }
    
    class BoxSquareVisitor : IVisitor<Box>
    {
        public object Visit(Box to)
        {
            return to.Side * to.Side;
        }
    }
}