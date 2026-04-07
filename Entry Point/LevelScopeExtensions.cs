using Somnambulo.Scripts.Runtime.Core.Services;
using Somnambulo.Scripts.Runtime.Infrastructure.Factories;
using Somnambulo.Scripts.Runtime.Infrastructure.Installers.Binders;
using VContainer;
using VContainer.Unity;

namespace Somnambulo.Scripts.Runtime.Infrastructure.Installers
{
    public static class LevelScopeExtensions
    {
        public static void RegisterGeneralLevelDependencies(this IContainerBuilder builder)
        {
            // 1. Local services
            builder.Register<TriggersEventBus>(Lifetime.Scoped);

            // 2. Factories
            builder.Register<ItemFactory>(Lifetime.Scoped);
            builder.Register<WeaponFactory>(Lifetime.Scoped);
            builder.Register<ProjectileFactory>(Lifetime.Scoped).AsImplementedInterfaces();

            // 3. Local binders
            builder.Register<SceneTriggersBinder>(Lifetime.Scoped);
            builder.Register<SceneDoorsBinder>(Lifetime.Scoped);
            builder.Register<SceneWeaponsBinder>(Lifetime.Scoped);
            builder.Register<SceneItemsBinder>(Lifetime.Scoped);
            builder.Register<SceneFabricCutBinder>(Lifetime.Scoped);
            builder.Register<ScenePuzzlesBinder>(Lifetime.Scoped);
            builder.Register<SceneSocketsBinder>(Lifetime.Scoped);

            // 4. Entry points
            builder.RegisterEntryPoint<SceneMasterBinder>();
            builder.RegisterEntryPoint<LevelLogicBootstrapper>();
            
            // 5. Integrations
            builder.RegisterEntryPoint<NodeCanvasGlobalBridge>();
        }
    }
}