using System;
using Somnambulo.Scripts.Runtime.Core.Models;
using Somnambulo.Scripts.Runtime.Core.Models.Ids;
using UnityEngine;

namespace Somnambulo.Scripts.Runtime.Core.Services
{
    /// <summary>
    /// Lightweight event aggregator. Decouples Triggers (View) from Logic (ViewModel/NodeCanvas).
    /// </summary>
    public class TriggersEventBus
    {
        // Event payload: TriggerID, Source (Player/Hand)
        public event Action<TriggerId, GameObject> OnTriggerActivated;

        public void Publish(TriggerId triggerId, GameObject activator)
        {
            // Debug.Log($"[BUS] Event: {triggerID} from {activator.name}");
            Debug.Log($"[BUS {this.GetHashCode()}] Publish: '{triggerId}' from {activator?.name}");
            OnTriggerActivated?.Invoke(triggerId, activator);
        }
    }
}