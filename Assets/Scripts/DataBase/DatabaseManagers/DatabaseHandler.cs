using System;
using System.Collections.Generic;

using UnityEngine;

using SQLite;

public class DatabaseHandler
{
    private static readonly string resourceFolderPath = Application.dataPath + "/Resources";
    private string dbUri = resourceFolderPath + "/Data/" + "database.db";
    public SQLiteConnection dbConnection;
    static public DatabaseHandler instance;


    private DatabaseHandler()
    {
        Debug.Log(dbUri);

        dbConnection = new SQLiteConnection(dbUri);
        dbConnection.Execute("PRAGMA foreign_keys=ON;");
    }

    public static DatabaseHandler GetInstance()
    {
        instance ??= new DatabaseHandler();

        return instance;
    }
}

