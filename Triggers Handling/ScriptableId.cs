using UnityEngine;

namespace Somnambulo.Scripts.Runtime.Core.Models.Ids
{
    public abstract class ScriptableId : ScriptableObject
    {
        public string StringId => name;
        public override string ToString() => StringId;
    }
}