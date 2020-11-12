using TraktLists.Api.DataContracts.BaseModel;

namespace TraktLists.Api.DataContracts.Users.Ratings
{
    
    public class TraktMovieRated : TraktRated
    {
        public TraktMovie movie { get; set; }
    }
}