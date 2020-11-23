namespace TraktLists.Api
{
    public static class TraktUris
    {
        public const string Id = "780354638b11da9a2e93e0c4498d6a1a0e8428a6da3cca70c98c19c8f0caeb52";
        public const string Secret = "113ea33e3283ef7349dfe0cb675aee83fe49be9ee540eb81fb1aec3f676b9f71";

        #region POST URI's

        public const string Token = @"https://api.trakt.tv/oauth/token";

        public const string SyncCollectionAdd = @"https://api.trakt.tv/sync/collection";
        public const string SyncCollectionRemove = @"https://api.trakt.tv/sync/collection/remove";
        public const string SyncWatchedHistoryAdd = @"https://api.trakt.tv/sync/history";
        public const string SyncWatchedHistoryRemove = @"https://api.trakt.tv/sync/history/remove";
        public const string SyncRatingsAdd = @"https://api.trakt.tv/sync/ratings";

        public const string ScrobbleStart = @"https://api.trakt.tv/scrobble/start";
        public const string ScrobblePause = @"https://api.trakt.tv/scrobble/pause";
        public const string ScrobbleStop = @"https://api.trakt.tv/scrobble/stop";
        #endregion

        #region GET URI's

        public const string WatchedMovies = @"https://api.trakt.tv/sync/watched/movies";
        public const string WatchedShows = @"https://api.trakt.tv/sync/watched/shows";
        public const string CollectedMovies = @"https://api.trakt.tv/sync/collection/movies?extended=metadata";
        public const string CollectedShows = @"https://api.trakt.tv/sync/collection/shows?extended=metadata";
        public const string PlaybackMovies = @"https://api.trakt.tv/sync/playback/movies";
        public const string PlaybackShows = @"https://api.trakt.tv/sync/playback/episodes";
        public const string UserLists = @"https://api.trakt.tv/users/{0}/lists/{1}/items/";

        // Recommendations
        public const string RecommendationsMovies = @"https://api.trakt.tv/recommendations/movies";
        public const string RecommendationsShows = @"https://api.trakt.tv/recommendations/shows";

        #endregion

        #region DELETE 

        // Recommendations
        public const string RecommendationsMoviesDismiss = @"https://api.trakt.tv/recommendations/movies/{0}";
        public const string RecommendationsShowsDismiss = @"https://api.trakt.tv/recommendations/shows/{0}";

        #endregion
    }
}

