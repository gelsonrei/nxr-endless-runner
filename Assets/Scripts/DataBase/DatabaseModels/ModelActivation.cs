using SQLite;

[Table("Activation")]
public class Activation : Base
{
    [PrimaryKey]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("initialData")]
    public string InitialData { get; set; }

    [Column("endData")]
    public string EndData { get; set; }
}



