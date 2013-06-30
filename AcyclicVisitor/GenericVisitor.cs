using System;
using System.Reflection;
using Microsoft.Practices.ObjectBuilder2;

namespace AcyclicVisitor
{
    class GenericVisitor : IVisitor<Shape>
    {
        private readonly IBuilderContext _context;
        
        public GenericVisitor(IBuilderContext context)
        {
            _context = context;
        }

        public void Visit(Shape to)
        {
            Type concreteShapeType = to.GetType();

            // Construct generic visitor type for concrete shape type
            Type concreteVisitorType = typeof(IVisitor<>).MakeGenericType(concreteShapeType);

            object concreteVisitor = _context.NewBuildUp(new NamedTypeBuildKey(concreteVisitorType, _context.BuildKey.Name));

            try
            {
                // Invoke
                concreteVisitorType.InvokeMember("Visit", BindingFlags.InvokeMethod, null, concreteVisitor, new object[] { to });
            }
            catch(TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
           
        }

        public void Dispose()
        {
        }
    }

}