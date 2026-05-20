using _GameData_.AI.GOAP.Interfaces;
using CrashKonijn.Goap.Behaviours;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GameData_.AI.GOAP
{
    public enum IDBrain
    {
        BrickShopSeller,
        BrickShopMover,
    }
    
    public class SubWorkerBrainComponent : MonoBehaviour, IBrainComponent
    {
        [SerializeField] private IDBrain brain;
        [field: SerializeField] private AgentBehaviour agentBehaviour;
        
        [Button]
        public void ActivateBrain()
        {
            switch (brain)
            {
                case IDBrain.BrickShopSeller:
                    agentBehaviour.SetGoal<SellingBrickGoal>(false);
                    break;
                case IDBrain.BrickShopMover:
                    agentBehaviour.SetGoal<BrickCleaningGoal>(false);
                    break;
            }
        }
    }
}