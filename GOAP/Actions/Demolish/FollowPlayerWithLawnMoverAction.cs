using _GameData_.Scripts.Entities.AI.GOAP.Behavior;
using _GameData_.Scripts.Entities.Items.AIItems.GOAP.Behavior;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using Zenject;

namespace _GameData_.AI.GOAP.Actions
{
    public class FollowPlayerWithLawnMoverAction: ActionBase<FollowPlayerWithLawnMoverAction.Data>, IInjectable
    {
        [Inject] private GrassSurfaceBlackboard grassSurfaceBlackboard;
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            
            public float Timer { get; set; }

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
            if (grassSurfaceBlackboard.IsAvailable)
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