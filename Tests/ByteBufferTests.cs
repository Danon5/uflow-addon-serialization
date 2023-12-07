using System;
using NUnit.Framework;
using UFlow.Addon.Serialization.Core.Runtime;

namespace UFlow.Addon.Serialization.Tests {
    public sealed class ByteBufferTests {
        [Test]
        public void IntWriteReadTest() {
            var buffer = new ByteBuffer();
            const int value = 5;
            buffer.Write(value);
            buffer.Reset();
            Assert.That(buffer.ReadInt(), Is.EqualTo(5));
        }

        [Test]
        public void IntArrayWriteReadTest() {
            var buffer = new ByteBuffer();
            var values = new[] { 1, 2, 3, 4, 5 };
            buffer.Write(values);
            buffer.Reset();
            buffer.ReadIntArrayInto(values);
            Assert.That(values[0], Is.EqualTo(1));
            Assert.That(values[1], Is.EqualTo(2));
            Assert.That(values[2], Is.EqualTo(3));
            Assert.That(values[3], Is.EqualTo(4));
            Assert.That(values[4], Is.EqualTo(5));
        }

        [Test]
        public void IntArrayWriteReadRawTest() {
            var buffer = new ByteBuffer();
            var values = new[] { 1, 2, 3, 4, 5 };
            buffer.Write(values);
            buffer.Reset();
            Assert.That(buffer.ReadInt(), Is.EqualTo(5));
            Assert.That(buffer.ReadInt(), Is.EqualTo(1));
            Assert.That(buffer.ReadInt(), Is.EqualTo(2));
            Assert.That(buffer.ReadInt(), Is.EqualTo(3));
            Assert.That(buffer.ReadInt(), Is.EqualTo(4));
            Assert.That(buffer.ReadInt(), Is.EqualTo(5));
        }

        [Test]
        public void StringWriteReadTest() {
            var buffer = new ByteBuffer();
            var str = "Testing string";
            buffer.Write(str);
            buffer.Reset();
            Assert.That(buffer.ReadString(), Is.EqualTo(str));
        }

        [Test]
        public void FlagsWriteReadTest() {
            var buffer = new ByteBuffer();
            var flags = TestFlags.Flag2 | TestFlags.Flag3;
            buffer.Write((byte)flags);
            buffer.Reset();
            var deserializedFlags = (TestFlags)buffer.ReadByte();
            var hasFlag1 = deserializedFlags.HasFlag(TestFlags.Flag1);
            var hasFlag2 = deserializedFlags.HasFlag(TestFlags.Flag2);
            var hasFlag3 = deserializedFlags.HasFlag(TestFlags.Flag3);
            var hasFlag4 = deserializedFlags.HasFlag(TestFlags.Flag4);
            Assert.That(hasFlag1, Is.False);
            Assert.That(hasFlag2, Is.True);
            Assert.That(hasFlag3, Is.True);
            Assert.That(hasFlag4, Is.False);
        }

        [Flags]
        private enum TestFlags : byte {
            Flag1 = 1 << 0,
            Flag2 = 1 << 1,
            Flag3 = 1 << 2,
            Flag4 = 1 << 3
        }
    }
}