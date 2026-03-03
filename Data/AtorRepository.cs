using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TESTE_DE_SERVIDOR.Models;

namespace TESTE_DE_SERVIDOR.Data;

public class AtorRepository

{
    private readonly string _connectionString;
    private readonly string _filmeColumn;
    private readonly string _atorColumn;

    public AtorRepository(string connectionString)
    {
        _connectionString = connectionString;
        // determine column names for joining
        using var conn = new SqlConnection(_connectionString);
        conn.Open();
        _filmeColumn = FindColumn(conn, "ElencoFilme", "filme");
        _atorColumn = FindColumn(conn, "ElencoFilme", "ator");
    }

    public List<Ator> ListAll()
    {
        var list = new List<Ator>();
        using var conn = new SqlConnection(_connectionString);
        conn.Open();
        using var cmd = new SqlCommand("SELECT Id, PrimeiroNome, UltimoNome, Genero FROM Atores ORDER BY PrimeiroNome, UltimoNome", conn);
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(ReadAtor(r));
        }
        return list;
    }

    public List<Ator> SearchByName(string nome)
    {
        var list = new List<Ator>();
        using var conn = new SqlConnection(_connectionString);
        conn.Open();
        using var cmd = new SqlCommand("SELECT Id, PrimeiroNome, UltimoNome, Genero FROM Atores WHERE PrimeiroNome LIKE @q OR UltimoNome LIKE @q ORDER BY PrimeiroNome, UltimoNome", conn);
        cmd.Parameters.AddWithValue("@q", "%" + nome + "%");
        using var r = cmd.ExecuteReader();
        while (r.Read()) list.Add(ReadAtor(r));
        return list;
    }

    public Ator? GetById(int id)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();
        using var cmd = new SqlCommand("SELECT Id, PrimeiroNome, UltimoNome, Genero FROM Atores WHERE Id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        using var r = cmd.ExecuteReader();
        if (r.Read()) return ReadAtor(r);
        return null;
    }

    public List<Filme> GetFilmsByActor(int atorId)
    {
        var list = new List<Filme>();
        using var conn = new SqlConnection(_connectionString);
        conn.Open();
        using var cmd = new SqlCommand($@"
            SELECT f.Id, f.Nome, f.Ano, f.Duracao
            FROM Filmes f
            INNER JOIN ElencoFilme ef ON ef.[{_filmeColumn}] = f.Id
            WHERE ef.[{_atorColumn}] = @id
            ORDER BY f.Nome", conn);
        cmd.Parameters.AddWithValue("@id", atorId);
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            var f = new Filme();
            f.Id = r["Id"] != DBNull.Value ? Convert.ToInt32(r["Id"]) : 0;
            f.Nome = r["Nome"]?.ToString() ?? string.Empty;
            f.Ano = r["Ano"] != DBNull.Value ? Convert.ToInt32(r["Ano"]) : 0;
            f.Duracao = r["Duracao"] != DBNull.Value ? (int?)Convert.ToInt32(r["Duracao"]) : null;
            list.Add(f);
        }
        return list;
    }

    private static Ator ReadAtor(SqlDataReader r)
    {
        var a = new Ator();
        a.Id = r["Id"] != DBNull.Value ? Convert.ToInt32(r["Id"]) : 0;
        a.PrimeiroNome = r["PrimeiroNome"]?.ToString() ?? string.Empty;
        a.UltimoNome = r["UltimoNome"]?.ToString() ?? string.Empty;
        a.Genero = r["Genero"]?.ToString() ?? string.Empty;
        return a;
    }

    private string FindColumn(SqlConnection conn, string table, string substring)
    {
        using var cmd = new SqlCommand("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME=@t", conn);
        cmd.Parameters.AddWithValue("@t", table);
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            string col = r.GetString(0);
            if (col.IndexOf(substring, StringComparison.OrdinalIgnoreCase) >= 0)
                return col;
        }
        throw new Exception($"could not find column containing '{substring}' in {table}");
    }
}
