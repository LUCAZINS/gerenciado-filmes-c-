using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TESTE_DE_SERVIDOR.Models;

namespace TESTE_DE_SERVIDOR.Data;

public class FilmeRepository
{
    private readonly string _connectionString;

    public FilmeRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // ✅ LISTAR TODOS
    public List<Filme> ListAll()
    {
        var list = new List<Filme>();

        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            "SELECT Id, Nome, Ano, Duracao FROM Filmes ORDER BY Nome",
            conn);

        using var r = cmd.ExecuteReader();

        while (r.Read())
        {
            list.Add(ReadFilme(r));
        }

        return list;
    }

    // ✅ BUSCAR POR ID
    public Filme? GetById(int id)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            "SELECT Id, Nome, Ano, Duracao FROM Filmes WHERE Id = @id",
            conn);

        cmd.Parameters.AddWithValue("@id", id);

        using var r = cmd.ExecuteReader();

        if (r.Read())
            return ReadFilme(r);

        return null;
    }

    // ✅ BUSCAR POR NOME
    public List<Filme> SearchByName(string nome)
    {
        var list = new List<Filme>();

        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            "SELECT Id, Nome, Ano, Duracao FROM Filmes WHERE Nome LIKE @q ORDER BY Nome",
            conn);

        cmd.Parameters.AddWithValue("@q", "%" + nome + "%");

        using var r = cmd.ExecuteReader();

        while (r.Read())
        {
            list.Add(ReadFilme(r));
        }

        return list;
    }

    // ----------------- helper -----------------
    private static Filme ReadFilme(SqlDataReader r)
    {
        return new Filme
        {
            Id = r["Id"] != DBNull.Value ? Convert.ToInt32(r["Id"]) : 0,
            Nome = r["Nome"]?.ToString() ?? string.Empty,
            Ano = r["Ano"] != DBNull.Value ? Convert.ToInt32(r["Ano"]) : 0,
            Duracao = r["Duracao"] != DBNull.Value
                ? (int?)Convert.ToInt32(r["Duracao"])
                : null
        };
    }
}