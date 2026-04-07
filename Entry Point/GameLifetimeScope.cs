using Somnambulo.Scripts.Runtime.Core.Models.Databases;
using Somnambulo.Scripts.Runtime.Core.Generated;
using Somnambulo.Scripts.Runtime.Infrastructure.Debugging;
using Somnambulo.Scripts.Runtime.Core.Interfaces;
using Somnambulo.Scripts.Runtime.Core.Models;
using Somnambulo.Scripts.Runtime.Infrastructure.Services;
using Sonity;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Somnambulo.Scripts.Runtime.Infrastructure.Installers
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("Databases")]
        [SerializeField] private TriggerIdsDatabase triggersIdsDatabase;
        [SerializeField] private ItemDatabase itemDatabase;
        [SerializeField] private WeaponDatabase weaponDatabase;
        [SerializeField] private ProjectileDatabase projectileDatabase;
        [SerializeField] private DoorDatabase doorDatabase;
        [SerializeField] private SocketDatabase socketDatabase;
        
        [Header("Build Info")]
        [SerializeField] private BuildInfoConfig buildInfoConfig;
        
        [Header("Debugging")] 
        [SerializeField] private bool enableSRDebugger;
        
        [Header("Infrastructure")]
        [SerializeField] private SoundManager sonitySoundManager;
        
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        protected override void Configure(IContainerBuilder builder)
        {
            // Build Info
            builder.RegisterInstance(buildInfoConfig);
            
            // Databases
            builder.RegisterInstance(triggersIdsDatabase);
            builder.RegisterInstance(weaponDatabase);
            builder.RegisterInstance(projectileDatabase);
            builder.RegisterInstance(doorDatabase);
            builder.RegisterInstance(itemDatabase);
            builder.RegisterInstance(socketDatabase);

            // Services and etc.
            if (enableSRDebugger) builder.RegisterEntryPoint<SRDebuggerService>();
            builder.Register<SceneLoaderService>(Lifetime.Singleton).As<ILevelLoader>();
            builder.RegisterComponentInNewPrefab(sonitySoundManager, Lifetime.Singleton).AsSelf();
            builder.Register<Triggers>(Lifetime.Singleton);
            
            // Force Sound Manager creation even though no one asks for it
            builder.RegisterBuildCallback(container => 
            {
                container.Resolve<SoundManager>();
            });
        }
    }
}