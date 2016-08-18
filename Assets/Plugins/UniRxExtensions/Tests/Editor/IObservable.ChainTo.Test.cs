using System;
using System.Collections.Generic;
using NUnit.Framework;
using UniRx;

namespace UniRxExtensions.Tests
{
    // ReSharper disable once InconsistentNaming
    [TestFixture]
    internal class IObservable_ChainTo_Test
    {
        [Test]
        public void Test()
        {
            var step = 0;

            Observable.FromSequence(new[] {"a", "b", "c"})
                .ChainTo(
                    (lastEvent, allEvents) =>
                    {
                        Assert.That(++step, Is.EqualTo(1));
                        Assert.That(lastEvent, Is.EqualTo("c"));
                        Assert.That(allEvents, Is.EqualTo(new List<string> {"a", "b", "c"}));

                        return Observable.Return(String.Join("", allEvents.ToArray()));
                    }
                )
                .Subscribe(
                    x =>
                    {
                        Assert.That(++step, Is.EqualTo(2));
                        Assert.That(x, Is.EqualTo("abc"));
                    },
                    () =>
                    {
                        Assert.That(++step, Is.EqualTo(3));
                    }
                );

            Assert.That(() => ++step, Is.EqualTo(4));
        }
    }
}
