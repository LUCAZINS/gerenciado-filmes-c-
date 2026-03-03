namespace TESTE_DE_SERVIDOR.Models;

public class ElencoFilme
{
    public int Id { get; set; }
    public int IdAtor { get; set; }
    public int IdFilme { get; set; }
    public string Papel { get; set; } = string.Empty;

    // Opcional – ajuda na exibição no console
    public override string ToString()
        => $"FilmeID: {IdFilme} | AtorID: {IdAtor} | Papel: {Papel}";
}