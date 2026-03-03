using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TESTE_DE_SERVIDOR.Models;

namespace TESTE_DE_SERVIDOR.Data;

public class ElencoFilmeRepository
{
    private readonly string _connectionString;

    public ElencoFilmeRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<ElencoFilme> ListAll()
    {
        var list = new List<ElencoFilme>();

        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            "SELECT Id, IdAtor, IdFilme, Papel FROM ElencoFilme ORDER BY IdFilme, IdAtor",
            conn);

        using var r = cmd.ExecuteReader();
        while (r.Read())
            list.Add(ReadElencoFilme(r));

        return list;
    }

    public List<ElencoFilme> GetByFilme(int filmeId)
    {
        var list = new List<ElencoFilme>();

        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            "SELECT Id, IdAtor, IdFilme, Papel FROM ElencoFilme WHERE IdFilme = @id ORDER BY IdAtor",
            conn);

        cmd.Parameters.AddWithValue("@id", filmeId);

        using var r = cmd.ExecuteReader();
        while (r.Read())
            list.Add(ReadElencoFilme(r));

        return list;
    }

    public List<ElencoFilme> GetByAtor(int atorId)
    {
        var list = new List<ElencoFilme>();

        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            "SELECT Id, IdAtor, IdFilme, Papel FROM ElencoFilme WHERE IdAtor = @id ORDER BY IdFilme",
            conn);

        cmd.Parameters.AddWithValue("@id", atorId);

        using var r = cmd.ExecuteReader();
        while (r.Read())
            list.Add(ReadElencoFilme(r));

        return list;
    }

    public List<ElencoFilme> SearchByPapel(string papel)
    {
        var list = new List<ElencoFilme>();

        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            "SELECT Id, IdAtor, IdFilme, Papel FROM ElencoFilme WHERE Papel LIKE @q ORDER BY IdFilme, IdAtor",
            conn);

        cmd.Parameters.AddWithValue("@q", "%" + papel + "%");

        using var r = cmd.ExecuteReader();
        while (r.Read())
            list.Add(ReadElencoFilme(r));

        return list;
    }

    public ElencoFilme? GetById(int id)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            "SELECT Id, IdAtor, IdFilme, Papel FROM ElencoFilme WHERE Id = @id",
            conn);

        cmd.Parameters.AddWithValue("@id", id);

        using var r = cmd.ExecuteReader();
        if (r.Read())
            return ReadElencoFilme(r);

        return null;
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

    public string GetNomeAtor(int atorId)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        using var cmd = new SqlCommand(@"
SELECT CONCAT(PrimeiroNome, ' ', UltimoNome)
FROM Atores
WHERE Id = @id;", conn);

        cmd.Parameters.AddWithValue("@id", atorId);

        var result = cmd.ExecuteScalar();
        var nome = result?.ToString()?.Trim();
        return string.IsNullOrWhiteSpace(nome) ? "Ator não encontrado" : nome;
    }

    private static ElencoFilme ReadElencoFilme(SqlDataReader r)
    {
        return new ElencoFilme
        {
            Id = r["Id"] != DBNull.Value ? Convert.ToInt32(r["Id"]) : 0,
            IdAtor = r["IdAtor"] != DBNull.Value ? Convert.ToInt32(r["IdAtor"]) : 0,
            IdFilme = r["IdFilme"] != DBNull.Value ? Convert.ToInt32(r["IdFilme"]) : 0,
            Papel = r["Papel"]?.ToString() ?? string.Empty
        };
    }
}