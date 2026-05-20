using _GameData_.Scripts.Entities.AI.GOAP.Behavior;
using _GameData_.Scripts.Entities.Navigation;
using _GameData_.Scripts.Entities.SnowClearing;
using _GameData_.Scripts.ScriptableObjects.Items;
using Game.Scripts.Core;
using UnityEngine;
using Zenject;

namespace _GameData_.AI.GOAP.Demolish
{
    public class ClosetSnowSurfaceTargetSensor : BaseClosetSurfaceTargetSensor<SnowSurfaceComponent>
    {
        [Inject] private NavigationComponent navigationComponent;
        [Inject] private GameplayComponent gameplayComponent;
        
        [Inject]
        private void Init(SnowSurfaceBlackboard surface)
        {
            SurfaceBlackboard = surface;
        }

        public override void Created()
        {
            base.Created();
            AvailableStates.Add(IDStateGame.snowCleaning);
            AvailableStates.Add(IDStateGame.SnowMachine);
            if (SurfaceBlackboard.CurrentSurface == null)
            {
                SetupNewSurface(navigationComponent.CraftTable.transform.position);
            }
        }
        
        protected override void SetupNewSurface(Vector3 position)
        {
            FindNewSurface(position, gameplayComponent.data.LevelBuildingData.SnowSurfaceComponents);
        }
    }
}