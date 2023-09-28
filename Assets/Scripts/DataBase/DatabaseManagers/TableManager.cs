using System.Reflection;
using System.Collections.Generic;

using UnityEngine;

using SQLite;

public class TableManager :  MonoBehaviour
{
    protected static DatabaseHandler databaseHandler;
    protected static SQLiteConnection dbCon;
     
    protected virtual void Start()
    {
        Init();
    }

    protected virtual void Awake()
    {
       
    }

    public static void Init()
    {
        databaseHandler = DatabaseHandler.GetInstance();
        dbCon = databaseHandler.dbConnection;
    }

    protected static List<T> QueryList<T>(string sql, bool list = true) where T : new()
    {
        var resultList = dbCon.Query<T>(sql);

        if (list)
        {
            foreach (var result in resultList)
                Print(result);
        }

        return resultList;
    }

    public static void Print<T>(T obj)
    {
        PropertyInfo[] properties = typeof(T).GetProperties();

        string result = "Tabela: "+typeof(T).ToString()+" - ";
        foreach (var property in properties)
        {
            var value = property.GetValue(obj);
            result += ($"{property.Name}: {value} | ");
        }
        
        Debug.Log(result);
    }

    public static Dictionary<string,string> GetFieldNameValue<T>(T obj)
    {
        PropertyInfo[] properties = typeof(T).GetProperties();
        Dictionary<string, string> keyValuePairs= new();

        foreach (var property in properties)
        {
            var value = property.GetValue(obj);
            keyValuePairs.Add(property.Name, value.ToString());
        }

        return keyValuePairs;
    }
}
