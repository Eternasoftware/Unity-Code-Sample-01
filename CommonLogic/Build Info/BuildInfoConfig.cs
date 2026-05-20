using Sirenix.OdinInspector;
using UnityEngine;

namespace Somnambulo.Scripts.Runtime.Infrastructure.Debugging
{
    // This config holds meta-data about the build.
    [CreateAssetMenu(fileName = "BuildInfo", menuName = "Somnambulo/Settings/Build Info")]
    public class BuildInfoConfig : ScriptableObject
    {
        [InfoBox("This config is automatically updated by build preprocessor")]
        public string BuildDate = "Unknown";
        public int BundleVersionCode;
        public string CommitHash = "Unknown";
    }
}