using _GameData_.Scripts.Entities.Items.AIItems.GOAP.Behavior;
using UnityEngine;
using Zenject;

namespace _GameData_.GOAP.Installers
{
    public class AIGeneralGameplayBlackboardInstaller : MonoInstaller
    {
        [SerializeField] private AIGeneralGameplayBlackboard component;
        public override void InstallBindings()
        {
            Container.Bind<AIGeneralGameplayBlackboard>()
                .FromInstance(component)
                .AsSingle();
        }
    }
}