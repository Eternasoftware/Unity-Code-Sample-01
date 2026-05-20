using System.Collections.Generic;
using _GameData_.Scripts.Entities.AI.GOAP.Behavior;
using _GameData_.Scripts.Entities.Decoration;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Zenject;

namespace _GameData_.AI.GOAP
{
    public class BoxFormationTargetSensor : LocalTargetSensorBase, IInjectable
    {
        [Inject] private DecorationBox decorationBox;

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
            if (!cache.ContainsKey(agent))
            {
                if (decorationBox.TryGetComponent(out PointsFormationBlackboard pointsFormationBlackboard))
                {
                    cache[agent] = new TransformTarget(pointsFormationBlackboard.Transforms[references.GetCachedComponent<WorkerBrainComponent>().MinionItem]);
                    return cache[agent];
                }
            }
            else
            {
                return cache[agent];
            }
            return null;
        }
    }
}