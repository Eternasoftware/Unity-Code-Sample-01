using _GameData_.Scripts.Entities.AI.GOAP.Behavior;
using _GameData_.Scripts.Entities.GrassCutting;
using _GameData_.Scripts.Entities.Navigation;
using _GameData_.Scripts.ScriptableObjects.Items;
using Game.Scripts.Core;
using UnityEngine;
using Zenject;

namespace _GameData_.AI.GOAP
{
    public class ClosetGrassSurfaceTargetSensor : BaseClosetSurfaceTargetSensor<GrassSurfaceComponent>
    {
        [Inject] private NavigationComponent navigationComponent;
        [Inject] private GameplayComponent gameplayComponent;
        
        [Inject]
        private void Init(GrassSurfaceBlackboard surface)
        {
            SurfaceBlackboard = surface;
        }

        public override void Created()
        {
            base.Created();
            AvailableStates.Add(IDStateGame.grassCutting);
            AvailableStates.Add(IDStateGame.tractor);
            if (SurfaceBlackboard.CurrentSurface == null)
            {
                SetupNewSurface(navigationComponent.CraftTable.transform.position);
            }
        }
        
        protected override void SetupNewSurface(Vector3 position)
        {
            FindNewSurface(position, gameplayComponent.data.LevelBuildingData.GrassSurfaceComponents);
        }
    }
}