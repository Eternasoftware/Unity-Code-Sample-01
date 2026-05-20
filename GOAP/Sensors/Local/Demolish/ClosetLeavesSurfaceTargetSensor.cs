using _GameData_.Scripts.Entities.AI.GOAP.Behavior;
using _GameData_.Scripts.Entities.LeavesClearing;
using _GameData_.Scripts.Entities.Navigation;
using _GameData_.Scripts.ScriptableObjects.Items;
using Game.Scripts.Core;
using UnityEngine;
using Zenject;

namespace _GameData_.AI.GOAP.Demolish
{
    public class ClosetLeavesSurfaceTargetSensor : BaseClosetSurfaceTargetSensor<LeavesSurfaceComponent>
    {
        [Inject] private NavigationComponent navigationComponent;
        [Inject] private GameplayComponent gameplayComponent;

        [Inject]
        private void Init(LeavesSurfaceBlackboard leavesSurfaceBlackboard)
        {
            SurfaceBlackboard = leavesSurfaceBlackboard;
        }

        public override void Created()
        {
            base.Created();
            AvailableStates.Add(IDStateGame.leavesCleaning);
            AvailableStates.Add(IDStateGame.LeavesMachine);
            
            if (SurfaceBlackboard.CurrentSurface == null)
            {
                SetupNewSurface(navigationComponent.CraftTable.transform.position);
            }
        }

        protected override void SetupNewSurface(Vector3 position)
        {
            FindNewSurface(position, gameplayComponent.data.LevelBuildingData.LeavesSurfaceComponents);
        }
    }
}