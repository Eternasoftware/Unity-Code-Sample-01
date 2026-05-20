using _GameData_.Scripts.Entities.Decoration;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Zenject;

namespace _GameData_.AI.GOAP
{
    public class BoxMaterialTargetSensor : LocalTargetSensorBase, IInjectable
    {
        [Inject] private DecorationBox decorationBox;

        private TransformTarget cache;
        public override void Created()
        {
            cache = new TransformTarget(decorationBox.transform);
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