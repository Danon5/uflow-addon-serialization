using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UFlow.Core.Runtime;

// ReSharper disable StaticMemberInGenericType

namespace UFlow.Addon.Serialization.Core.Runtime {
    internal static class FieldSerializerCache<T> {
        private static readonly SerializerEntry[] s_entries;
        private static readonly Dictionary<Type, int[]> s_attributeIndexMap = new();
        private static readonly Dictionary<FieldInfo, int> s_fieldIndexMap = new();

        static FieldSerializerCache() {
            var objType = typeof(T);
            var fieldSerializerType = typeof(FieldSerializer<,>);
            var fieldSerializers = new List<SerializerEntry>();
            foreach (var fieldInfo in objType.GetFields()) {
                var fieldSerializerGenericType = fieldSerializerType.MakeGenericType(objType, fieldInfo.FieldType);
                var fieldSerializer = Activator.CreateInstance(fieldSerializerGenericType, (int)Marshal.OffsetOf<T>(fieldInfo.Name));
                fieldSerializers.Add(new SerializerEntry {
                    fieldInfo = fieldInfo,
                    serializer = fieldSerializer as IFieldSerializer<T>
                });
            }
            s_entries = fieldSerializers.ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IFieldSerializer<T>> AsEnumerable() {
            foreach (var entry in s_entries)
                yield return entry.serializer;
        }
        
        public static IEnumerable<IFieldSerializer<T>> AsEnumerableWithAttribute<TAttribute>() where TAttribute : Attribute {
            var attributeType = typeof(TAttribute);
            if (!s_attributeIndexMap.TryGetValue(attributeType, out var indexMap)) {
                var indices = new List<int>();
                for (var i = 0; i < s_entries.Length; i++) {
                    var entry = s_entries[i];
                    if (!UFlowUtils.Reflection.HasAttribute<TAttribute>(entry.fieldInfo)) continue;
                    indices.Add(i);
                }
                indexMap = indices.ToArray();
                s_attributeIndexMap.Add(attributeType, indexMap);
            }
            foreach (var index in indexMap)
                yield return s_entries[index].serializer;
        }

        public static IFieldSerializer<T> Get(FieldInfo info) {
            if (!s_fieldIndexMap.TryGetValue(info, out var index)) {
                for (var i = 0; i < s_entries.Length; i++) {
                    if (!ReferenceEquals(s_entries[i].fieldInfo, info)) continue;
                    index = i;
                    break;
                }
                s_fieldIndexMap.Add(info, index);
            }
            return s_entries[index].serializer;
        }

        public static bool TryGetWithThrowOnFailure(FieldInfo info, out IFieldSerializer<T> serializer) {
            if (!s_fieldIndexMap.TryGetValue(info, out var index)) {
                serializer = default;
                return false;
            }
            serializer = s_entries[index].serializer;
            return true;
        }

        private struct SerializerEntry {
            public FieldInfo fieldInfo;
            public IFieldSerializer<T> serializer;
        }
    }
}