using _GameData_.Scripts.Entities.Demolish;
using _GameData_.Scripts.Entities.Player;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;

namespace _GameData_.AI.GOAP.Actions
{
    public class AttackOldWallAction : ActionBase<AttackOldWallAction.Data>
    {
        public class Data : IActionData
        {
            public ITarget Target { get; set; }

            public WallComponent TargetCache;

            public AIMaulComponent InstrumentComponent;
            
            [GetComponent]
            public UnitAnimator UnitAnimator { get; set; }

        }

        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, Data data)
        {
            if (data.Target is not TransformTarget)
                return;
            var transformTarget = data.Target as TransformTarget;
            
            data.TargetCache = transformTarget.Transform.GetComponent<WallComponent>();
            if (data.TargetCache == null)
                return;
            data.TargetCache.IsAiTarget = true;
            data.InstrumentComponent = agent.GetComponent<AIInstrumentsSwitcher>().prevInstrumentGameObject.GetComponent<AIMaulComponent>();
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            if (data.UnitAnimator.IsMaulAttack)
            {
                return ActionRunState.Continue;
            }
            if (data.TargetCache == null)
            {
                return ActionRunState.Stop;
            }
            if (data.TargetCache.IsAvailableForDemolish)
            {
                Attack(data);
                return ActionRunState.Continue;
            }
            data.TargetCache.IsAiTarget = false;
            return ActionRunState.Stop;
        }

        private void Attack(Data data)
        {
            data.UnitAnimator.AttackBigHammer(data.InstrumentComponent.GetSpeed());
        }

        public override void End(IMonoAgent agent, Data data)
        {
            if (data.TargetCache == null || data.InstrumentComponent == null)
                return;
            data.TargetCache.IsAiTarget = false;
        }
    }
}