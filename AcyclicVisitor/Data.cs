using System.Collections;
using System.Collections.Generic;

namespace AcyclicVisitor
{
    public abstract class Shape
    {
    }

    public class Circle : Shape
    {
        public double Radius { get; set; }
    }

    public class Box : Shape
    {
        public double Side { get; set; }
    }

    public class Canvas : Shape, IEnumerable<Shape>
    {
        private readonly List<Shape> _shapes = new List<Shape>();

        public double Height { get; set; }

        public double Width { get; set; }

        public void Add(Shape s)
        {
            _shapes.Add(s);
        }

        public IEnumerator<Shape> GetEnumerator()
        {
            return _shapes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}