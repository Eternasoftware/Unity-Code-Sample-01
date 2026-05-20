using _GameData_.Scripts.Entities.AI.GOAP.Behavior;
using _GameData_.Scripts.Entities.Cleaning;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;

namespace _GameData_.AI.GOAP.Actions
{
    public class WanderClearingAction<T> : ActionBase<WanderClearingAction<T>.Data>, IInjectable where T : DirtSurfaceComponent
    {
        protected DirtySurfaceBlackboard<T> SurfaceBlackboard;
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            public float Timer;
        }

        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, Data data)
        {
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            if (!SurfaceBlackboard.IsAvailable || SurfaceBlackboard.IsComplete)
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