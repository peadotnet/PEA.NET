using System.Collections.Generic;

namespace Pea.Core
{
    public class StopDecision
    {
        public bool MustStop { get; set; }
        public List<string> Reasons { get; } = new List<string>();

        public StopDecision(bool mustStop, string reason = null)
        {
            MustStop = mustStop;
            if (!string.IsNullOrEmpty(reason)) Reasons.Add(reason);
        }
    }
}
