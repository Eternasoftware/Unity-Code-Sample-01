using _GameData_.Scripts.Entities.Navigation;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Zenject;

namespace _GameData_.AI.GOAP
{
    public class DumpTargetSensor : LocalTargetSensorBase, IInjectable
    {
        [Inject] private NavigationComponent navigation;
        private TransformTarget cache;
        public override void Created()
        {
            cache = new TransformTarget(navigation.DumpZone.transform);
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