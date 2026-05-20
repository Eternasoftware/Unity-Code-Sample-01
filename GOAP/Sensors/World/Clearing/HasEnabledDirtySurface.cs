using _GameData_.Scripts.Entities.Items.AIItems.GOAP.Behavior;
using _GameData_.Scripts.ScriptableObjects.Items;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Game.Scripts.Core;
using Zenject;

namespace _GameData_.AI.GOAP.Sensors.World
{
    public class HasEnabledDirtySurface : LocalWorldSensorBase, IInjectable
    {
        [Inject] private GameplayComponent gameplayComponent;
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
            var tmp = aiGeneralGameplayBlackboard.cacheState;
            if (tmp == IDStateGame.clearingFloor || tmp == IDStateGame.scrubberMachine)
            {
                if (!gameplayComponent.IsProgressMax())
                {
                    return answer[1];
                }
            }
            return answer[0];
        }
    }
}