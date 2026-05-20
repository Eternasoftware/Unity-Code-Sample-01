using _GameData_.Scripts.Entities.AI.GOAP.Behavior;
using _GameData_.Scripts.Entities.Items.AIItems.GOAP.Behavior;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using Game.Scripts.Core;
using Zenject;

namespace _GameData_.AI.GOAP.Actions
{
    public class FollowPlayerWithMopAction : ActionBase<FollowPlayerWithMopAction.Data>, IInjectable
    {
        [Inject] private FloorDirtySurfaceBlackboard dirtySurfaceBlackboard;
        
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            
            public float Timer { get; set; }
            
            [GetComponent]
            public WaterCapacityBlackboard WaterCapacityBlackboard { get; set; }

            [GetComponent] public AIMoveBehavior AIMoveBehavior { get; set; }
        }
        
        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, Data data)
        {
            data.Timer = 1;
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            if (dirtySurfaceBlackboard.CurrentSurface != null && dirtySurfaceBlackboard.CurrentSurface.enabled && !dirtySurfaceBlackboard.CurrentSurface.IsComplete)
            {
                return ActionRunState.Stop;
            }

            if (data.WaterCapacityBlackboard.IsWaterCapacityFull)
            {
                return ActionRunState.Stop;
            }

            data.Timer -= context.DeltaTime;
            if (!IsInRange(agent, agent.DistanceObserver.GetDistance(agent, data.Target, null), data, null))
            {
                if (data.Timer <= 0)
                {
                    data.Timer = 1;
                    data.AIMoveBehavior.UpdateDestination(data.Target);
                }
            }

            return ActionRunState.Continue;
        }

        public override void End(IMonoAgent agent, Data data)
        {
        }
    }
}