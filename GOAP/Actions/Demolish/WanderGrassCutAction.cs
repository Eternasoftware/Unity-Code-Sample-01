using _GameData_.Scripts.Entities.AI.GOAP.Behavior;
using _GameData_.Scripts.Entities.GrassCutting;
using Zenject;

namespace _GameData_.AI.GOAP.Actions
{
    public class WanderGrassCutAction : WanderClearingAction<GrassSurfaceComponent>
    {
        [Inject]
        private void Init(GrassSurfaceBlackboard grassSurfaceBlackboard)
        {
            SurfaceBlackboard = grassSurfaceBlackboard;
        }
    }
}