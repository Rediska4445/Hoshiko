using Hoshiko.Models;
using Hoshiko.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshiko.Controller
{
    public class MediaController
    {
        private readonly IMediaRepository<MovieItem> _moviesRepo;
        private readonly IMediaRepository<SeriesItem> _seriesRepo;
        private readonly IMediaRepository<TrackItem> _tracksRepo;

        public MediaController(
            IMediaRepository<MovieItem> moviesRepo,
            IMediaRepository<SeriesItem> seriesRepo,
            IMediaRepository<TrackItem> tracksRepo)
        {
            _moviesRepo = moviesRepo;
            _seriesRepo = seriesRepo;
            _tracksRepo = tracksRepo;
        }

        public MovieItem PlayMovie(int id) => _moviesRepo.GetById(id);
        public SeriesItem PlaySeries(int id) => _seriesRepo.GetById(id);
        public TrackItem PlayTrack(int id) => _tracksRepo.GetById(id);

        public List<MovieItem> GetMovies() => _moviesRepo.GetAll();
        public List<SeriesItem> GetSeries() => _seriesRepo.GetAll();
        public List<TrackItem> GetTracks() => _tracksRepo.GetAll();

        public List<MovieItem> SearchMovies(string query) => _moviesRepo.Search(query);
        public List<SeriesItem> SearchSeries(string query) => _seriesRepo.Search(query);
        public List<TrackItem> SearchTracks(string query) => _tracksRepo.Search(query);
    }
}
