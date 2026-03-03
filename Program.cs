using System;
using System.Collections.Generic;
using TESTE_DE_SERVIDOR.Data;
using TESTE_DE_SERVIDOR.Models;

class Program
{
    static void Main()
    {
        // 🔹 String de conexão com o SQL Server
        string connectionString =
            "Server=localhost\\SQLEXPRESS;Database=Filmes;Trusted_Connection=True;TrustServerCertificate=True;";

        // 🔹 Instanciando os repositórios (responsáveis por acessar o banco)
        var repoFilme = new FilmeRepository(connectionString);
        var repoAtor = new AtorRepository(connectionString);
        var repoElenco = new ElencoFilmeRepository(connectionString);
        var repoGen = new FilmeGeneroRepository(connectionString);

        Console.WriteLine("Conectado ao banco de dados!");
        Console.WriteLine("\n=== Gerenciador de Filmes ===");

        // 🔁 Loop principal do sistema (fica rodando até o usuário sair)
        while (true)
        {
            Console.WriteLine("\n=== O que você deseja consultar? ===");
            Console.WriteLine("1. Filmes");
            Console.WriteLine("2. Atores");
            Console.WriteLine("3. Gêneros");
            Console.WriteLine("4. Elenco");
            Console.WriteLine("5. Sair");

            Console.Write("\nEscolha uma opção (1-5): ");
            string? opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    MenuFilmes(repoFilme, repoGen, repoElenco);
                    break;

                case "2":
                    MenuAtores(repoAtor);
                    break;

                case "3":
                    MenuGeneros(repoGen);
                    break;

                case "4":
                    MenuElenco(repoElenco);
                    break;

                case "5":
                    Console.WriteLine("Encerrando sistema...");
                    return; // encerra o programa

                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
        }
    }

