using System.Collections.Generic;
using _GameData_.Scripts.Entities.Items.AIItems.GOAP.Behavior;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Zenject;

namespace _GameData_.AI.GOAP
{
    public class BushTargetSensor : LocalTargetSensorBase, IInjectable
    {
        [Inject] private AIGeneralGameplayBlackboard aiGeneralGameplayBlackboard;
        
        private Dictionary<IMonoAgent, TransformTarget> cache;
        
        public override void Created()
        {
            cache = new Dictionary<IMonoAgent, TransformTarget>();
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            var closet = aiGeneralGameplayBlackboard.GetBush(agent.transform.position);
            if (closet is null)
            {
                return null;
            }
            if (!cache.ContainsKey(agent))
            {
                cache[agent] = new TransformTarget(closet.transform);
                return cache[agent];
            }
            cache[agent].Transform = closet.transform;
            return cache[agent];
        }
    }
}