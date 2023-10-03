using UnityEngine;

using System.Collections;

namespace EndlessRunner
{
    public class DataBase 
    {
        public static float SelectData(string key)
        {
            float data = PlayerPrefs.GetFloat(key);

            //Debug.Log("Data selected at key: " + key + " : " + data);

            return data;
        }

        public static string SelectSData(string key)
        {
            string data = PlayerPrefs.GetString(key);

            //Debug.Log("Data selected at key: " + key + " : " + data);

            return data;
        }

        public static void InsertData(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);

            //Debug.Log("Data inserted at key: " + key + " : " + value);
        }

        public static void InsertData(string key, string value)
        {
            PlayerPrefs.SetString(key, value);

            //Debug.Log("Data inserted at key: " + key + " : " + value);
        }

        public static void DeleteData(string key)
        {
            PlayerPrefs.DeleteKey(key);

            //Debug.Log("Data deleted at key: " + key );
        }
    }
}