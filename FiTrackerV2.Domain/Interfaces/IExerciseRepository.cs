using FiTrackerV2.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiTrackerV2.Domain.Interfaces
{
    public interface IExerciseRepository
    {
        Task<int> CreateAsync(Exercise exercise);
        Task<Exercise> GetByIdAsync(int id);
        Task<List<Exercise>> GetAllAsync();
        Task<bool> UpdateAsync(Exercise exercise);
        Task<bool> DeleteAsync(int id);
    }
}
