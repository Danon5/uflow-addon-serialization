using System.Reflection;
using System.Runtime.CompilerServices;

namespace UFlow.Addon.Serialization.Core.Runtime {
    public static class SerializationAPI {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize<T>(in ByteBuffer buffer, ref T value) {
            if (!SerializerCache<T>.TryGetWithThrowOnFailure(out var serializer)) return;
            serializer.Serialize(buffer, ref value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Deserialize<T>(in ByteBuffer buffer) {
            if (!SerializerCache<T>.TryGetWithThrowOnFailure(out var serializer)) return default;
            var value = (T)default;
            serializer.Deserialize(buffer, ref value);
            return value;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeserializeInto<T>(in ByteBuffer buffer, ref T value) {
            if (!SerializerCache<T>.TryGetWithThrowOnFailure(out var serializer)) return;
            serializer.Deserialize(buffer, ref value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SerializeField<T>(in ByteBuffer buffer, ref T obj, in FieldInfo info) {
            if (!FieldSerializerCache<T>.TryGetWithThrowOnFailure(info, out var serializer)) return;
            serializer.Serialize(buffer, ref obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeserializeFieldInto<T>(in ByteBuffer buffer, ref T obj, in FieldInfo info) {
            if (!FieldSerializerCache<T>.TryGetWithThrowOnFailure(info, out var serializer)) return;
            serializer.Deserialize(buffer, ref obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong CalculateHash(in string source) {
            var hash = 14695981039346656037UL;
            var typeName = source;
            foreach (var c in typeName) {
                hash ^= c;
                hash *= 1099511628211UL;
            }
            return hash;
        }
    }
}