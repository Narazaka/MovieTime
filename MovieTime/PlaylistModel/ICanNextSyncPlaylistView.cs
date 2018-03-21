using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTime.PlaylistModel {
    public interface ICanNextSyncPlaylistView : IPlaylistView {
        string NextSync();
    }
}
