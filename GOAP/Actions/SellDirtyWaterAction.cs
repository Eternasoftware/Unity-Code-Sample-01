using _GameData_.Scripts.Entities.Cleaning.Interfaces;
using _GameData_.Scripts.Entities.TriggerZones;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using Zenject;

namespace _GameData_.AI.GOAP.Actions
{
    public class SellDirtyWaterAction : ActionBase<SellDirtyWaterAction.Data>, IInjectable
    {
        [Inject] private DumpDirtyWaterComponent dumpDirtyWaterComponent;
        public class Data : IActionData
        {
            public ITarget Target { get; set; }

            [GetComponent]
            public IWaterStorage WaterStorage { get; set; }

            public bool IsSelling { get; set; }
        }

        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, Data data)
        {
            data.IsSelling = false;
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            if (!data.IsSelling && data.WaterStorage.GetWaterCapacity() > 0)
            {
                data.IsSelling = true;
                if (dumpDirtyWaterComponent.gameObject.activeSelf)
                {
                    dumpDirtyWaterComponent.SellForAI(data.WaterStorage);
                }
                else
                {
                    data.WaterStorage.GetWaterStorage().SellAllItems();
                }
            }

            if (data.WaterStorage.GetWaterCapacity() == 0)
            {
                return ActionRunState.Stop;
            }

            return ActionRunState.Continue;
        }

        public override void End(IMonoAgent agent, Data data)
        {
            data.IsSelling = false;
        }
    }
}