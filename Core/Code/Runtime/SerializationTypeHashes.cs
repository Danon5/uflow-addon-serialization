// ReSharper disable StaticMemberInGenericType

namespace UFlow.Addon.Serialization.Core.Runtime {
    public sealed class SerializationTypeHashes<T> {
        public static readonly ulong Hash;
        
        static SerializationTypeHashes() => Hash = SerializationAPI.CalculateHash(typeof(T).ToString());
    }
}