using _GameData_.Scripts.Entities.Items.AIItems.GOAP.Behavior;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Zenject;

namespace _GameData_.AI.GOAP
{
    public class ClosestDynamicFurnitureTargetSensor : LocalTargetSensorBase, IInjectable
    {
        [Inject] private AIGeneralGameplayBlackboard aiGeneralGameplayBlackboard;
        private TransformTarget cache;
        public override void Created()
        {
            cache = new TransformTarget(aiGeneralGameplayBlackboard.transform);
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            var closestFurniture = aiGeneralGameplayBlackboard.GetClosetDynamicFurniture(agent.transform.position);
            if (closestFurniture is null)
                return null;
            cache.Transform = closestFurniture.transform;
            return cache;
        }
    }
}