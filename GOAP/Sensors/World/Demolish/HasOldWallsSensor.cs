using _GameData_.Scripts.Entities.Items.AIItems.GOAP.Behavior;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Zenject;

namespace _GameData_.AI.GOAP.Sensors.World
{
    public class HasOldWallsSensor : LocalWorldSensorBase, IInjectable
    {
        [Inject] private AIGeneralGameplayBlackboard aiGeneralGameplayBlackboard;
        private readonly SenseValue[] answer = { new SenseValue(0), new SenseValue(1) };

        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            bool t = aiGeneralGameplayBlackboard.HasOldWall();

            if (t)
            {
                return answer[1];
            }

            return answer[0];
        }
    }
}