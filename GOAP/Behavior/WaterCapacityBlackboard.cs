using _GameData_.Scripts.Entities.Cleaning;
using _GameData_.Scripts.Entities.Cleaning.Interfaces;
using UnityEngine;

namespace _GameData_.Scripts.Entities.AI.GOAP.Behavior
{
    public class WaterCapacityBlackboard : MonoBehaviour, IWaterStorage
    {
        [SerializeField] private WaterStorage waterStorage;

        public bool IsWaterCapacityFull => waterStorage.IsFull;

        public bool IsWaterCapacityZero => waterStorage.Capacity == 0;
        
        public int GetWaterCapacity()
        {
            return waterStorage.Capacity;
        }

        public WaterStorage GetWaterStorage()
        {
            return waterStorage;
        }
    }
}