using FiTrackerV2.Domain.Interfaces;
using FiTrackerV2.Domain.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace FiTrackerV2.DAL
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly string _connectionString;

        public ExerciseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<int> CreateAsync(Exercise exercise)
        {
            await using var conn = GetConnection();
            await conn.OpenAsync();

            var cmd = new SqlCommand(
                "INSERT INTO Exercises (Name, Sets, Reps, Weight) VALUES (@name, @sets, @reps, @weight); SELECT SCOPE_IDENTITY();",
                conn
            );

            cmd.Parameters.AddWithValue("@name", exercise.Name);
            cmd.Parameters.AddWithValue("@sets", exercise.Sets);
            cmd.Parameters.AddWithValue("@reps", exercise.Reps);
            cmd.Parameters.AddWithValue("@weight", exercise.Weight);

            var id = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(id);
        }

        public async Task<Exercise> GetByIdAsync(int id)
        {
            await using var conn = GetConnection();
            await conn.OpenAsync();

            var cmd = new SqlCommand("SELECT * FROM Exercises WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Exercise
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Sets = reader.GetInt32(reader.GetOrdinal("Sets")),
                    Reps = reader.GetInt32(reader.GetOrdinal("Reps")),
                    Weight = reader.GetDecimal(reader.GetOrdinal("Weight"))
                };
            }

            return null;
        }

        public async Task<List<Exercise>> GetAllAsync()
        {
            var exercises = new List<Exercise>();
            await using var conn = GetConnection();
            await conn.OpenAsync();

            var cmd = new SqlCommand("SELECT * FROM Exercises", conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                exercises.Add(new Exercise
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Sets = reader.GetInt32(reader.GetOrdinal("Sets")),
                    Reps = reader.GetInt32(reader.GetOrdinal("Reps")),
                    Weight = reader.GetDecimal(reader.GetOrdinal("Weight"))
                });
            }

            return exercises;
        }

        public async Task<bool> UpdateAsync(Exercise exercise)
        {
            await using var conn = GetConnection();
            await conn.OpenAsync();

            var cmd = new SqlCommand(
                "UPDATE Exercises SET Name=@name, Sets=@sets, Reps=@reps, Weight=@weight WHERE Id=@id",
                conn
            );

            cmd.Parameters.AddWithValue("@name", exercise.Name);
            cmd.Parameters.AddWithValue("@sets", exercise.Sets);
            cmd.Parameters.AddWithValue("@reps", exercise.Reps);
            cmd.Parameters.AddWithValue("@weight", exercise.Weight);
            cmd.Parameters.AddWithValue("@id", exercise.Id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var conn = GetConnection();
            await conn.OpenAsync();

            var cmd = new SqlCommand("DELETE FROM Exercises WHERE Id=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }
    }
}