using SQLite;

[Table("Lead")]
public class Lead: Base
{
    [PrimaryKey]
    [Column("cpf")]
    public string Cpf { get; set; }
    [Column("name")]
    public string Name { get; set; }

    [Column("phone")]
    public string Phone { get; set; }

    [Column("email")]
    public string Email { get; set; }

    [Column("birthday")]
    public string Birthday { get; set; }

    [Column("activation")]
    public int Activation { get; set; }
}



