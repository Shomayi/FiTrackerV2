using FiTrackerV2.Domain.Interfaces;
using FiTrackerV2.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiTrackerV2.BLL
{
    public class ExerciseService
    {
        private readonly IExerciseRepository _exerciseRepo;

        public ExerciseService(IExerciseRepository exerciseRepo)
        {
            _exerciseRepo = exerciseRepo;
        }

        public Task<int> CreateExerciseAsync(Exercise exercise) => _exerciseRepo.CreateAsync(exercise);
        public Task<Exercise> GetExerciseAsync(int id) => _exerciseRepo.GetByIdAsync(id);
        public Task<List<Exercise>> GetAllExercisesAsync() => _exerciseRepo.GetAllAsync();
        public Task<bool> UpdateExerciseAsync(Exercise exercise) => _exerciseRepo.UpdateAsync(exercise);
        public Task<bool> DeleteExerciseAsync(int id) => _exerciseRepo.DeleteAsync(id);
    }
}
