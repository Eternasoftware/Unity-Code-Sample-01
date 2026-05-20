using _GameData_.Scripts.Entities.Craft;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Zenject;

namespace _GameData_.AI.GOAP.Sensors
{
    public class HasBrickToMoveSensor : LocalWorldSensorBase, IInjectable
    {
        [Inject] private ShredderComponent shredderComponent;
        private SenseValue[] answer = { new SenseValue(0), new SenseValue(1), new SenseValue(2) };
        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            var stackEmpty = shredderComponent.itemStacks.Value.Empty;
            var tmp = references.GetCachedComponent<ItemsCollectorComponent>();
            if (tmp.IsFull)
            {
                return answer[2];
            }
            else if (!tmp.IsFull && stackEmpty && tmp.CurrentCapacity >= 1)
            {
                return answer[2];
            }
            if (!stackEmpty)
            {
                return answer[1];
            }
            return answer[0];
        }
    }
}