using TraktLists.Api.DataContracts.BaseModel;

namespace TraktLists.Api.DataContracts.Users.Ratings
{
    
    public class TraktEpisodeRated : TraktRated
    {
        public TraktEpisode episode { get; set; }
    }
}