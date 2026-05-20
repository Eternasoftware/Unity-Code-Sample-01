using _GameData_.Scripts.Entities.Decoration;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Zenject;

namespace _GameData_.AI.GOAP.Sensors
{
    public class HasMaterialsSensor : LocalWorldSensorBase, IInjectable
    {
        [Inject] private DecorationBox decorationBox;
        private readonly SenseValue[] answer = { new SenseValue(0), new SenseValue(1) };

        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            if (!decorationBox.Visible)
            {
                return answer[0];
            }
            var r = decorationBox.CanGiveItems();
            if (r)
            {
                return answer[1];
            }
            return answer[0];
        }
    }
}