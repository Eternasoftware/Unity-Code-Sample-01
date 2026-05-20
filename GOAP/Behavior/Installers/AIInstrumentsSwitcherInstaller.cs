using _GameData_.AI.GOAP;
using Zenject;

namespace _GameData_.GOAP.Installers
{
    public class AIInstrumentsSwitcherInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AIInstrumentsSwitcher>()
                .FromComponentInHierarchy(gameObject)
                .AsSingle();
        }
    }
}