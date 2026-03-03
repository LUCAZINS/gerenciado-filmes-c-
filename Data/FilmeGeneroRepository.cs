using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TESTE_DE_SERVIDOR.Models;

namespace TESTE_DE_SERVIDOR.Data;

public class FilmeGeneroRepository
{
    private readonly string _connectionString;

    public FilmeGeneroRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<FilmeGenero> ListAll()
    {
        var list = new List<FilmeGenero>();

        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            "SELECT Id, IdFilme, IdGenero FROM FilmesGenero ORDER BY IdFilme, IdGenero",
            conn);

        using var r = cmd.ExecuteReader();
        while (r.Read())
            list.Add(Read(r));

        return list;
    }

    public List<FilmeGenero> GetByFilme(int filmeId)
    {
        var list = new List<FilmeGenero>();

        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            "SELECT Id, IdFilme, IdGenero FROM FilmesGenero WHERE IdFilme = @id ORDER BY IdGenero",
            conn);

        cmd.Parameters.AddWithValue("@id", filmeId);

        using var r = cmd.ExecuteReader();
        while (r.Read())
            list.Add(Read(r));

        return list;
    }

    public List<FilmeGenero> GetByGenero(int generoId)
    {
        var list = new List<FilmeGenero>();

        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            "SELECT Id, IdFilme, IdGenero FROM FilmesGenero WHERE IdGenero = @id ORDER BY IdFilme",
            conn);

        cmd.Parameters.AddWithValue("@id", generoId);

        using var r = cmd.ExecuteReader();
        while (r.Read())
            list.Add(Read(r));

        return list;
    }

    public List<FilmeGenero> SearchByFilmeName(string nome, FilmeRepository filmeRepo)
    {
        var filmes = filmeRepo.SearchByName(nome);
        var list = new List<FilmeGenero>();

        foreach (var f in filmes)
            list.AddRange(GetByFilme(f.Id));

        return list;
    }

    public List<FilmeGenero> SearchByGeneroName(string nome)
    {
        var list = new List<FilmeGenero>();

        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        using var cmd = new SqlCommand(@"
SELECT fg.Id, fg.IdFilme, fg.IdGenero
FROM FilmesGenero fg
INNER JOIN Generos g ON g.Id = fg.IdGenero
WHERE g.Genero LIKE @q
ORDER BY fg.IdFilme;", conn);

        cmd.Parameters.AddWithValue("@q", "%" + nome + "%");

        using var r = cmd.ExecuteReader();
        while (r.Read())
            list.Add(Read(r));

        return list;
    }

    public string GetNomeFilme(int filmeId)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        using var cmd = new SqlCommand("SELECT Nome FROM Filmes WHERE Id = @id", conn);
        cmd.Parameters.AddWithValue("@id", filmeId);

        var result = cmd.ExecuteScalar();
        return result?.ToString() ?? "Filme não encontrado";
    }

    public string GetNomeGenero(int generoId)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        using var cmd = new SqlCommand("SELECT Genero FROM Generos WHERE Id = @id", conn);
        cmd.Parameters.AddWithValue("@id", generoId);

        var result = cmd.ExecuteScalar();
        return result?.ToString() ?? "Gênero não encontrado";
    }

    private static FilmeGenero Read(SqlDataReader r)
    {
        return new FilmeGenero
        {
            Id = r["Id"] != DBNull.Value ? Convert.ToInt32(r["Id"]) : 0,
            IdFilme = r["IdFilme"] != DBNull.Value ? Convert.ToInt32(r["IdFilme"]) : 0,
            IdGenero = r["IdGenero"] != DBNull.Value ? Convert.ToInt32(r["IdGenero"]) : 0
        };
    }
}