using System;
using UnityEngine.Scripting;

namespace UFlow.Addon.Serialization.Core.Runtime {
    [Preserve]
    internal sealed class ArraySerializer<T> : ISerializer<T[]> {
        public void Serialize(in ByteBuffer buffer, ref T[] value) {
            buffer.Write(value.Length);
            for (var i = 0; i < value.Length; i++) {
                var element = value[i];
                if (!SerializerCache<T>.TryGetWithThrowOnFailure(out var serializer)) continue;
                serializer.Serialize(buffer, ref element);
            }
        }
        
        public void Deserialize(in ByteBuffer buffer, ref T[] value) {
            var length = buffer.ReadInt();
            if (value == null)
                value = new T[length];
            else if (length != value.Length)
                Array.Resize(ref value, length);
            for (var i = 0; i < length; i++) {
                if (!SerializerCache<T>.TryGetWithThrowOnFailure(out var serializer)) continue;
                T element = default;
                serializer.Deserialize(buffer, ref element);
                value[i] = element;
            }
        }
    }
}