using _GameData_.Scripts.Entities.AI.GOAP.Behavior;
using _GameData_.Scripts.Entities.LeavesClearing;
using Zenject;

namespace _GameData_.AI.GOAP.Actions
{
    public class WanderLeavesCutAction : WanderClearingAction<LeavesSurfaceComponent>
    {
        [Inject]
        private void Init(LeavesSurfaceBlackboard blackboard)
        {
            SurfaceBlackboard = blackboard;
        }
    }
}