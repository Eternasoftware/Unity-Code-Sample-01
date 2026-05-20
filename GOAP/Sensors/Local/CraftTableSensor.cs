using _GameData_.Scripts.Entities.Navigation;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Zenject;

namespace _GameData_.AI.GOAP
{
    public class CraftTableSensor : LocalTargetSensorBase, IInjectable
    {
        [Inject] private NavigationComponent navigationComponent;
        private TransformTarget cache;
        public override void Created()
        {
            cache = new TransformTarget(navigationComponent.CraftTable);
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