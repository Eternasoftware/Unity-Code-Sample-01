using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using Zenject;

namespace _GameData_.AI.GOAP
{
    public class ZenjectGoapInjector : GoapConfigInitializerBase, IGoapInjector
    {   
        private DiContainer container;

        [Inject]
        private void Construct(DiContainer container)
        {
            this.container = container;
        }
        public override void InitConfig(GoapConfig config)
        {
            config.GoapInjector = this;
        }

        public void Inject(IActionBase action)
        {
            if (action is IInjectable)
            {
                container.Inject(action);
            }
        }

        public void Inject(IGoalBase goal)
        {
            if (goal is IInjectable)
            {
                container.Inject(goal);
            }
        }

        public void Inject(IWorldSensor worldSensor)
        {
            if (worldSensor is IInjectable)
            {
                container.Inject(worldSensor);
            }
        }

        public void Inject(ITargetSensor targetSensor)
        {
            if (targetSensor is IInjectable)
            {
                container.Inject(targetSensor);
            }
        }
    }
}