    // ========================= MENU FILMES =========================
    static void MenuFilmes(
        FilmeRepository repoFilme,
        FilmeGeneroRepository repoGen,
        ElencoFilmeRepository repoElenco)
    {
        Console.WriteLine("\n--- Menu Filmes ---");
        Console.WriteLine("1. Listar todos");
        Console.WriteLine("2. Pesquisar por nome");
        Console.WriteLine("3. Voltar");

        Console.Write("Escolha: ");
        string? op = Console.ReadLine();

        if (op == "1")
        {
            // 🔹 Busca todos os filmes no banco
            var filmes = repoFilme.ListAll();

            // 🔹 Exibe os filmes encontrados
            foreach (var f in filmes)
            {
                Console.WriteLine($"ID: {f.Id} | {f.Nome} ({f.Ano}) - {f.Duracao ?? 0} min");
            }
        }
        else if (op == "2")
        {
            // 🔹 Pesquisa filme pelo nome
            Console.Write("Digite o nome do filme: ");
            string? nome = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(nome))
            {
                var filmes = repoFilme.SearchByName(nome);

                foreach (var f in filmes)
                {
                    Console.WriteLine($"ID: {f.Id} | {f.Nome} ({f.Ano})");
                }
            }
        }
    }

    // ========================= MENU ATORES =========================
    static void MenuAtores(AtorRepository repoAtor)
    {
        Console.WriteLine("\n--- Menu Atores ---");
        Console.WriteLine("1. Listar todos");
        Console.WriteLine("2. Pesquisar por nome");
        Console.WriteLine("3. Voltar");

        Console.Write("Escolha: ");
        string? op = Console.ReadLine();

        if (op == "1")
        {
            var atores = repoAtor.ListAll();

            foreach (var a in atores)
            {
                Console.WriteLine($"ID: {a.Id} | {a.NomeCompleto} | Gênero: {a.Genero}");
            }
        }
        else if (op == "2")
        {
            Console.Write("Digite o nome do ator: ");
            string? nome = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(nome))
            {
                var atores = repoAtor.SearchByName(nome);

                foreach (var a in atores)
                {
                    Console.WriteLine($"ID: {a.Id} | {a.NomeCompleto}");
                }
            }
        }
    }

    // ========================= MENU GÊNEROS =========================
    static void MenuGeneros(FilmeGeneroRepository repoGen)
    {
        Console.WriteLine("\n--- Menu Gêneros ---");
        Console.WriteLine("1. Listar associações");
        Console.WriteLine("2. Pesquisar por nome do gênero");
        Console.WriteLine("3. Voltar");

        Console.Write("Escolha: ");
        string? op = Console.ReadLine();

        if (op == "1")
        {
            var lista = repoGen.ListAll();

           foreach (var fg in lista)
        {
    string nomeFilme = repoGen.GetNomeFilme(fg.IdFilme);
    string nomeGenero = repoGen.GetNomeGenero(fg.IdGenero);

        Console.WriteLine($"AssocID: {fg.Id} | Filme: {nomeFilme} | Gênero: {nomeGenero} (GeneroID: {fg.IdGenero})");
        }
        }
        else if (op == "2")
        {
            Console.Write("Digite o nome do gênero: ");
            string? nome = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(nome))
            {
                var lista = repoGen.SearchByGeneroName(nome);

                foreach (var fg in lista)
            {
            string nomeFilme = repoGen.GetNomeFilme(fg.IdFilme);
            string nomeGenero = repoGen.GetNomeGenero(fg.IdGenero);

    Console.WriteLine($"AssocID: {fg.Id} | Filme: {nomeFilme} | Gênero: {nomeGenero} (GeneroID: {fg.IdGenero})");
            }
            }
        }
    }

    // ========================= MENU ELENCO =========================
    static void MenuElenco(ElencoFilmeRepository repoElenco)
    {
        Console.WriteLine("\n--- Menu Elenco ---");
        Console.WriteLine("1. Listar todo elenco");
        Console.WriteLine("2. Por filme (ID)");
        Console.WriteLine("3. Por ator (ID)");
        Console.WriteLine("4. Pesquisar por papel");
        Console.WriteLine("5. Voltar");

        Console.Write("Escolha: ");
        string? op = Console.ReadLine();

        if (op == "1")
        {
            var elenco = repoElenco.ListAll();

            foreach (var e in elenco)
{
    // Busca os nomes reais no banco
    string nomeFilme = repoElenco.GetNomeFilme(e.IdFilme);
    string nomeAtor = repoElenco.GetNomeAtor(e.IdAtor);

    Console.WriteLine($"ID: {e.Id} | Filme: {nomeFilme} | Ator: {nomeAtor} | Papel: {e.Papel}");
}
            
        }
        else if (op == "2")
        {
            Console.Write("Digite o ID do filme: ");
            if (int.TryParse(Console.ReadLine(), out int filmeId))
            {
                var elenco = repoElenco.GetByFilme(filmeId);

                foreach (var e in elenco)
                {
                    // Busca os nomes reais no banco
                string nomeFilme = repoElenco.GetNomeFilme(e.IdFilme);
                string nomeAtor = repoElenco.GetNomeAtor(e.IdAtor);
                    Console.WriteLine($"ID: {e.Id} | Filme: {nomeFilme} | Ator: {nomeAtor} | Papel: {e.Papel}");
                }
            }
        }
        else if (op == "3")
        {
            Console.Write("Digite o ID do ator: ");
            if (int.TryParse(Console.ReadLine(), out int atorId))
            {
                var elenco = repoElenco.GetByAtor(atorId);

                foreach (var e in elenco)
                {
                    Console.WriteLine($"FilmeID: {e.IdFilme} | Papel: {e.Papel}");
                }
            }
        }
        else if (op == "4")
        {
            Console.Write("Digite o papel: ");
            string? papel = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(papel))
            {
                var elenco = repoElenco.SearchByPapel(papel);

                foreach (var e in elenco)
                {
                    // Busca os nomes reais no banco
string nomeFilme = repoElenco.GetNomeFilme(e.IdFilme);
string nomeAtor = repoElenco.GetNomeAtor(e.IdAtor);
                    Console.WriteLine($"ID: {e.Id} | Filme: {nomeFilme} | Ator: {nomeAtor} | Papel: {e.Papel}");                }
            }
        }
    }
}