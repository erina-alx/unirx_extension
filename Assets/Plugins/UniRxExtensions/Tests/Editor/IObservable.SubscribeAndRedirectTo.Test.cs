using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;
using UniRx;
using UniRxExtensions.Tests.Utils;
using UnityEngine.Events;

namespace UniRxExtensions.Tests
{
    // ReSharper disable once InconsistentNaming
    [TestFixture]
    internal class IObservable_SubscribeAndRedirectTo_Test
    {
        [Test]
        public void Test_Subject()
        {
            int[] destinationArray = null;
            var subject = new ReplaySubject<int>();

            TestStream.Make(1, 2, 3).SubscribeAndRedirectTo(subject);
            subject.OnCompleted();

            subject.ToArray().Subscribe(x => destinationArray = x);

            Assert.That(destinationArray, Is.EqualTo(new[] {1, 2, 3}));
        }

        [Test]
        public void Test_UnityEvent()
        {
            int[] destinationArray = null;
            var ue = new Event();

            ue.AsObservable().Take(3).ToArray().Subscribe(x => destinationArray = x);

            TestStream.Make(1, 2, 3).SubscribeAndRedirectTo(ue);

            Assert.That(destinationArray, Is.EqualTo(new[] {1, 2, 3}));
        }

        [Test]
        public void Test_Collection()
        {
            var collection = new Collection<int>();

            TestStream.Make(1, 2, 3).SubscribeAndRedirectTo(collection);

            Assert.That(collection.ToArray(), Is.EqualTo(new[] {1, 2, 3}));
        }

        [Test]
        public void Test_Stack()
        {
            var stack = new Stack<int>();

            TestStream.Make(1, 2, 3).SubscribeAndRedirectTo(stack);

            Assert.That(stack.Count, Is.EqualTo(3));
            Assert.That(stack.Pop(), Is.EqualTo(3));
            Assert.That(stack.Pop(), Is.EqualTo(2));
            Assert.That(stack.Pop(), Is.EqualTo(1));
        }

        [Test]
        public void Test_Queue()
        {
            var queue = new Queue<int>();

            TestStream.Make(1, 2, 3).SubscribeAndRedirectTo(queue);

            Assert.That(queue.Count, Is.EqualTo(3));
            Assert.That(queue.Dequeue(), Is.EqualTo(1));
            Assert.That(queue.Dequeue(), Is.EqualTo(2));
            Assert.That(queue.Dequeue(), Is.EqualTo(3));
        }

        [Test]
        public void Test_Dictionary()
        {
            var dictionary = new Dictionary<string, double>();

            TestStream.Make(1, 2, 3).SubscribeAndRedirectTo(
                dictionary,
                x => x.ToString(),
                x => x * 0.5
                );

            Assert.That(dictionary.Count, Is.EqualTo(3));
            Assert.That(dictionary["1"], Is.EqualTo(0.5));
            Assert.That(dictionary["2"], Is.EqualTo(1.0));
            Assert.That(dictionary["3"], Is.EqualTo(1.5));
        }

        private class Event :
            UnityEvent<int>
        {
        }
    }
}
