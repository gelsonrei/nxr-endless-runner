using SQLite;

[Table("Ranking")]
public class Ranking: Base
{
    [PrimaryKey]
    [Column("id")]
    public int Id { get; set; }

    [Column("cpf")]
    public string Cpf { get; set; }

    [Column("maxDistance")]
    public int MaxDistance { get; set; }

    [Column("maxPoints")]
    public int MaxPoints { get; set; }
}



