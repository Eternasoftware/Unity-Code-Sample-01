using _GameData_.Scripts.ScriptableObjects.Items;
using Game.Scripts.Core;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _GameData_.Scripts.Entities.Items.AIItems.GOAP.Behavior
{
    public class AIGeneralGameplayBlackboard : MonoBehaviour
    {
        [Inject] public GameplayComponent GameplayComponent { get; set; }

        public IDStateGame cacheState;
        
        private void OnEnable()
        {
            GameplayComponent.ChangeGameState += OnHandleGameState;
        }

        private void OnDisable()
        {
            GameplayComponent.ChangeGameState -= OnHandleGameState;
        }

        private void OnHandleGameState(IDStateGame idStateGame)
        {
            cacheState = idStateGame;
        }
        
        public Transform GetClosetFurniture(Vector3 position)
        {
            if (cacheState != IDStateGame.demolishFurniture && cacheState != IDStateGame.bulldozer)
            {
                return null;
            }
            float minDistance = float.MaxValue;
            Transform near = null;
            foreach (var container in GameplayComponent.data.LevelBuildingData.FurnitureContainer)
            {
                foreach (var furniture in container.FurnitureChildComponents)
                {
                    if (furniture.IsAvailableForDemolish() && !furniture.IsAiTarget)
                    {
                        float distance = Vector3.Distance(furniture.transform.position, position);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            near = furniture.transform;
                        }
                    }
                }
            }
            return near;
        }

        public Transform GetClosetDynamicFurniture(Vector3 position)
        {
            if (cacheState != IDStateGame.ClearingFurniture)
            {
                return null;
            }
            float minDistance = float.MaxValue;
            Transform near = null;
            foreach (var container in GameplayComponent.data.LevelBuildingData.DynamicFurnitureContainer)
            {
                foreach (var furniture in container.Childs)
                {
                    if (furniture.IsInteractable && !furniture.IsAiTarget)
                    {
                        float distance = Vector3.Distance(furniture.transform.position, position);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            near = furniture.transform;
                        }
                    }
                }
            }
            return near;
        }
        
        public Transform GetClosetOldWall(Vector3 position)
        {
            if (cacheState != IDStateGame.demolishWall && cacheState != IDStateGame.bulldozer)
            {
                return null;
            }
            float minDistance = float.MaxValue;
            Transform near = null;
            foreach (var container in GameplayComponent.data.LevelBuildingData.WallContainer)
            {
                foreach (var component in container.WallComponents)
                {
                    if (!component.IsDestroy && !component.IsAiTarget)
                    {
                        float distance = Vector3.Distance(component.transform.position, position);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            near = component.transform;
                        }
                    }
                }
            }
            return near;
        }

        public Transform GetClosetIce(Vector3 position)
        {
            if (cacheState != IDStateGame.iceCleaning)
            {
                return null;
            }
            float minDistance = float.MaxValue;
            Transform near = null;
            foreach (var container in GameplayComponent.data.LevelBuildingData.IceContainers)
            {
                foreach (var element in container.trashList)
                {
                    if (element.AvailableForDemolish() && !element.IsAITarget)
                    {
                        float distance = Vector3.Distance(element.transform.position, position);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            near = element.transform;
                        }
                    }   
                }
            }
            return near;  
        }
        
        public Transform GetClosetOldFloor(Vector3 position)
        {
            if (cacheState != IDStateGame.demolishFloor && cacheState != IDStateGame.bulldozer)
            {
                return null;
            }
            int r = Random.Range(0, 100);
            if (r >= 75)
            {
                float minDistance = float.MinValue;
                Transform near = null;
                foreach (var container in GameplayComponent.data.LevelBuildingData.FloorContainer)
                {
                    foreach (var component in container.FragmentedChildComponents)
                    {
                        foreach (var element in component.elements)
                        {
                            if (element.AvailableForDemolish() && !element.IsAITarget)
                            {
                                float distance = Vector3.Distance(component.transform.position, position);
                                if (distance > minDistance)
                                {
                                    minDistance = distance;
                                    near = element.transform;
                                }
                            }   
                        }
                    }
                }
                return near;   
            }
            else
            {
                float minDistance = float.MaxValue;
                Transform near = null;
                foreach (var container in GameplayComponent.data.LevelBuildingData.FloorContainer)
                {
                    foreach (var component in container.FragmentedChildComponents)
                    {
                        foreach (var element in component.elements)
                        {
                            if (element.AvailableForDemolish() && !element.IsAITarget)
                            {
                                float distance = Vector3.Distance(component.transform.position, position);
                                if (distance < minDistance)
                                {
                                    minDistance = distance;
                                    near = element.transform;
                                }
                            }   
                        }
                    }
                }
                return near;
            }
        }

        public Transform GetClosetBuildWall(Vector3 pos)
        {
            if (cacheState != IDStateGame.buildWalls)
            {
                return null;
            }
            float minDistance = float.MaxValue;
            float minDistance2 = float.MaxValue;
            Transform near = null;
            Transform nearIsAiTarget = null;
            foreach (var container in GameplayComponent.data.LevelBuildingData.WallContainer)
            {
                foreach (var component in container.WallComponents)
                {
                    if (component.CanReceive && !component.IsAiTarget)
                    {
                        float distance = Vector3.Distance(component.transform.position, pos);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            near = component.transform;
                        }
                    }
                    else if (component.CanReceive)
                    {
                        float distance = Vector3.Distance(component.transform.position, pos);
                        if (distance < minDistance2)
                        {
                            minDistance2 = distance;
                            nearIsAiTarget = component.transform;
                        }
                    }
                }
            }
            if (near == null)
            {
                return nearIsAiTarget;
            }
            return near; 
        }
        
        public Transform GetClosetFloorBuild(Vector3 pos)
        {
            if (cacheState != IDStateGame.decorationFloor)
            {
                return null;
            }
            float minDistance = float.MaxValue;
            float minDistance2 = float.MaxValue;
            Transform near = null;
            Transform nearIsAiTarget = null;
            foreach (var container in GameplayComponent.data.LevelBuildingData.FloorContainer)
            {
                foreach (var component in container.FragmentedChildComponents)
                {
                    if (component.CanReceive)
                    {
                        float distance = Vector3.Distance(component.transform.position, pos);
                        if (!component.IsAiTarget)
                        {
                            if (distance < minDistance)
                            {
                                minDistance = distance;
                                near = component.transform;
                            }
                        }
                        else
                        {
                            if (distance < minDistance2)
                            {
                                minDistance2 = distance;
                                nearIsAiTarget = component.transform;
                            }
                        }
                    }
                }
            }
            if (near == null)
            {
                return nearIsAiTarget;
            }
            return near; 
        }

        public Transform GetClosetDecorPlace(Vector3 position)
        {
            if (cacheState != IDStateGame.decorationFurniture)
            {
                return null;
            }
            float minDistance = float.MaxValue;
            Transform near = null;
            foreach (var container in GameplayComponent.data.LevelBuildingData.DecorationFurnitureContainer)
            {
                foreach (var component in container.ChildsDecorationFurnitureComponents)
                {
                    if (component.CanReceive && !component.IsAiTarget)
                    {
                        float distance = Vector3.Distance(component.transform.position, position);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            near = component.transform;
                        }
                    }
                }
            }
            return near;
        }

        public Transform GetBush(Vector3 position)
        {
            if (cacheState != IDStateGame.bushCutting)
            {
                return null;
            }
            float minDistance = float.MaxValue;
            Transform near = null;
            foreach (var component in GameplayComponent.data.LevelBuildingData.BushSurfaceComponents)
            {
                if (!component.IsComplete && !component.IsAITarget && !component.IsActive)
                {
                    float distance = Vector3.Distance(component.transform.position, position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        near = component.transform;
                    }
                }
            }
            return near;
        }

        public bool HasDynamicFurniture()
        {
            foreach (var container in GameplayComponent.data.LevelBuildingData.DynamicFurnitureContainer)
            {
                foreach (var furniture in container.Childs)
                {
                    if (furniture.IsInteractable && !furniture.IsAiTarget)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool HasFurniture()
        {
            if (cacheState != IDStateGame.bulldozer && cacheState != IDStateGame.demolishFurniture)
            {
                return false;
            }
            foreach (var container in GameplayComponent.data.LevelBuildingData.FurnitureContainer)
            {
                foreach (var furniture in container.FurnitureChildComponents)
                {
                    if (furniture.IsAvailableForDemolish())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool HasLeaves()
        {
            if (cacheState != IDStateGame.leavesCleaning && cacheState != IDStateGame.LeavesMachine)
            {
                return false;
            }
            return !GameplayComponent.IsProgressMax();
        }

        public bool HasIce()
        {
            if (cacheState != IDStateGame.iceCleaning)
            {
                return false;
            }
            return !GameplayComponent.IsProgressMax();
        }
        
        public bool HasSnow()
        {
            if (cacheState != IDStateGame.snowCleaning && cacheState != IDStateGame.SnowMachine)
            {
                return false;
            }
            return !GameplayComponent.IsProgressMax();
        }

        public bool HasOldFloor()
        {
            if (cacheState != IDStateGame.bulldozer && cacheState != IDStateGame.demolishFloor)
            {
                return false;
            }
            foreach (var container in GameplayComponent.data.LevelBuildingData.FloorContainer)
            {
                foreach (var component in container.FragmentedChildComponents)
                {
                    foreach (var element in component.elements)
                    {
                        if (element.AvailableForDemolish())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        
        public bool HasOldWall()
        {
            if (cacheState != IDStateGame.bulldozer && cacheState != IDStateGame.demolishWall)
            {
                return false;
            }
            foreach (var container in GameplayComponent.data.LevelBuildingData.WallContainer)
            {
                foreach (var component in container.WallComponents)
                {
                    if (!component.IsDestroy)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool HasNotFinishedWalls()
        {
            if (cacheState != IDStateGame.buildWalls)
            {
                return false;
            }
            foreach (var container in GameplayComponent.data.LevelBuildingData.WallContainer)
            {
                foreach (var component in container.WallComponents)
                {
                    if (component.CanReceive)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        
        public bool HasNotFinishedFloor()
        {
            if (cacheState != IDStateGame.decorationFloor)
            {
                return false;
            }
            foreach (var container in GameplayComponent.data.LevelBuildingData.FloorContainer)
            {
                foreach (var component in container.FragmentedChildComponents)
                {
                    if (component.CanReceive)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool HasPlaceToDecor()
        {
            if (cacheState!= IDStateGame.decorationFurniture)
            {
                return false;
            }
            foreach (var container in GameplayComponent.data.LevelBuildingData.DecorationFurnitureContainer)
            {
                foreach (var component in container.ChildsDecorationFurnitureComponents)
                {
                    if (component.CanReceive)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool HasDirtyFloor()
        {
            if (cacheState != IDStateGame.clearingFloor && cacheState != IDStateGame.scrubberMachine)
            {
                return false;
            }

            if (!GameplayComponent.IsProgressMax())
            {
                return true;
            }

            return false;
        }
    }
}