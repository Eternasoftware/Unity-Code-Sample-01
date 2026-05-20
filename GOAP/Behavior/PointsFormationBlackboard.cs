using System.Collections.Generic;
using Game.Scripts.ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GameData_.Scripts.Entities.AI.GOAP.Behavior
{
    public class PointsFormationBlackboard : SerializedMonoBehaviour
    {
        public Dictionary<MinionItem, Transform> Transforms;
    }
}