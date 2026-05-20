using _GameData_.Scripts.Entities.AI.GOAP.Behavior;
using _GameData_.Scripts.Entities.Cleaning;
using _GameData_.Scripts.Entities.Navigation;
using _GameData_.Scripts.ScriptableObjects.Items;
using Game.Scripts.Core;
using UnityEngine;
using Zenject;

namespace _GameData_.AI.GOAP
{
    public class ClosetDirtyFloorSurfaceTargetSensor : BaseClosetSurfaceTargetSensor<FloorDirtSurface>
    {
        [Inject] private GameplayComponent gameplayComponent;
        [Inject] private NavigationComponent navigationComponent;

        [Inject]
        private void Init(FloorDirtySurfaceBlackboard component)
        {
            SurfaceBlackboard = component;
        }

        public override void Created()
        {
            base.Created();
            AvailableStates.Add(IDStateGame.clearingFloor);
            AvailableStates.Add(IDStateGame.scrubberMachine);
            
            if (SurfaceBlackboard.CurrentSurface == null)
            {
                SetupNewSurface(navigationComponent.CraftTable.transform.position);
            }
        }

        protected override void SetupNewSurface(Vector3 position)
        {
            FindNewSurface(navigationComponent.CraftTable.transform.position, gameplayComponent.data.LevelBuildingData.DirtSurfaceComponents);
        }
    }
}