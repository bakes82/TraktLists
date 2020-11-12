using System.Collections.Generic;
using TraktLists.Api.DataContracts.BaseModel;
using TraktLists.Api.DataContracts.Sync.Collection;
using TraktLists.Api.DataContracts.Sync.Ratings;
using TraktLists.Api.DataContracts.Sync.Watched;

namespace TraktLists.Api.DataContracts.Sync
{
    public class TraktSync<TMovie, TShow, TEpisode>
    {
        public List<TMovie> movies { get; set; }

        public List<TShow> shows { get; set; }

        public List<TEpisode> episodes { get; set; }
    }

    public class TraktSyncRated : TraktSync<TraktMovieRated, TraktShowRated, TraktEpisodeRated>
    {
    }

    public class TraktSyncWatched : TraktSync<TraktMovieWatched, TraktShowWatched, TraktEpisodeWatched>
    {
    }

    public class TraktSyncCollected : TraktSync<TraktMovieCollected, TraktShowCollected, TraktEpisodeCollected>
    {
    }

    public class TraktSyncUncollected : TraktSync<TraktMovie, TraktShowCollected, TraktEpisodeCollected>
    {
    }
}