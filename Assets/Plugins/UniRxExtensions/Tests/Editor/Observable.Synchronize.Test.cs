using NUnit.Framework;
using UniRx;

namespace UniRxExtensions.Tests
{
    // ReSharper disable once InconsistentNaming
    [TestFixture]
    internal class Observable_Synchronize_Test
    {
        [Test]
        public void Test()
        {
            var a = new ReactiveProperty<int>(1);
            var b = new ReactiveProperty<int>(2);

            Assert.That(a.Value, Is.EqualTo(1));
            Assert.That(b.Value, Is.EqualTo(2));

            var d = Observable.Synchronize(a, b);

            Assert.That(a.Value, Is.EqualTo(1));
            Assert.That(b.Value, Is.EqualTo(1));

            a.Value = 3;

            Assert.That(a.Value, Is.EqualTo(3));
            Assert.That(b.Value, Is.EqualTo(3));

            b.Value = 4;

            Assert.That(a.Value, Is.EqualTo(4));
            Assert.That(b.Value, Is.EqualTo(4));

            d.Dispose();

            a.Value = 5;

            Assert.That(a.Value, Is.EqualTo(5));
            Assert.That(b.Value, Is.EqualTo(4));

            b.Value = 6;

            Assert.That(a.Value, Is.EqualTo(5));
            Assert.That(b.Value, Is.EqualTo(6));
        }
    }
}
