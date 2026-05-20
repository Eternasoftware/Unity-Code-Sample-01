using _GameData_.Scripts.Entities.AI.GOAP.Behavior;
using _GameData_.Scripts.Entities.Craft;
using _GameData_.Scripts.Entities.Player.Interfaces;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using Zenject;

namespace _GameData_.AI.GOAP.Actions
{
    public class SellBrickShopAction : ActionBase<SellBrickShopAction.Data>, IInjectable
    {
        [Inject] private RecycleShopComponent shop;
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
        }

        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, Data data)
        {
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            if (shop.IsSelling)
            {
                return ActionRunState.Continue;
            }
            if (shop.IsCanSell)
            {
                shop.Enable();
                return ActionRunState.Stop;
            }
            return ActionRunState.Continue;
        }

        public override void End(IMonoAgent agent, Data data)
        {
            Vector3 direction = shop.transform.position - agent.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            agent.transform.rotation = targetRotation;
            agent.GetComponent<AIBaseMoveBehavior>().SetupNewOriginPosition(agent.transform.position);
        }
    }
}