using System;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace AcyclicVisitor
{
    public class VisitorContainerExtension : VisitorContainerExtension<object>
    {
        
    }

    public class VisitorContainerExtension<T> : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Strategies.AddNew<VisitorBuilderStrategy<T>>(UnityBuildStage.PreCreation);
        }
    }

    internal class VisitorBuilderStrategy<T> : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            Type typeToBuild = context.BuildKey.Type;
            if (typeToBuild.IsGenericType && typeof (IVisitor<>).MakeGenericType(typeof(T)) == typeToBuild)
            {
                AbstractVisitor<T> abstractVisitor = new AbstractVisitor<T>(context);

                context.Existing = abstractVisitor;

                context.AddResolverOverrides(new DependencyOverride(typeToBuild, abstractVisitor));
            }
        }
    }
}