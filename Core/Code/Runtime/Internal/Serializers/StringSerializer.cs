using UnityEngine.Scripting;

namespace UFlow.Addon.Serialization.Core.Runtime {
    [Preserve]
    internal sealed class StringSerializer : ISerializer<string> {
        public void Serialize(in ByteBuffer buffer, ref string value) => buffer.Write(value);

        public void Deserialize(in ByteBuffer buffer, ref string value) => buffer.ReadString();
    }
}