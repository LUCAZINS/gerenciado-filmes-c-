namespace TESTE_DE_SERVIDOR.Models;

public class Ator
{
    public int Id { get; set; }

    public string PrimeiroNome { get; set; } = string.Empty;

    public string UltimoNome { get; set; } = string.Empty;

    public string Genero { get; set; } = string.Empty;

    // Propriedade calculada (não está no banco)
    public string NomeCompleto =>
        string.IsNullOrWhiteSpace(UltimoNome)
            ? PrimeiroNome
            : PrimeiroNome + " " + UltimoNome;

    // Opcional – melhora exibição no Console
    public override string ToString()
        => $"{NomeCompleto} ({Genero})";
}