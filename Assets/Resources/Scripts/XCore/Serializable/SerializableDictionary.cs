using System.Collections.Generic;
using System;
using UnityEngine;
using System.Runtime.Serialization;

namespace XCore.Serializable
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializable
    {
        public SerializableDictionary() { }

        protected SerializableDictionary(SerializationInfo info, StreamingContext context)
        {
            // Retrieve the key-value pairs from the serialization info
            int itemCount = info.GetInt32("ItemCount");
            for (int i = 0; i < itemCount; i++)
            {
                TKey key = (TKey)info.GetValue($"Key{i}", typeof(TKey));
                TValue value = (TValue)info.GetValue($"Value{i}", typeof(TValue));
                Add(key, value);
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Store the key-value pairs in the serialization info
            info.AddValue("ItemCount", this.Count);
            int i = 0;
            foreach (KeyValuePair<TKey, TValue> kvp in this)
            {
                info.AddValue($"Key{i}", kvp.Key);
                info.AddValue($"Value{i}", kvp.Value);
                i++;
            }
        }
    }
}
