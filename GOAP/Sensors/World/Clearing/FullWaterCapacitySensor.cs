using _GameData_.Scripts.Entities.AI.GOAP.Behavior;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;

namespace _GameData_.AI.GOAP.Sensors.World
{
    public class FullWaterCapacitySensor : LocalWorldSensorBase
    {
        private readonly SenseValue[] answer = { new SenseValue(0), new SenseValue(1) };

        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            var tmp = references.GetCachedComponent<WaterCapacityBlackboard>();
            if (tmp.IsWaterCapacityFull)
            {
                return answer[1];
            }
            return answer[0];
        }
    }
}