using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTime.PlaylistModel {
    public class ShufflePlaylistView : ICanNextSyncPlaylistView {
        public Playlist Playlist { get; }

        IList<string> PlaylistOrder { get; set; }
        int Index { get; set; }

        public ShufflePlaylistView(Playlist playlist, string initialEntry = null) {
            Playlist = playlist;
            if (initialEntry == null) {
                NewOrder();
            } else {
                PlaylistOrder = new List<string> { initialEntry };
            }
            Index = -1;
        }

        public Task<string> Next() => Task.FromResult(NextSync());
        public string NextSync() {
            var index = NextIndex();
            return PlaylistOrder[index]; // 直接入れると更新される前のPlaylistOrderを参照してしまう
        }

        int NextIndex() {
            if (++Index >= PlaylistOrder.Count) NewOrder();
            return Index;
        }

        void NewOrder() {
            var previousEntry = PlaylistOrder == null ? null : PlaylistOrder.Last();
            PlaylistOrder = Playlist.OrderBy(_ => Guid.NewGuid()).ToList();
            if (PlaylistOrder.First() == previousEntry) {
                // 次周最初の要素が直前の要素とかぶった場合はその要素を末尾に持ってきて連続再生を防ぐ
                PlaylistOrder.RemoveAt(0);
                PlaylistOrder.Add(previousEntry);
            }
            Index = 0;
        }
    }
}
