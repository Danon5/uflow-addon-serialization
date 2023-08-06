namespace UFlow.Addon.Serialization.Core.Runtime {
    internal sealed class UnmanagedSerializer<T> : ISerializer<T> where T : unmanaged {
        public void Serialize(in ByteBuffer buffer, ref T value) => buffer.WriteUnsafe(value);

        public void Deserialize(in ByteBuffer buffer, ref T value) => value = buffer.ReadUnsafe<T>();
    }
}