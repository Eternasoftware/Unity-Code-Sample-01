using _GameData_.Scripts.Entities.Craft;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Zenject;

namespace _GameData_.AI.GOAP
{
    public class BrickGrabSensor : LocalTargetSensorBase, IInjectable
    {
        [Inject] private ShredderComponent shredderComponent;
        private TransformTarget cache;
        public override void Created()
        {
            cache = new TransformTarget(shredderComponent.BrickAITarget);
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