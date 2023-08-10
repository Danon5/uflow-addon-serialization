using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UFlow.Core.Runtime;

namespace UFlow.Addon.Serialization.Core.Runtime {
    internal static class SerializerCache<T> {
        private static readonly ISerializer<T> s_serializer;

        static SerializerCache() {
            var thisAssembly = Assembly.GetAssembly(typeof(SerializerCache<>));
            var serializerType = typeof(ISerializer<>);
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                if (assembly == thisAssembly) continue;
                foreach (var type in assembly.GetTypes()) {
                    if (!UFlowUtils.Reflection.IsDerivedType(type, serializerType)) continue;
                    s_serializer = Activator.CreateInstance(type) as ISerializer<T>;
                    return;
                }
            }
            var genericType = typeof(T);
            var stringType = typeof(string);
            var arraySerializerType = typeof(ArraySerializer<>);
            var stringSerializerType = typeof(StringSerializer);
            var unmanagedSerializerType = typeof(UnmanagedSerializer<>);
            var genericSerializerType = typeof(GenericSerializer<>);
            if (genericType == stringType)
                s_serializer = Activator.CreateInstance(stringSerializerType) as ISerializer<T>;
            else if (genericType.IsArray) {
                var arraySerializerGenericType = arraySerializerType.MakeGenericType(genericType.GetElementType());
                s_serializer = Activator.CreateInstance(arraySerializerGenericType) as ISerializer<T>;
            }
            else if (UFlowUtils.Reflection.IsUnmanaged(genericType)) {
                var unmanagedSerializerGenericType = unmanagedSerializerType.MakeGenericType(genericType);
                s_serializer = Activator.CreateInstance(unmanagedSerializerGenericType) as ISerializer<T>;
            }
            else {
                var genericSerializerGenericType = genericSerializerType.MakeGenericType(genericType);
                s_serializer = Activator.CreateInstance(genericSerializerGenericType) as ISerializer<T>;
            }
        }

        public static bool TryGet(out ISerializer<T> serializer) {
            if (s_serializer == null) {
                serializer = default;
                return false;
            }
            serializer = s_serializer;
            return true;
        }
        
        public static bool TryGetWithThrowOnFailure(out ISerializer<T> serializer) {
            if (s_serializer == null) {
                serializer = default;
                throw new Exception($"No serializer exists for type {typeof(T)}");
            }
            serializer = s_serializer;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Exists() => s_serializer != null;
    }
}