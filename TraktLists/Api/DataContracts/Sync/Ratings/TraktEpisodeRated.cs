using TraktLists.Api.DataContracts.BaseModel;

namespace TraktLists.Api.DataContracts.Sync.Ratings
{
    public class TraktEpisodeRated : TraktRated
    {
        public int? number { get; set; }

        public TraktEpisodeId ids { get; set; }
    }
}