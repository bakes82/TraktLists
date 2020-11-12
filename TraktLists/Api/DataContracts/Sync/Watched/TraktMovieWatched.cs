using TraktLists.Api.DataContracts.BaseModel;

namespace TraktLists.Api.DataContracts.Sync.Watched
{
    public class TraktMovieWatched : TraktMovie
    {
        public string watched_at { get; set; }
    }
}