using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UniRx;

namespace UniRxExtensions.Tests
{
    // ReSharper disable once InconsistentNaming
    [TestFixture]
    internal class Observable_FromSequence_Test
    {
        [Test]
        public void Test_Enumerable()
        {
            var enumerable = Enumerable.Range(0, 3);
            int[] destinationArray = null;
            var completed = false;

            // ReSharper disable once PossibleMultipleEnumeration
            Observable.FromSequence(enumerable)
                .ToArray()
                .Subscribe(
                x => destinationArray = x,
                () => completed = true
                );

            Assert.True(completed);
            // ReSharper disable once PossibleMultipleEnumeration
            Assert.That(destinationArray, Is.EqualTo(enumerable.ToArray()));
        }

        [Test]
        public void Test_IList()
        {
            IList<int> list = new List<int> {0, 1, 2,};
            int[] destinationArray = null;
            var completed = false;

            Observable.FromSequence(list)
                .ToArray()
                .Subscribe(
                    x => destinationArray = x,
                    () => completed = true
                );

            Assert.True(completed);
            Assert.That(destinationArray, Is.EqualTo(list.ToArray()));
        }

        [Test]
        public void Test_Array()
        {
            var sourceArray = new[] {0, 1, 2,};
            int[] destinationArray = null;
            var completed = false;

            Observable.FromSequence(sourceArray)
                .ToArray()
                .Subscribe(
                    x => destinationArray = x,
                    () => completed = true
                );

            Assert.True(completed);
            Assert.That(destinationArray, Is.EqualTo(sourceArray));
        }
    }
}
