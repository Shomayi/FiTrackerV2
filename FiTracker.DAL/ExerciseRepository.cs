using FiTrackerV2.Domain.Interfaces;
using FiTrackerV2.Domain.Models;
using MySqlConnector;
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

        public ExerciseRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("FiTrackerDb");
        }

        public async Task<int> CreateAsync(Exercise exercise)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand(
                "INSERT INTO Exercises (Name, Sets, Reps, Weight) VALUES (@name, @sets, @reps, @weight); SELECT LAST_INSERT_ID();",
                conn
            );
            cmd.Parameters.AddWithValue("@name", exercise.Name);
            cmd.Parameters.AddWithValue("@sets", exercise.Sets);
            cmd.Parameters.AddWithValue("@reps", exercise.Reps);
            cmd.Parameters.AddWithValue("@weight", exercise.Weight);

            var id = (long)await cmd.ExecuteScalarAsync();
            return (int)id;
        }

        public async Task<Exercise> GetByIdAsync(int id)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("SELECT * FROM Exercises WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Exercise
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Sets = reader.GetInt32("Sets"),
                    Reps = reader.GetInt32("Reps"),
                    Weight = reader.GetDecimal("Weight")
                };
            }
            return null;
        }

        public async Task<List<Exercise>> GetAllAsync()
        {
            var exercises = new List<Exercise>();
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("SELECT * FROM Exercises", conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                exercises.Add(new Exercise
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Sets = reader.GetInt32("Sets"),
                    Reps = reader.GetInt32("Reps"),
                    Weight = reader.GetDecimal("Weight")
                });
            }
            return exercises;
        }

        public async Task<bool> UpdateAsync(Exercise exercise)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand(
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
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("DELETE FROM Exercises WHERE Id=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }
    }
}
