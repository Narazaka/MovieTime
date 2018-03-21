using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTime {
    namespace Config {
        public class ScreenGroupConfig {
            /// <summary>
            /// <code>
            /// - Members: [1,2,3] # group1
            /// - Members: [5]
            /// - Members: [7,8] # group2
            /// </code>
            /// </summary>
            public List<ScreenGroup> Groups { get; set; } = new List<ScreenGroup>();

            Dictionary<int, int> _IndexToGroupSourceIndex = null;
            Dictionary<int, HashSet<int>> _GroupMembers = null;

            /// <summary>
            /// indexがグループに入っている場合グループソースインデックスを返す
            /// </summary>
            /// <param name="index">インデックス</param>
            /// <returns>グループソースインデックス</returns>
            public int? GroupSourceIndex(int index) => IndexToGroupSourceIndex.TryGetValue(index, out var sourceIndex) ? sourceIndex as int? : null;

            /// <summary>
            /// グループのメンバー数
            /// </summary>
            /// <param name="groupSourceIndex">グループソースインデックス</param>
            /// <param name="maxIndex">インデックス上限</param>
            /// <returns>メンバー数</returns>
            public int GroupMemberCount(int groupSourceIndex, int maxIndex) => GroupMembers[groupSourceIndex].Where(index => index <= maxIndex).Count();

            // 2要素以上あるグループへのマップ
            Dictionary<int, int> IndexToGroupSourceIndex {
                get {
                    if (_IndexToGroupSourceIndex == null) MakeMap();
                    return _IndexToGroupSourceIndex;
                }
            }

            Dictionary<int, HashSet<int>> GroupMembers {
                get {
                    if (_GroupMembers == null) MakeMap();
                    return _GroupMembers;
                }
            }

            void MakeMap() {
                var indexToGroupSourceIndex = new Dictionary<int, int>();
                var groupMembers = new Dictionary<int, HashSet<int>>();
                foreach (var group in Groups) {
                    var members = group.Members ?? new List<int>();
                    if (members.Count <= 1) continue; // 要素一つではグループにする必要がない
                    var sourceIndex = members.OrderBy(i => i).First();
                    groupMembers[sourceIndex] = new HashSet<int>(members);
                    foreach (var index in members) {
                        indexToGroupSourceIndex[index] = sourceIndex;
                    }
                }
                _IndexToGroupSourceIndex = indexToGroupSourceIndex;
                _GroupMembers = groupMembers;
            }

            public static ScreenGroupConfig Load() => FileConfigUtil.Load<ScreenGroupConfig>(PathBase.ScreenGroupConfig);
            public void Save() => FileConfigUtil.Save(PathBase.ScreenGroupConfig, this);
        }

        public class ScreenGroup {
            public List<int> Members { get; set; }
        }
    }
}
