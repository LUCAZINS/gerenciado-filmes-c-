namespace TESTE_DE_SERVIDOR.Models;

public class FilmeGenero
{
    public int Id { get; set; }
    public int IdFilme { get; set; }
    public int IdGenero { get; set; }

    public override string ToString()
        => $"Associação {Id} | Filme: {IdFilme} | Gênero: {IdGenero}";
}