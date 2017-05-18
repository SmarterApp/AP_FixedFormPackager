using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedFormPackager.Common.Models
{
    public class ItemScoringInformation
    {
        public string ScoringModel { get; set; }
        public string Score { get; set; }
        public string Weight { get; set; }
        public Dictionary<string, string> ScoreParameters { get; set; } = new Dictionary<string, string>();
    }
}
