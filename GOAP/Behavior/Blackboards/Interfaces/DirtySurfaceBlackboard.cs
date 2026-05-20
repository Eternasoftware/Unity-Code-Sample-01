using _GameData_.Scripts.Entities.Cleaning;
using UnityEngine;

namespace _GameData_.Scripts.Entities.AI.GOAP.Behavior
{
    public abstract class DirtySurfaceBlackboard<T> : MonoBehaviour where T : DirtSurfaceComponent
    {
        public T CurrentSurface { get; private set; }

        public bool IsAvailable => CurrentSurface != null && CurrentSurface.enabled && !CurrentSurface.IsComplete;

        public bool IsComplete => CurrentSurface != null && CurrentSurface.IsComplete;
        
        public void BzeroSurface()
        {
            CurrentSurface = null;
        }

        public void SetSurface(T surfaceComponent)
        {
            CurrentSurface = surfaceComponent;
        }
    }
}