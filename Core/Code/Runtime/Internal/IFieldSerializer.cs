﻿namespace UFlow.Addon.Serialization.Core.Runtime {
    internal interface IFieldSerializer<TObject> {
        public void Serialize(in ByteBuffer buffer, ref TObject value);
        public void Deserialize(in ByteBuffer buffer, ref TObject value);
    }
}