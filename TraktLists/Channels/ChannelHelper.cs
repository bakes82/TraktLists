using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Channels;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Library;
using MediaBrowser.Model.Channels;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.MediaInfo;
using MediaBrowser.Model.Querying;
using TraktLists.Api;
using TraktLists.Helpers;

namespace TraktLists.Channels
{
    public class ChannelHelper
    {
        public async Task<ChannelItemResult> GetChannelItemResult(IUserManager userManager,
            ILibraryManager libraryManager, TraktChannel channelToConfigure, ILogger logger, TraktApi traktApi,
            CancellationToken cancellationToken)
        {
            await Plugin.Instance.SchedTaskResourcePool.WaitAsync(cancellationToken).ConfigureAwait(false);

            try
            {
                var users = userManager.Users;

                var channels = libraryManager.GetItemList(new InternalItemsQuery
                {
                    Recursive = true,
                    IncludeItemTypes = new[] {"Channel"}
                }).Where(x => !string.IsNullOrEmpty(x.Name)).ToList();


                var currentChannel = channels.First(x => x.Name == channelToConfigure.ChannelName).Id.ToString()
                    .Replace("-", string.Empty);

                if (!channelToConfigure.Enabled)
                    foreach (var user in users)
                    {
                        user.Policy.EnableAllChannels = false;

                        var enabledChannels = user.Policy.EnabledChannels != null
                            ? user.Policy.EnabledChannels.ToList()
                            : new List<string>();
                        var disabledChannels = user.Policy.BlockedChannels != null
                            ? user.Policy.BlockedChannels.ToList()
                            : new List<string>();

                        //Logger.Info("Enabled " + string.Join(",", enabledChannels.Select(x => x.ToString()).ToArray()));
                        //Logger.Info("Blocked " + string.Join(",", disabledChannels.Select(x => x.ToString()).ToArray()));

                        if (!disabledChannels.Contains(currentChannel))
                            disabledChannels.Add(currentChannel);
                        //Logger.Info("Added To Disabled " + currentChannel);

                        if (enabledChannels.Any())
                        {
                            if (enabledChannels.Contains(currentChannel)) enabledChannels.Remove(currentChannel);
                        }
                        else
                        {
                            foreach (var channel in channels.Where(x => x.Name != channelToConfigure.ChannelName))
                                enabledChannels.Add(channel.Id.ToString().Replace("-", string.Empty));
                        }

                        user.Policy.EnabledChannels = enabledChannels.ToArray();
                        user.Policy.BlockedChannels = disabledChannels.ToArray();

                        logger.Info("Enabled " + string.Join(",", enabledChannels.Select(x => x.ToString()).ToArray()));
                        logger.Info("Blocked " +
                                    string.Join(",", disabledChannels.Select(x => x.ToString()).ToArray()));

                        userManager.UpdateUser(user);
                    }
                else
                    foreach (var user in users)
                    {
                        user.Policy.EnableAllChannels = false;

                        var enabledChannels = user.Policy.EnabledChannels != null
                            ? user.Policy.EnabledChannels.ToList()
                            : new List<string>();
                        var disabledChannels = user.Policy.BlockedChannels != null
                            ? user.Policy.BlockedChannels.ToList()
                            : new List<string>();

                        //Logger.Info("Enabled " + string.Join(",", enabledChannels.Select(x => x.ToString()).ToArray()));
                        //Logger.Info("Blocked " + string.Join(",", disabledChannels.Select(x => x.ToString()).ToArray()));

                        if (disabledChannels.Contains(currentChannel)) disabledChannels.Remove(currentChannel);

                        if (enabledChannels.Any())
                        {
                            if (enabledChannels.Contains(currentChannel)) continue;

                            enabledChannels.Add(currentChannel);
                        }
                        else
                        {
                            foreach (var channel in channels)
                                enabledChannels.Add(channel.Id.ToString().Replace("-", string.Empty));
                        }

                        user.Policy.EnabledChannels = enabledChannels.ToArray();
                        user.Policy.BlockedChannels = disabledChannels.ToArray();

                        logger.Info("Enabled " + string.Join(",", enabledChannels.Select(x => x.ToString()).ToArray()));
                        logger.Info("Blocked " +
                                    string.Join(",", disabledChannels.Select(x => x.ToString()).ToArray()));

                        userManager.UpdateUser(user);
                    }

                var newItems = new List<ChannelItemInfo>();

                if (channelToConfigure.Enabled == false)
                    return await Task.FromResult(new ChannelItemResult
                    {
                        Items = newItems.ToList()
                    });

                if (string.IsNullOrEmpty(Plugin.Instance.PluginConfiguration.Pin) &&
                    string.IsNullOrEmpty(Plugin.Instance.PluginConfiguration.TraktUser.AccessToken))
                    return await Task.FromResult(new ChannelItemResult
                    {
                        Items = newItems.ToList()
                    });

                var channelLibraryItem = libraryManager.GetItemList(new InternalItemsQuery
                {
                    Name = channelToConfigure.ChannelName
                });
                
                logger.Info(channelToConfigure.ChannelName);
                logger.Info("Found " + channelLibraryItem.Length.ToString());
                

                // ReSharper disable once ComplexConditionExpression
                var ids = libraryManager.GetInternalItemIds(new InternalItemsQuery
                {
                    IncludeItemTypes = new[] {"Movie"}
                });

                var libraryItems = libraryManager.GetInternalItemIds(new InternalItemsQuery
                {
                    ParentIds = new[] {channelLibraryItem[0].InternalId}
                }).ToList();

                if (!string.IsNullOrEmpty(Plugin.Instance.PluginConfiguration.Pin))
                    Plugin.Instance.PluginConfiguration.TraktUser.PIN = Plugin.Instance.PluginConfiguration.Pin;

                var listData = await traktApi.GetTraktUserListItems(Plugin.Instance.PluginConfiguration.TraktUser,
                    channelToConfigure.TraktListUserName, channelToConfigure.TraktListName, new CancellationToken());

                logger.Info($"Count of items on list {listData.Count}");

                var mediaItems =
                    libraryManager.GetItemList(
                            new InternalItemsQuery
                            {
                                IncludeItemTypes = new[] {nameof(Movie)},
                                IsVirtualItem = false,
                                OrderBy = new[]
                                {
                                    new ValueTuple<string, SortOrder>(ItemSortBy.SeriesSortName, SortOrder.Ascending),
                                    new ValueTuple<string, SortOrder>(ItemSortBy.SortName, SortOrder.Ascending)
                                }
                            })
                        .ToList();

                logger.Info($"Count of items in emby {mediaItems.Count}");

                var foundMovies = new List<string>();
                var notFoundMovies = new List<string>();
                
                logger.Info("Internal Ids: " + string.Join(", ", libraryItems.Select(x => x.ToString()).ToArray()));

                foreach (var movie in mediaItems.OfType<Movie>())
                {
                    //Logger.Info($"Movie {movie.Name}");

                    //logger.Info(movie.InternalId.ToString());
                    if (libraryItems.Contains(movie.InternalId)) continue;

                    var foundMovie = Match.FindMatch(movie, listData);

                    if (foundMovie != null)
                    {
                        //Logger.Info($"Movie {movie.Name} found");

                        var embyMove = libraryManager.GetItemById(movie.Id);
                        if (embyMove != null)
                        {
                            foundMovies.Add(embyMove.Name);
                            //logger.Info($"Emby Movie {embyMove} found!");
                            // if(string.IsNullOrEmpty(embyMove.Path)){
                            var newMovie = new ChannelItemInfo
                            {
                                Name = embyMove.Name,
                                ImageUrl = embyMove.PrimaryImagePath,
                                Id = embyMove.Id.ToString(),
                                Type = ChannelItemType.Media,
                                ContentType = ChannelMediaContentType.Movie,
                                MediaType = ChannelMediaType.Video,
                                IsLiveStream = false,
                                MediaSources = new List<MediaSourceInfo>
                                {
                                    new ChannelMediaInfo
                                        {Path = embyMove.Path, Protocol = MediaProtocol.File}.ToMediaSource()
                                },
                                OriginalTitle = embyMove.OriginalTitle
                            };
                            if (!newItems.Any(x => x.Id == embyMove.Id.ToString()))
                            {
                                newItems.Add(newMovie);
                            }

                            // }
                        }
                    }
                    else
                    {
                        notFoundMovies.Add(movie.Name);
                        //logger.Debug($"Movie {movie.Name} not found");
                    }
                }

                logger.Info($"Count of items in List to add {newItems.Count}");

                var distinctItems = newItems.Distinct().ToList();

                logger.Info($"Count of distinct items in List to add {distinctItems.Count}");

                logger.Info("Found Movies: " + string.Join(", ", foundMovies.Select(x => x.ToString()).ToArray()));
                logger.Info(
                    "Not Found Movies: " + string.Join(", ", notFoundMovies.Select(x => x.ToString()).ToArray()));
                
                logger.Info(
                    "Not Found Movies: " + string.Join(", ", distinctItems.Select(x => x.Id.ToString()).ToArray()));


                return await Task.FromResult(new ChannelItemResult
                {
                    Items = distinctItems.ToList()
                });
            }
            finally
            {
                Plugin.Instance.SchedTaskResourcePool.Release();
            }
        }
    }
}