using TraktLists.Api.DataContracts.BaseModel;

namespace TraktLists.Api.DataContracts.Sync.Watched
{
    public class TraktEpisodeWatched : TraktEpisode
    {
        public string watched_at { get; set; }
    }
}