using _GameData_.Scripts.Entities.AI.GOAP.Behavior;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using Game.Scripts.Core;
using Zenject;

namespace _GameData_.AI.GOAP.Actions
{
    public class WanderClearingFloorAction : ActionBase<WanderClearingFloorAction.Data>, IInjectable
    {
        [Inject] private FloorDirtySurfaceBlackboard dirtySurfaceBlackboard;

        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            
            [GetComponent]
            public WaterCapacityBlackboard WaterCapacityBlackboard { get; set; }
            
            public float Timer { get; set; }
        }

        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, Data data)
        {
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            if (!dirtySurfaceBlackboard.IsAvailable ||
                dirtySurfaceBlackboard.IsComplete ||
                data.WaterCapacityBlackboard.IsWaterCapacityFull)
            {
                data.Timer -= context.DeltaTime;
                if (data.Timer < 0)
                {
                    return ActionRunState.Stop;
                }
            }
            else
            {
                data.Timer = 2;
            }

            if (IsInRange(agent, agent.DistanceObserver.GetDistance(agent, data.Target, null), data, null))
            {
                return ActionRunState.Stop;
            }

            return ActionRunState.Continue;
        }

        public override void End(IMonoAgent agent, Data data)
        {
        }
    }
}