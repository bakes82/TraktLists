using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Model.Tasks;

namespace TraktLists
{
    public class ChannelUpdateScheduledTaskP7 : IScheduledTask, IConfigurableScheduledTask
    {
        private ITaskManager TaskManager { get; set; }

        public ChannelUpdateScheduledTaskP7(ITaskManager taskMan)
        {
            TaskManager = taskMan;
        }
        public async Task Execute(CancellationToken cancellationToken, IProgress<double> progress)
        {
            foreach (var t in TaskManager.ScheduledTasks)
            {
                if (t.Name == "Refresh Internet Channels")
                {
                    await TaskManager.Execute(t, new TaskOptions());
                }
            }
        }

        public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
        {
            return new[]
            {
                new TaskTriggerInfo
                {
                    Type          = TaskTriggerInfo.TriggerInterval,
                    IntervalTicks = TimeSpan.FromDays(1).Ticks
                }
            };
        }

        private readonly TraktChannel _traktChannel =
            Plugin.Instance.PluginConfiguration.PreConfiguredTraktChannels.Single(x => x.Id == 7);
        public string Name        => "Update Channel: " + _traktChannel.ChannelName;
        public string Key         => "PreConfigured" + _traktChannel.Id;
        public string Description => "Create/Update channel from trakt list.";
        public string Category    => "Trakt List Channels";
        public bool IsHidden      => false;
        public bool IsEnabled     => true;
        public bool IsLogged      => true;
    }
}