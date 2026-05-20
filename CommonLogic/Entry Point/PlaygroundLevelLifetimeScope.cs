using Somnambulo.Scripts.Runtime.Core.Interfaces;
using Somnambulo.Scripts.Runtime.Core.ViewModels;
using Somnambulo.Scripts.Runtime.Core.ViewModels.Levels;
using Somnambulo.Scripts.Runtime.View.Player;
using VContainer;
using VContainer.Unity;

namespace Somnambulo.Scripts.Runtime.Infrastructure.Installers
{
    public class PlaygroundLevelLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<PlaygroundLevelViewModel>(Lifetime.Scoped).As<ILevelViewModel>();
                
            builder.Register<PlayerViewModel>(Lifetime.Scoped);

            builder.RegisterComponentInHierarchy<PlayerContext>();
            
            builder.RegisterGeneralLevelDependencies();
        }
    }
}