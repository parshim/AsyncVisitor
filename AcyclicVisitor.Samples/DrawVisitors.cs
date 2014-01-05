namespace AcyclicVisitor.Samples
{
    class CircleDrawVisitor : IVisitor<Circle>
    {
        private readonly IGraphics _graphics;

        public CircleDrawVisitor(IGraphics graphics)
        {
            _graphics = graphics;
        }

        public object Visit(Circle to)
        {
            _graphics.DrawCircle(to.Radius);

            return null;
        }

        public void Dispose()
        {
        }
    }
    
    class BoxDrawVisitor : IVisitor<Box>
    {
        private readonly IGraphics _graphics;

        public BoxDrawVisitor(IGraphics graphics)
        {
            _graphics = graphics;
        }

        public object Visit(Box to)
        {
            _graphics.DrawBox(to.Side);

            return null;
        }

        public void Dispose()
        {
        }
    }

    class CanvasDrawVisitor : IVisitor<Canvas>
    {
        private readonly IGraphics _graphics;
        private readonly IVisitor<Shape> _visitor;

        public CanvasDrawVisitor(IVisitor<Shape> visitor, IGraphics graphics)
        {
            _visitor = visitor;
            _graphics = graphics;
        }

        public object Visit(Canvas to)
        {
            _graphics.DrawRectangle(to.Height, to.Width);

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
