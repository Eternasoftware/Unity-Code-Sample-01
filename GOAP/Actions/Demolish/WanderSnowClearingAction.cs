using _GameData_.Scripts.Entities.AI.GOAP.Behavior;
using _GameData_.Scripts.Entities.SnowClearing;
using Zenject;

namespace _GameData_.AI.GOAP.Actions
{
    public class WanderSnowClearingAction : WanderClearingAction<SnowSurfaceComponent>
    {
        [Inject]
        private void Init(SnowSurfaceBlackboard blackboard)
        {
            SurfaceBlackboard = blackboard;
        }
    }
}