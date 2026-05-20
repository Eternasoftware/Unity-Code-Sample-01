using UnityEngine;
using Zenject;

namespace _GameData_.GOAP.Installers
{
    public abstract class BaseMonoInstaller<T> : MonoInstaller
    {
        [SerializeField] protected T context;

        public override void InstallBindings()
        {
            Container.Bind<T>().FromInstance(context).AsSingle();
        }
    }
}