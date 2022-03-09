using System;
using System.Linq;
using System.Text;
using Sitecore.Diagnostics;

namespace FX.Core.Utils
{
    public class GoalService
    {
        public void TriggerGoal(string goalId)
        {
            try{
                Sitecore.Diagnostics.Log.Info("Trigger Goal:" + goalId, this);
                FX.Core.Utils.Util.TriggerTracker(goalId);
            }
			catch (Exception ex)
			{
                Log.Error(string.Format("Failed to call TriggerTracker for goal ID {0}", goalId), ex);
            }
        }
    }
}
