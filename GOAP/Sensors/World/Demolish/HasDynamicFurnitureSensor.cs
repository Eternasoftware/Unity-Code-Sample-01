using _GameData_.Scripts.Entities.AI.GOAP.Behavior.Items;
using _GameData_.Scripts.Entities.Items.AIItems.GOAP.Behavior;
using _GameData_.Scripts.ScriptableObjects.Items;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Zenject;

namespace _GameData_.AI.GOAP.Sensors.World
{
    public class HasDynamicFurnitureSensor : LocalWorldSensorBase, IInjectable
    {
        [Inject] private AIGeneralGameplayBlackboard aiGeneralGameplayBlackboard;
        private SenseValue[] answer = { new SenseValue(0), new SenseValue(1), new SenseValue(2) };

        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            if (aiGeneralGameplayBlackboard.cacheState != IDStateGame.ClearingFurniture)
            {
                return answer[0];   
            }
            var tmp = references.GetCachedComponent<AIInstrumentsSwitcher>();
            if (tmp != null && tmp.CurrentInstrument != null && tmp.CurrentInstrument is AIObjectGrabber component)
            {
                if (!component.GrabberActionAvailable)
                {
                    return answer[2];
                }
            }
            if (aiGeneralGameplayBlackboard.HasDynamicFurniture())
            {
                return answer[1];
            }
            return answer[0];
        }
    }
}