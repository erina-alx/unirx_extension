using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UniRx;

namespace UniRxExtensions.Tests
{
    // ReSharper disable once InconsistentNaming
    [TestFixture]
    internal class Observable_FromEnumerator_Test
    {
        [Test]
        public void Test_Interface()
        {
            var list = new List<int> {1, 2, 3};
            int[] destinationArray = null;
            var completed = false;

            Observable.FromEnumerator(list.GetEnumerator())
                .ToArray()
                .Subscribe(
                    x => destinationArray = x,
                    () => completed = true
                );

            Assert.True(completed);
            Assert.That(destinationArray, Is.EqualTo(list.ToArray()));
        }

        [Test]
        public void Test_ListEnumerator()
        {
            var list = new List<int> {1, 2, 3};
            int[] destinationArray = null;
            var completed = false;

            Observable.FromListEnumerator(list.GetEnumerator())
                .ToArray()
                .Subscribe(
                    x => destinationArray = x,
                    () => completed = true
                );

            Assert.True(completed);
            Assert.That(destinationArray, Is.EqualTo(list.ToArray()));
        }

        [Test]
        public void Test_LinkedListEnumerator()
        {
            var list = new LinkedList<int>(new[] {1, 2, 3});
            int[] destinationArray = null;
            var completed = false;

            Observable.FromLinkedListEnumerator(list.GetEnumerator())
                .ToArray()
                .Subscribe(
                    x => destinationArray = x,
                    () => completed = true
                );

            Assert.True(completed);
            Assert.That(destinationArray, Is.EqualTo(list.ToArray()));
        }
    }
}
