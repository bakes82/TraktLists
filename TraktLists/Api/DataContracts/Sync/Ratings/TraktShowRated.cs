using System.Collections.Generic;
using TraktLists.Api.DataContracts.BaseModel;

namespace TraktLists.Api.DataContracts.Sync.Ratings
{
    public class TraktShowRated : TraktRated
    {
        public string title { get; set; }

        public int? year { get; set; }

        public TraktShowId ids { get; set; }

        public List<TraktSeasonRated> seasons { get; set; }

        public class TraktSeasonRated : TraktRated
        {
            public int? number { get; set; }

            public List<TraktEpisodeRated> episodes { get; set; }
        }
    }
}