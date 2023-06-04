using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using XCore.Serializable;

namespace XCore
{
    public static class GameSettings
    {
        private static readonly string SAVE_PATH = Application.persistentDataPath;

        private static SerializableDictionary<string, bool> _boolDictionary = new SerializableDictionary<string, bool>();
        public static void SetBool(string key, bool value)
        {
            _boolDictionary[key] = value;
        }

        public static bool GetValue(string key)
        {
            bool value = false;

            if (_boolDictionary.ContainsKey(key))
            {
                value = _boolDictionary[key];
            }

            return value;
        }
        
        public static void Load()
        {
            _boolDictionary.AddRange(LoadSerializableDictionary<string, bool>("settings.dat"));
        }
        public static void Save()
        {
            SaveSerializableDictionary(_boolDictionary, "settings.dat");
        }
        public static SerializableDictionary<TKey, TValue> LoadSerializableDictionary<TKey, TValue>(string filename)
        {
            SerializableDictionary<TKey, TValue> dictionary = null;
            string filePath = Path.Combine(SAVE_PATH, filename);

            if (File.Exists(filePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    dictionary = (SerializableDictionary<TKey, TValue>)formatter.Deserialize(fileStream);
                }
            }
            else
            {
                Debug.LogWarning("Save file not found.");
                dictionary = new SerializableDictionary<TKey, TValue>();
            }

            return dictionary;
        }

        public static void SaveSerializableDictionary<TKey, TValue>(SerializableDictionary<TKey, TValue> dictionary, string filename)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string filePath = Path.Combine(SAVE_PATH, filename);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                formatter.Serialize(fileStream, dictionary);
            }

            Debug.Log($"File saved at: {filePath}");
        }
    }
}