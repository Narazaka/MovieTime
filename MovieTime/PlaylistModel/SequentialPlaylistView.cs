using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTime.PlaylistModel {
    public class SequentialPlaylistView : ICanNextSyncPlaylistView {
        public Playlist Playlist { get; }

        int Index { get; set; } = -1;

        public SequentialPlaylistView(Playlist playlist, string initialEntry = null) {
            Playlist = playlist;
            if (initialEntry != null) {
                Index = Playlist.FindIndex(entry => entry == initialEntry);
                if (Index != -1) --Index;
            }
        }

        public Task<string> Next() => Task.FromResult(NextSync());
        public string NextSync() => Playlist[NextIndex()];

        int NextIndex() => ++Index >= Playlist.Count ? Index = 0 : Index;
    }
}
