using Hoshiko.Models.Entity;
using Hoshiko.Repository;
using Hoshiko.Repository.Program;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshiko.Controller
{
    public class TvProgramsController
    {
        private readonly IRepository<TvProgramEntity> _repository;

        public TvProgramsController()
        {
            _repository = new ProgramRepository();
        }

        public TvProgramsController(IRepository<TvProgramEntity> repository)
        {
            _repository = repository;
        }

        public List<TvProgramEntity> GetAllPrograms()
        {
            try
            {
                return _repository.GetAll();
            }
            catch
            {
                return new List<TvProgramEntity>();
            }
        }

        public List<TvProgramEntity> SearchPrograms(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return GetAllPrograms();

            try
            {
                return _repository.Search(query);
            }
            catch
            {
                return new List<TvProgramEntity>();
            }
        }

        public bool AddProgram(TvProgramEntity program, out string error)
        {
            error = null;

            if (program == null)
            {
                error = "Данные передачи не заполнены.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(program.Title))
            {
                error = "Название передачи не может быть пустым.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(program.ChannelName))
            {
                error = "Название канала не может быть пустым.";
                return false;
            }

            if (program.StartTime == DateTime.MinValue)
            {
                error = "Укажите корректное время начала передачи.";
                return false;
            }

            try
            {
                int id = _repository.Add(program);
                return id > 0;
            }
            catch (Exception ex)
            {
                error = $"Ошибка добавления в БД: {ex.Message}";
                return false;
            }
        }

        public bool DeleteProgram(int id, out string error)
        {
            error = null;
            try
            {
                bool deleted = _repository.Delete(id);
                if (!deleted)
                    error = "Передача с таким ID не найдена.";

                return deleted;
            }
            catch (Exception ex)
            {
                error = $"Ошибка при удалении: {ex.Message}";
                return false;
            }
        }

        public bool UpdateProgram(TvProgramEntity program, out string error)
        {
            error = null;
            if (program == null || program.Id <= 0)
            {
                error = "Некорректные данные для обновления.";
                return false;
            }

            try
            {
                return _repository.Update(program);
            }
            catch (Exception ex)
            {
                error = $"Ошибка при обновлении: {ex.Message}";
                return false;
            }
        }
    }
}
