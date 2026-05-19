using Hoshiko.Context;
using Hoshiko.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Hoshiko.Repository.Recommend.Implement
{
    public class RecommendRepository<T> : IRecomendRepository<T> where T : class
    {
        private static readonly Random _random = new Random();

        public List<T> GetRecommendationsByGenres(List<GenreEntity> favoriteGenres, int count)
        {
            if (count <= 0)
                return new List<T>();

            List<T> result = new List<T>();

            if (favoriteGenres != null && favoriteGenres.Any())
            {
                var genreIds = favoriteGenres.Select(g => g.Id).ToList();

                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, "GenreId");
                var closure = Expression.Constant(genreIds);
                var method = typeof(List<int>).GetMethod("Contains", new[] { typeof(int) });
                var call = Expression.Call(closure, method, property);
                var lambda = Expression.Lambda<Func<T, bool>>(call, parameter);

                using (var db = new HoshikoDbContext())
                {
                    result = db.Set<T>().Where(lambda).Take(count).ToList();
                }
            }

            if (!result.Any())
            {
                int randomCount = _random.Next(1, count + 1);

                using (var db = new HoshikoDbContext())
                {
                    result = db.Set<T>()
                               .OrderBy(x => Guid.NewGuid())
                               .Take(randomCount)
                               .ToList();
                }
            }

            return result;
        }
    }
}
