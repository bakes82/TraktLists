using System.Collections.Generic;
using TraktLists.Api.DataContracts.BaseModel;

namespace TraktLists.Api.DataContracts.Sync.Watched
{
    public class TraktSeasonWatched : TraktSeason
    {
        public string watched_at { get; set; }

        public List<TraktEpisodeWatched> episodes { get; set; }
    }
}
