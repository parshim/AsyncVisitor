using System;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AcyclicVisitor.Samples
{
    [TestClass]
    public class TestAcyclicVisitior
    {
        private IUnityContainer _container;
        private IGraphics _graphics;

        [TestInitialize]
        public void TestInitialize()
        {
            _container = BuildContainer();
            _graphics = _container.Resolve<IGraphics>();
        }

        private static IUnityContainer BuildContainer()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterInstance(A.Fake<IGraphics>())
                     .RegisterType<IVisitor<Box>, BoxDrawVisitor>("Draw")
                     .RegisterType<IVisitor<Circle>, CircleDrawVisitor>("Draw")
                     .RegisterType<IVisitor<Canvas>, CanvasDrawVisitor>("Draw")
                     .RegisterType<IVisitor<Box>, BoxMagnifyVisitor>("Magnify")
                     .RegisterType<IVisitor<Circle>, CircleMagnifyVisitor>("Magnify")
                     .RegisterType<IVisitor<Canvas>, CanvasMagnifyVisitor>("Magnify")
                     .RegisterType<IVisitor<Box>, BoxSquareVisitor>("Square")
                     .RegisterType<IVisitor<Circle>, CircleSquareVisitor>("Square");

            container.AddNewExtension<VisitorContainerExtension>();

            return container;
        }

        [TestCleanup]
        public void TearDown()
        {
            _container.Dispose();
        }

        [TestMethod]
        public void TestDrawBox()
        {
            Box b = new Box {Side = 5};

            IVisitor<Shape> visitor = _container.Resolve<IVisitor<Shape>>("Draw");

            b.Accept(visitor);

            A.CallTo(() => _graphics.DrawBox(5)).MustHaveHappened();
        }

        [TestMethod]
        public void TestDrawBoxWithConcreteVisitor()
        {
            Box b = new Box { Side = 5 };

            IVisitor<Box> visitor = _container.Resolve<IVisitor<Box>>("Draw");

            b.Accept(visitor);

            A.CallTo(() => _graphics.DrawBox(5)).MustHaveHappened();
        }

        [TestMethod]
        public void TestComputeBoxSquere()
        {
            Box b = new Box {Side = 5};

            IVisitor<Shape> visitor = _container.Resolve<IVisitor<Shape>>("Square");

            double square = (double)b.Accept(visitor);
            
            square.Should().Be(25);
        }

        [TestMethod]
        public void TestDrawCanvas()
        {
            Canvas c = new Canvas
                {
                    Height = 10,
                    Width = 12
                };

            c.Add(new Box {Side = 5});
            c.Add(new Circle {Radius = 12});

            IVisitor<Shape> visitor = _container.Resolve<IVisitor<Shape>>("Draw");

            c.Accept(visitor);

            A.CallTo(() => _graphics.DrawRectangle(10, 12)).MustHaveHappened();
            A.CallTo(() => _graphics.DrawBox(5)).MustHaveHappened();
            A.CallTo(() => _graphics.DrawCircle(12)).MustHaveHappened();
        }

        [TestMethod]
        public void TestMagnifyAndThenDrawCircle()
        {
            Circle c = new Circle {Radius = 5};

            var magnificationVisitor = _container.Resolve<IVisitor<Shape>>("Magnify", new ParameterOverride("ratio", 2D));
            var drawVisitor = _container.Resolve<IVisitor<Shape>>("Draw");

            c.Accept(magnificationVisitor, drawVisitor);

            A.CallTo(() => _graphics.DrawCircle(10)).MustHaveHappened();
        }

        [TestMethod]
        public void TestDrawCanvasWithNewShapeWithoutVisitorImplemented()
        {
            Canvas c = new Canvas
                {
                    Height = 10,
                    Width = 12
                };

            c.Add(new Box {Side = 5});
            c.Add(new MyShape {SomeData = 3});

            var visitor = _container.Resolve<IVisitor<Shape>>("Draw");

            try
            {
                c.Accept(visitor);

                Assert.Fail("Exception expected");
            }
            catch (InvalidOperationException)
            {
                // Expecting exception here for missing visitor implementation
            }

            A.CallTo(() => _graphics.DrawRectangle(10, 12)).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => _graphics.DrawBox(5)).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => _graphics.DrawCircle(0)).WithAnyArguments().MustNotHaveHappened();
        }

        [TestMethod]
        public void TestDrawCanvasWithNewShapeWithCustomVisitor()
        {
            Canvas c = new Canvas
                {
                    Height = 10,
                    Width = 12
                };

            MyShape myShape = new MyShape {SomeData = 3};

            c.Add(new Box {Side = 5});
            c.Add(myShape);

            IVisitor<MyShape> customShapeVisitor = A.Fake<IVisitor<MyShape>>();

            _container.RegisterInstance("Draw", customShapeVisitor);

            var visitor = _container.Resolve<IVisitor<Shape>>("Draw");

            c.Accept(visitor);

            A.CallTo(() => _graphics.DrawRectangle(10, 12)).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => _graphics.DrawBox(5)).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => _graphics.DrawCircle(0)).WithAnyArguments().MustNotHaveHappened();
            A.CallTo(() => customShapeVisitor.Visit(myShape)).MustHaveHappened(Repeated.Exactly.Once);
        }
    }

    public class MyShape : Shape
    {
        public int SomeData { get; set; }
    }
}
