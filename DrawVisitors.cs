namespace AcyclicVisitor
{
    class CircleDrawVisitor : IVisitor<Circle>
    {
        private readonly IGraphics _graphics;

        public CircleDrawVisitor(IGraphics graphics)
        {
            _graphics = graphics;
        }

        public void Visit(Circle to)
        {
            _graphics.DrawCircle(to.Radius);
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

        public void Visit(Box to)
        {
            _graphics.DrawBox(to.Side);
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

        public void Visit(Canvas to)
        {
            _graphics.DrawRectangle(to.Height, to.Width);

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
