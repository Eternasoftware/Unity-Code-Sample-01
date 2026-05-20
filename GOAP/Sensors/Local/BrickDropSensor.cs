using _GameData_.Scripts.Entities.Craft;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Zenject;

namespace _GameData_.AI.GOAP
{
    public class BrickDropSensor : LocalTargetSensorBase, IInjectable
    {
        [Inject] private RecycleShopComponent shop;
        private TransformTarget cache;
        public override void Created()
        {
            cache = new TransformTarget(shop.aiPointToSell);
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            return cache;
        }
    }
}