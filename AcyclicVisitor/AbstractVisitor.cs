using System;
using System.Reflection;
using Microsoft.Practices.ObjectBuilder2;

namespace AcyclicVisitor
{
    internal class AbstractVisitor : IVisitor<object>
    {
        private readonly IBuilderContext _context;
        
        public AbstractVisitor(IBuilderContext context)
        {
            _context = context;
        }

        public object Visit(object to)
        {
            Type visiteeType = to.GetType();

            // Construct generic visitor type for concrete shape type
            Type concreteVisitorType = typeof(IVisitor<>).MakeGenericType(visiteeType);

            object concreteVisitor = _context.NewBuildUp(new NamedTypeBuildKey(concreteVisitorType, _context.BuildKey.Name));

            if (concreteVisitor.GetType() == typeof (AbstractVisitor))
            {
                throw new InvalidOperationException(string.Format("Failed to resolve vistor  for {0}", visiteeType));
            }

            try
            {
                // Invoke
                return concreteVisitorType.InvokeMember("Visit", BindingFlags.InvokeMethod, null, concreteVisitor, new[] { to });
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