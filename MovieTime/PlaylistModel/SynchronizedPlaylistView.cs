using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTime.PlaylistModel {
    public class SynchronizedPlaylistView : IPlaylistView {
        public IPlaylistView PlaylistView { get; }
        public int WorkerCount { get; }

        ConcurrentStack<Task<string>> WorkerAllDone { get; } = new ConcurrentStack<Task<string>>();
        ConcurrentStack<TaskCompletionSource<bool>> WorkerWaitSources { get; } = new ConcurrentStack<TaskCompletionSource<bool>>();

        public SynchronizedPlaylistView(IPlaylistView playlistView, int workerCount) {
            PlaylistView = playlistView;
            WorkerCount = workerCount;
            NewNextWait();
        }

        public async Task<string> Next() {
            Task<string> done;
            while (!WorkerAllDone.TryPeek(out done)) await Task.Delay(10);
            if (WorkerWaitSources.TryPop(out var source)) {
                source.SetResult(true);
            }
            return await done;
        }

        void NewNextWait() {
            var sources = Enumerable.Range(0, WorkerCount).Select(_ => new TaskCompletionSource<bool>()).ToArray();
            var whenAll = Task.WhenAll(sources.Select(source => source.Task)).ContinueWith(_ => {
                WorkerAllDone.TryPop(out var done);
                NewNextWait();
                return PlaylistView.Next().Result;
                });
            WorkerWaitSources.PushRange(sources);
            WorkerAllDone.Push(whenAll);
        }
    }
}
