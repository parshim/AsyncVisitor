namespace AcyclicVisitor
{
    class CircleMagnifyVisitor : IVisitor<Circle>
    {
        private readonly double _ratio;

        public CircleMagnifyVisitor(double ratio)
        {
            _ratio = ratio;
        }

        public void Visit(Circle to)
        {
            to.Radius *= _ratio;
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
        
        public void Visit(Box to)
        {
            to.Side *= _ratio;
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

        public void Visit(Canvas to)
        {
            to.Height *= _ratio;
            to.Width *= _ratio;

            foreach (Shape shape in to)
            {
                shape.Accept(_visitor);
            }
        }

        public void Dispose()
        {
        }
    }
}
