using _GameData_.Scripts.Entities.AI.GOAP.Behavior.Items;
using _GameData_.Scripts.Entities.Demolish;
using _GameData_.Scripts.Entities.Player;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;

namespace _GameData_.AI.GOAP.Actions
{
    public class AttackFurnitureAction : ActionBase<AttackFurnitureAction.Data>
    {
        public class Data : IActionData
        {
            public ITarget Target { get; set; }

            public FurnitureComponent TargetCache;

            public AIHammerComponent HammerComponent;
            
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
        
            data.TargetCache = transformTarget.Transform.GetComponent<FurnitureComponent>();
            if (data.TargetCache == null)
                return;
            data.TargetCache.IsAiTarget = true;
            data.HammerComponent = agent.GetComponent<AIInstrumentsSwitcher>().prevInstrumentGameObject.GetComponent<AIHammerComponent>();
            data.HammerComponent.trailRenderer.enabled = false;
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            if (data.UnitAnimator.IsHammerAttack)
            {
                return ActionRunState.Continue;
            }
            if (data.TargetCache == null)
            {
                return ActionRunState.Stop;
            }
            if (data.TargetCache.IsAvailableForDemolish())
            {
                Attack(data);
                return ActionRunState.Continue;
            }
            return ActionRunState.Stop;
        }

        private void Attack(Data data)
        {
            data.UnitAnimator.AttackHammer(data.HammerComponent.GetSpeed());
            data.TargetCache.objectHighlight.Highlight();
            data.HammerComponent.trailRenderer.enabled = true;
        }
        
        public override void End(IMonoAgent agent, Data data)
        {
            if (data.TargetCache is null || data.HammerComponent is null)
            {
                return;
            }
            data.TargetCache.IsAiTarget = false;
            data.HammerComponent.trailRenderer.enabled = false;
            data.TargetCache.objectHighlight.Unhighlight();
        }
    }
}