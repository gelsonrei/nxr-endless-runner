using System;
using System.Collections.Generic;

using UnityEngine;

public class RankingManager : TableManager
{
    protected override void Start()
    {
        base.Start();
    }

    public static void CreateOrUpdate(int id, string cpf, int maxDistance, int maxPoints)
    {
        if (Get(id) == null)
            Create(cpf, maxDistance, maxPoints);
        else Update(id, cpf, maxDistance, maxPoints);
    }

    public static void Create(string cpf, int maxDistance, int maxPoints)
    {
        var ranking = new Ranking
        {
            Id = Select().Count + 1,
            Cpf = cpf,
            MaxDistance = maxDistance,
            MaxPoints = maxPoints
        };
        dbCon.Insert(ranking);
        
        Debug.Log("CREATE ranking");
        Print(ranking);
    }

    public static void Update(int id, string cpf, int maxDistance, int maxPoints)
    {
        var ranking = Get(id);
        ranking.Cpf = cpf;
        ranking.MaxDistance = maxDistance;
        ranking.MaxPoints = maxPoints;
        
        dbCon.Update(ranking);

        Debug.Log("UPDATE ranking");
        Print(ranking);
    }

    public static void Delete(int id)
    {
        var ranking = Get(id);

        dbCon.Delete(ranking);

        Debug.Log("DELEATE ranking");
        Print(ranking);
    }

    public static List<Ranking> Select(int limit = 0)
    {
        string sql = "SELECT * FROM Ranking ";
        sql += " ORDER BY maxDistance DESC ";
        if (limit > 0)
        {
            sql += "  LIMIT " + limit;
        }

        List<Ranking> m_ranking = QueryList<Ranking>(sql);

        return m_ranking;
    }

    public static Ranking Get(int id)
    {
        string sql = "SELECT * FROM Ranking where id=" + id;

        return GetOne(sql);
    }

    public static Ranking GetOne(string sql)
    {
        var ranking = QueryList<Ranking>(sql);
        return ranking.Count > 0 ? ranking[0] : null;
    }
}
