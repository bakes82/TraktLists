using System.Collections.Generic;
using TraktLists.Api.DataContracts.BaseModel;

namespace TraktLists.Api.DataContracts.Sync.Watched
{
    public class TraktShowWatched : TraktShow
    {
        public string watched_at { get; set; }

        public List<TraktSeasonWatched> seasons { get; set; }
    }
}