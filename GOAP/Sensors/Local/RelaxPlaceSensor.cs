using _GameData_.Scripts.Entities.Player.Interfaces;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;

namespace _GameData_.AI.GOAP
{
    public class RelaxPlaceSensor : LocalTargetSensorBase
    {
        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            return references.GetCachedComponent<BaseMovementComponent>().cacheOriginPosition;
        }
    }
}