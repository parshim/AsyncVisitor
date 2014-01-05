namespace AcyclicVisitor.Samples
{
    class CircleMagnifyVisitor : IVisitor<Circle>
    {
        private readonly double _ratio;

        public CircleMagnifyVisitor(double ratio)
        {
            _ratio = ratio;
        }

        public object Visit(Circle to)
        {
            to.Radius *= _ratio;

            return null;
        }

        public void Dispose()
        {
        }
    }

    class BoxMagnifyVisitor : IVisitor<Box>
    {
        private readonly double _ratio;

        public BoxMagnifyVisitor(double ratio)
        {
            _ratio = ratio;
        }
        
        public object Visit(Box to)
        {
            to.Side *= _ratio;

            return null;
        }

        public void Dispose()
        {
        }
    }

    class CanvasMagnifyVisitor : IVisitor<Canvas>
    {
        private readonly double _ratio;
        private readonly IVisitor<Shape> _visitor;

        public CanvasMagnifyVisitor(double ratio, IVisitor<Shape> visitor)
        {
            _ratio = ratio;
            _visitor = visitor;
        }

        public object Visit(Canvas to)
        {
            to.Height *= _ratio;
            to.Width *= _ratio;

            foreach (Shape shape in to)
            {
                shape.Accept(_visitor);
            }

            return null;
        }

        public void Dispose()
        {
        }
    }
}
