using System;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace AcyclicVisitor
{
    public class VisitorContainerExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Strategies.AddNew<VisitorBuilderStrategy>(UnityBuildStage.PreCreation);
        }
    }

    internal class VisitorBuilderStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            Type typeToBuild = context.BuildKey.Type;

            if (typeToBuild.IsGenericType && typeof (IVisitor<>) == typeToBuild.GetGenericTypeDefinition())
            {
                AbstractVisitor abstractVisitor = new AbstractVisitor(context);

                context.Existing = abstractVisitor;

                context.AddResolverOverrides(new DependencyOverride(typeToBuild, abstractVisitor));
            }
        }
    }
}