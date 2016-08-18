using NUnit.Framework;
using UniRx;
using UnityEngine.Events;

namespace UniRxExtensions.Tests
{
    // ReSharper disable once InconsistentNaming
    [TestFixture]
    internal class UnityEvent_InvokeByTuple_Test
    {
        const int Element1 = 123;
        const float Element2 = 4.5f;
        const double Element3 = 6.7;
        const string Element4 = "890";

        [Test]
        public void Test_For1Element()
        {
            var ue = new Event<int>();
            var done = false;

            ue.AsObservable()
                .Subscribe(
                    x =>
                    {
                        Assert.That(x, Is.EqualTo(Element1));

                        done = true;
                    }
                );

            ue.Invoke(Tuple.Create(Element1));

            Assert.True(done);
        }

        [Test]
        public void Test_For2Elements()
        {
            var ue = new Event<int, float>();
            var done = false;

            ue.AsObservable()
                .Subscribe(
                    x =>
                    {
                        Assert.That(x.Item1, Is.EqualTo(Element1));
                        Assert.That(x.Item2, Is.EqualTo(Element2));

                        done = true;
                    }
                );

            ue.Invoke(Tuple.Create(Element1, Element2));

            Assert.True(done);
        }

        [Test]
        public void Test_For3Elements()
        {
            var ue = new Event<int, float, double>();
            var done = false;

            ue.AsObservable()
                .Subscribe(
                    x =>
                    {
                        Assert.That(x.Item1, Is.EqualTo(Element1));
                        Assert.That(x.Item2, Is.EqualTo(Element2));
                        Assert.That(x.Item3, Is.EqualTo(Element3));

                        done = true;
                    }
                );

            ue.Invoke(Tuple.Create(Element1, Element2, Element3));

            Assert.True(done);
        }

        [Test]
        public void Test_For4Elements()
        {
            var ue = new Event<int, float, double, string>();
            var done = false;

            ue.AsObservable()
                .Subscribe(
                    x =>
                    {
                        Assert.That(x.Item1, Is.EqualTo(Element1));
                        Assert.That(x.Item2, Is.EqualTo(Element2));
                        Assert.That(x.Item3, Is.EqualTo(Element3));
                        Assert.That(x.Item4, Is.EqualTo(Element4));

                        done = true;
                    }
                );

            ue.Invoke(Tuple.Create(Element1, Element2, Element3, Element4));

            Assert.True(done);
        }

        private class Event<T> :
            UnityEvent<T>
        {
        }

        private class Event<T1, T2> :
            UnityEvent<T1, T2>
        {
        }

        private class Event<T1, T2, T3> :
            UnityEvent<T1, T2, T3>
        {
        }

        private class Event<T1, T2, T3, T4> :
            UnityEvent<T1, T2, T3, T4>
        {
        }
    }
}
