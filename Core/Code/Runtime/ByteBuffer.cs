﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace UFlow.Addon.Serialization.Core.Runtime {
    public sealed class ByteBuffer {
        private const int c_default_capacity = 4096;
        private readonly bool m_autoResize;
        private readonly bool m_currentSystemUsesLittleEndian;
        private byte[] m_buffer;

        public int Cursor { get; private set; }
        public int Capacity { get; private set; }

        public ByteBuffer() {
            m_autoResize = false;
            Capacity = c_default_capacity;
            m_buffer = new byte[Capacity];
            m_currentSystemUsesLittleEndian = BitConverter.IsLittleEndian;
        }

        public ByteBuffer(bool autoResize) {
            m_autoResize = autoResize;
            Capacity = c_default_capacity;
            m_buffer = new byte[Capacity];
            m_currentSystemUsesLittleEndian = BitConverter.IsLittleEndian;
        }

        public ByteBuffer(int capacity) {
            m_autoResize = false;
            Capacity = capacity;
            m_buffer = new byte[Capacity];
            m_currentSystemUsesLittleEndian = BitConverter.IsLittleEndian;
        }

        public ByteBuffer(bool autoResize, int initialCapacity) {
            m_autoResize = autoResize;
            Capacity = initialCapacity;
            m_buffer = new byte[Capacity];
            m_currentSystemUsesLittleEndian = BitConverter.IsLittleEndian;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(sbyte value) => WriteUnsafe(value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(byte value) => WriteUnsafe(value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(bool value) => WriteUnsafe((byte)(value ? 1 : 0));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(short value) => WriteUnsafe(value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(ushort value) => WriteUnsafe(value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(int value) => WriteUnsafe(value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(uint value) => WriteUnsafe(value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(float value) => WriteUnsafe(value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(long value) => WriteUnsafe(value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(ulong value) => WriteUnsafe(value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(double value) => WriteUnsafe(value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(char value) => WriteUnsafe(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(string value) {
            Write((ushort)value.Length);
            Encoding.UTF8.GetBytes(value, m_buffer.AsSpan(Cursor, Capacity - Cursor));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(in ReadOnlySpan<sbyte> values) => WriteArrayUnsafe(values);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(in ReadOnlySpan<byte> values) => WriteArrayUnsafe(values);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(in ReadOnlySpan<short> values) => WriteArrayUnsafe(values);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(in ReadOnlySpan<ushort> values) => WriteArrayUnsafe(values);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(in ReadOnlySpan<int> values) => WriteArrayUnsafe(values);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(in ReadOnlySpan<uint> values) => WriteArrayUnsafe(values);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(in ReadOnlySpan<float> values) => WriteArrayUnsafe(values);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(in ReadOnlySpan<long> values) => WriteArrayUnsafe(values);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(in ReadOnlySpan<ulong> values) => WriteArrayUnsafe(values);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(in ReadOnlySpan<double> values) => WriteArrayUnsafe(values);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(in ReadOnlySpan<char> values) => WriteArrayUnsafe(values);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(in ReadOnlySpan<string> values) {
            Write(values.Length);
            foreach (var value in values)
                Write(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sbyte ReadSByte() => ReadUnsafe<sbyte>();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ReadByte() => ReadUnsafe<byte>();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadBool() => ReadUnsafe<byte>() == 1;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public short ReadShort() => ReadUnsafe<short>();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ushort ReadUShort() => ReadUnsafe<ushort>();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInt() => ReadUnsafe<int>();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ReadUInt() => ReadUnsafe<uint>();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ReadFloat() => ReadUnsafe<float>();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ReadLong() => ReadUnsafe<long>();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ReadULong() => ReadUnsafe<ulong>();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double ReadDouble() => ReadUnsafe<double>();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public char ReadChar() => ReadUnsafe<char>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ReadString() {
            var length = ReadUnsafe<ushort>();
            return Encoding.UTF8.GetString(m_buffer.AsSpan(Cursor, length));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sbyte[] ReadSByteArray() => ReadArrayUnsafe<sbyte>();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] ReadByteArray() => ReadArrayUnsafe<byte>();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public short[] ReadShortArray() => ReadArrayUnsafe<short>();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ushort[] ReadUShortArray() => ReadArrayUnsafe<ushort>();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int[] ReadIntArray() => ReadArrayUnsafe<int>();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint[] ReadUIntArray() => ReadArrayUnsafe<uint>();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float[] ReadFloatArray() => ReadArrayUnsafe<float>();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long[] ReadLongArray() => ReadArrayUnsafe<long>();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong[] ReadULongArray() => ReadArrayUnsafe<ulong>();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double[] ReadDoubleArray() => ReadArrayUnsafe<double>();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public char[] ReadCharArray() => ReadArrayUnsafe<char>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string[] ReadStringArray() {
            var length = ReadUnsafe<int>();
            var arr = new string[length];
            for (var i = 0; i < length; i++)
                arr[i] = ReadString();
            return arr;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadSByteArrayInto(in Span<sbyte> span) => ReadArrayIntoUnsafe(span);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadByteArrayInto(in Span<byte> span) => ReadArrayIntoUnsafe(span);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadShortArrayInto(in Span<short> span) => ReadArrayIntoUnsafe(span);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadUShortArrayInto(in Span<ushort> span) => ReadArrayIntoUnsafe(span);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadIntArrayInto(in Span<int> span) => ReadArrayIntoUnsafe(span);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadUIntArrayInto(in Span<uint> span) => ReadArrayIntoUnsafe(span);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadFloatArrayInto(in Span<float> span) => ReadArrayIntoUnsafe(span);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadLongArrayInto(in Span<long> span) => ReadArrayIntoUnsafe(span);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadULongArrayInto(in Span<ulong> span) => ReadArrayIntoUnsafe(span);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadDoubleArrayInto(in Span<double> span) => ReadArrayIntoUnsafe(span);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadCharArrayInto(in Span<char> span) => ReadArrayIntoUnsafe(span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadStringArrayInto(in Span<string> span) {
            var length = ReadUnsafe<int>();
            for (var i = 0; i < length; i++)
                span[i] = ReadString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ResetCursor() => Cursor = 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<byte> GetBytesToCursor() => new(m_buffer, 0, Cursor + 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void WriteUnsafe<T>(T value) {
            var size = Marshal.SizeOf<T>();
            if (m_autoResize)
                EnsureLength(ref m_buffer, Cursor + size);
            var ptr = (byte*)Unsafe.AsPointer(ref value);
            if (m_currentSystemUsesLittleEndian) {
                for (var i = 0; i < size; i++)
                    m_buffer[Cursor + i] = ptr[i];
            }
            else {
                for (var i = size - 1; i >= 0; i--)
                    m_buffer[Cursor + i] = ptr[i];
            }
            Cursor += size;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteArrayUnsafe<T>(in ReadOnlySpan<T> span) {
            if (m_autoResize)
                EnsureLength(ref m_buffer, span.Length);
            WriteUnsafe(span.Length);
            foreach (var value in span)
                WriteUnsafe(value);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe T ReadUnsafe<T>() {
            var size = Marshal.SizeOf<T>();
            var ptr = (byte*)Unsafe.AsPointer(ref m_buffer[Cursor]);
            if (!m_currentSystemUsesLittleEndian) {
                for (var i = 0; i < size / 2; i++)
                    (ptr[i], ptr[size - 1 - i]) = (ptr[size - 1 - i], ptr[i]);
            }
            var value = Unsafe.Read<T>(ptr);
            Cursor += size;
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ReadArrayUnsafe<T>() {
            var length = ReadUnsafe<int>();
            var values = new T[length];
            for (var i = 0; i < length; i++)
                values[i] = ReadUnsafe<T>();
            return values;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadArrayIntoUnsafe<T>(in Span<T> span) {
            var length = ReadUnsafe<int>();
            for (var i = 0; i < length; i++)
                span[i] = ReadUnsafe<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadArrayOverwriteUnsafe<T>(ref T[] array) {
            var length = ReadUnsafe<int>();
            Array.Resize(ref array, length);
            for (var i = 0; i < length; i++)
                array[i] = ReadUnsafe<T>();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureLength<T>(ref T[] array, int length, int maxLength = int.MaxValue) {
            if (array.Length >= length) return;
            var oldCapacity = array.Length;
            var newCapacity = Math.Max(oldCapacity, 1);
            while (newCapacity <= length && newCapacity <= maxLength)
                newCapacity <<= 1;
            newCapacity = Math.Min(newCapacity, maxLength);
            Capacity = newCapacity;
            Array.Resize(ref array, newCapacity);
        }
    }
}