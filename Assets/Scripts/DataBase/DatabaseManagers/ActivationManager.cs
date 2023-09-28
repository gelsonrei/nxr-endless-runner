using System;
using System.Collections.Generic;

using UnityEngine;

public class ActivationManager : TableManager
{
    protected override void Start()
    {
        base.Start();
    }

    public static void CreateOrUpdate(int id, string name, string initialData, string endData)
    {
        if (Get(id) == null)
            Create(name, initialData, endData);
        else Update(id, name, initialData, endData);
    }

    public static void Create(string name, string initialData, string endData)
    {
        var activation = new Activation
        {
            Name = name,
            InitialData = initialData,
            EndData = endData
        };
        dbCon.Insert(activation);

        Debug.Log("CREATE activation");
        Print(activation);
    }

    public static void Update(int id, string name, string initialData, string endData)
    {
        var activation = Get(id);
        activation.Name = name;
        activation.InitialData = initialData;
        activation.EndData = endData;

        dbCon.Update(activation);

        Debug.Log("UPDATE Activation");
        Print(activation);
    }

    public static void Delete(int id)
    {
        var activation = Get(id);

        dbCon.Delete(activation);

        Debug.Log("DELEATE activation");
        Print(activation);
    }

    public static int GetLength()
    {
        var activations = QueryList<Activation>("SELECT * FROM Activation");
       
        return activations.Count > 0 ? activations.Count : 0;
    }

    public static Activation Get(int id)
    {
        string sql = "SELECT * FROM Activation where id=" + id;

        return GetOne(sql);
    }

    private static Activation GetOne(string sql)
    {
        var activations = QueryList<Activation>(sql);
        return activations.Count > 0 ? activations[0] : null;
    }

}
