using Hoshiko.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshiko.Repository.Recommend
{
    public interface IRecomendRepository<T>
    {
        List<T> GetRecommendationsByGenres(List<GenreEntity> favoriteGenres, int count);
    }
}
