using _GameData_.Scripts.Entities.AI.GOAP.Behavior.Items;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;

namespace _GameData_.AI.GOAP.Sensors
{
    public abstract class HasItem<T> : LocalWorldSensorBase where T : IInstrumentComponent
    {
        private SenseValue positive;
        private SenseValue negative;
        public override void Created()
        {
            positive = new SenseValue(1);
            negative = new SenseValue(0);
        }

        public override void Update()
        {
        }
        
        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            var tmp = references.GetCachedComponent<AIInstrumentsSwitcher>().CurrentInstrument;
            if (tmp is null)
            {
                return negative;
            }
            if (tmp is T)
            {
                return positive;
            }
            return negative;
        }
    }
}