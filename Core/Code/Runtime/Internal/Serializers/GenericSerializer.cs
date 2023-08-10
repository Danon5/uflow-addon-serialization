using System;
using UnityEngine.Scripting;

namespace UFlow.Addon.Serialization.Core.Runtime {
    [Preserve]
    internal sealed class GenericSerializer<T> : ISerializer<T> {
        public void Serialize(in ByteBuffer buffer, ref T value) {
            foreach (var fieldSerializer in FieldSerializerCache<T>.AsEnumerable())
                fieldSerializer.Serialize(buffer, ref value);
        }
        
        public void Deserialize(in ByteBuffer buffer, ref T value) {
            value ??= Activator.CreateInstance<T>();
            foreach (var fieldSerializer in FieldSerializerCache<T>.AsEnumerable())
                fieldSerializer.Deserialize(buffer, ref value);
        }
    }
}