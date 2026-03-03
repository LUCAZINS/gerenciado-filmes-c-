namespace TESTE_DE_SERVIDOR.Models;

public class Filme
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Ano { get; set; }
    public int? Duracao { get; set; }

    // (Opcional) ajuda na exibição
    public override string ToString()
        => $"{Nome} ({Ano}) - {(Duracao?.ToString() ?? "N/A")} min";
}