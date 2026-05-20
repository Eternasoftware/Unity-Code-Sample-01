using CrashKonijn.Goap.Behaviours;
using UnityEngine;
using Zenject;

namespace _GameData_.AI.GOAP.Installers
{
    public class GoapRunnerBehaviourInstaller : MonoInstaller
    {
        [SerializeField] private GoapRunnerBehaviour context;
        
        public override void InstallBindings()
        {
            Container.Bind<GoapRunnerBehaviour>().FromInstance(context).AsSingle().Lazy();
        }
    }
}