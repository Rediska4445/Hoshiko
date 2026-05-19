using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshiko.Models
{
    public class PivotTvRow
    {
        public DateTime StartTime { get; set; }

        public Dictionary<string, string> Channels { get; set; } = new Dictionary<string, string>();
    }
}
