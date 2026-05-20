using _GameData_.Scripts.Entities.Player;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;

namespace _GameData_.AI.GOAP.Sensors.World
{
    public class CapacityBackpackSensor : LocalWorldSensorBase
    {
        public override void Created()
        {
        }
        private  SenseValue zero = new SenseValue(0);

        public override void Update()
        {
        }

        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            var component = references.GetCachedComponent<BackpackComponent>();
            if (component == null)
                return zero;
            return component.CurrentCapacity;
        }
    }
}