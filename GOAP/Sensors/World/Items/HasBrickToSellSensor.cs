using _GameData_.Scripts.Entities.Craft;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Zenject;

namespace _GameData_.AI.GOAP.Sensors
{
    public class HasBrickToSellSensor : LocalWorldSensorBase, IInjectable
    {
        [Inject] private RecycleShopComponent shop;
        private SenseValue[] answer = { new SenseValue(0), new SenseValue(1) };

        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            if (shop.IsCanSell)
            {
                return answer[1];
            }
            return answer[0];
        }
    }
}