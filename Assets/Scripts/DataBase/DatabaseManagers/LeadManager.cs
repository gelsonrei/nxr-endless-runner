using System;
using System.Collections.Generic;

using UnityEngine;

public class LeadManager : TableManager
{
    protected override void Start()
    {
        base.Start();
    }

    public static void CreateOrUpdate(string name = "", string phone = "", string email = "", string cpf = "", string birthday = "", int activation = 0)
    {
        if (GetOne(cpf) == null)
            Create(name, phone, email, cpf, birthday, activation);
        else Update(name, phone, email, cpf, birthday, activation);
    }

    public static void Create(string name = "", string phone = "", string email = "", string cpf = "", string birthday = "", int activation = 0)
    {
        var lead = new Lead
        {
            Name = name,
            Phone = phone,
            Email = email,
            Cpf = cpf,
            Birthday = birthday,
            Activation = activation
        };
        dbCon.Insert(lead);
        
        Debug.Log("CREATE Lead");
        Print(lead);
    }

    public static void Update(string name = "", string phone = "", string email = "", string cpf = "", string birthday = "", int activation = 0)
    {
        var lead = GetOne(cpf);
        lead.Name = name;
        lead.Phone = phone;
        lead.Email = email;
        lead.Birthday = birthday;
        lead.Activation = activation;

        dbCon.Update(lead);

        Debug.Log("UPDATE Lead");
        Print(lead);
    }

    public static void Delete(string cpf)
    {
        var lead = GetOne(cpf);

        dbCon.Delete(lead);

        Debug.Log("DELEATE Lead");
        Print(lead);
    }

    public static List<Lead> Select(int limit = 0)
    {
        string sql = "SELECT * FROM Lead ";
        if (limit > 0)
        {
            sql += "  LIMIT " + limit;
        }

        List<Lead> m_lead = QueryList<Lead>(sql);

        return m_lead;
    }

    public static Lead GetOne(string cpf)
    {
        string sql = "SELECT * FROM Lead where cpf='" + cpf + "'";
        var leads = QueryList<Lead>(sql);

        return leads.Count > 0 ? leads[0] : null;
    }
}
