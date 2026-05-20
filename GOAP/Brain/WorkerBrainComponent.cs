using _GameData_.AI.GOAP.Interfaces;
using _GameData_.Scripts.ScriptableObjects.Items;
using CrashKonijn.Goap.Behaviours;
using Game.Scripts.Core;
using Game.Scripts.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace _GameData_.AI.GOAP
{
    [RequireComponent(typeof(AgentBehaviour))]
    public class WorkerBrainComponent : MonoBehaviour, IBrainComponent
    {
        [Inject] private GameplayComponent gameplayComponent;
        private AgentBehaviour agentBehaviour { get; set; }
        public MinionItem MinionItem;
        
        private void Awake()
        {
            agentBehaviour = GetComponent<AgentBehaviour>();
        }

        private void OnEnable()
        {
            gameplayComponent.ChangeGameState += OnHandleChangeGameState;
        }

        public void ActivateBrain()
        {
            enabled = true;
            OnHandleChangeGameState(gameplayComponent.GetCurrentState());
        }

        private void OnDisable()
        {
            gameplayComponent.ChangeGameState -= OnHandleChangeGameState;
        }

        private void OnHandleChangeGameState(IDStateGame obj)
        {
            switch (obj)
            {
                case IDStateGame.demolishFurniture:
                    agentBehaviour.SetGoal<DemolishFurnitureGoal>(false);
                    return;
                case IDStateGame.demolishWall:
                    agentBehaviour.SetGoal<DemolishWallGoal>(false);
                    return;
                case IDStateGame.demolishFloor:
                    agentBehaviour.SetGoal<DemolishFloorGoal>(false);
                    return;
                case IDStateGame.buildWalls:
                    agentBehaviour.SetGoal<BuildWallsGoal>(false);
                    return;
                case IDStateGame.decorationFloor:
                    agentBehaviour.SetGoal<BuildFloorGoal>(false);
                    return;
                case IDStateGame.decorationFurniture:
                    agentBehaviour.SetGoal<DecorationFurnitureGoal>(false);
                    return;
                case IDStateGame.bulldozer:
                    agentBehaviour.SetGoal<RelaxGoal>(false);
                    return;
                case IDStateGame.bushCutting:
                    agentBehaviour.SetGoal<CutBushGoal>(false);
                    return;
                case IDStateGame.iceCleaning:
                    agentBehaviour.SetGoal<DemolishIceGoal>(false);
                    return;
                case IDStateGame.ClearingFurniture:
                    agentBehaviour.SetGoal<ClearingFurnitureGoal>(false);
                    return;
            }
            
            if (obj is IDStateGame.scrubberMachine or IDStateGame.clearingFloor)
            {
                agentBehaviour.SetGoal<ClearingFloorGoal>(false);
            }
            else if (obj is IDStateGame.grassCutting or IDStateGame.tractor)
            {
                agentBehaviour.SetGoal<CutGrassGoal>(false);
            }
            else if (obj is IDStateGame.leavesCleaning or IDStateGame.LeavesMachine)
            {
                agentBehaviour.SetGoal<ClearingLeavesGoal>(false);
            }
            else if (obj is IDStateGame.snowCleaning or IDStateGame.SnowMachine)
            {
                agentBehaviour.SetGoal<ClearingSnowGoal>(false);
            }
        }
    }
}