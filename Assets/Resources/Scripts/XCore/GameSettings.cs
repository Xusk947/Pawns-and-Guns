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
        private static SerializableDictionary<string, float> _floatDictionary = new SerializableDictionary<string, float>();
        private static SerializableDictionary<string, int> _intDictionary = new SerializableDictionary<string, int>();
        public static void SetBool(string key, bool value)
        {
            _boolDictionary[key] = value;
        }

        public static void SetFloat(string key, float value)
        {
            _floatDictionary[key] = value;
        }

        public static void SetInt(string key, int value)
        {
            _intDictionary[key] = value;
        }

        public static int GetInt(string key)
        {
            int value = 0;

            if (_intDictionary.ContainsKey(key))
            {
                value = _intDictionary[key];
            }
            return value;
        }

        public static float GetFloat(string key) {
            float value = 0f;

            if (_floatDictionary.ContainsKey(key))
            {
                value = _floatDictionary[key];
            }
            
            return value;
        }

        public static bool GetBool(string key)
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
            _boolDictionary.AddRange(LoadSerializableDictionary<string, bool>("settings_b.dat"));
            _intDictionary.AddRange(LoadSerializableDictionary<string, int>("settings_i.dat"));
            _floatDictionary.AddRange(LoadSerializableDictionary<string, float>("settings_f.dat"));
        }
        public static void Save()
        {
            SaveSerializableDictionary(_boolDictionary, "settings_b.dat");
            SaveSerializableDictionary(_intDictionary, "settings_i.dat");
            SaveSerializableDictionary(_floatDictionary, "settings_f.dat");
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
        }
    }
}