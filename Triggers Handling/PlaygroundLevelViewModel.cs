using System;
using Somnambulo.Scripts.Runtime.Core.Generated;
using Somnambulo.Scripts.Runtime.Core.Interfaces;
using Somnambulo.Scripts.Runtime.Core.Models;
using Somnambulo.Scripts.Runtime.Core.Models.Ids;
using Somnambulo.Scripts.Runtime.Core.Services;
using UnityEngine;

namespace Somnambulo.Scripts.Runtime.Core.ViewModels.Levels
{
    public class PlaygroundLevelViewModel : ILevelViewModel, IDisposable
    {
        private readonly TriggersEventBus triggersEventBus;
        private readonly Triggers triggers;
        private readonly ILevelLoader levelLoader;

        public PlaygroundLevelViewModel(TriggersEventBus triggersEventBus, Triggers triggers, ILevelLoader levelLoader)
        {
            this.triggersEventBus = triggersEventBus;
            this.triggers = triggers;
            this.levelLoader = levelLoader;
            Debug.Log($"[ViewModel] Constructor called. Subscribing to Bus {triggersEventBus.GetHashCode()}");
        }

        public void Initialize()
        {
            triggersEventBus.OnTriggerActivated += HandleTrigger;
        }

        private void HandleTrigger(TriggerId triggerId, GameObject activator)
        {
            Debug.Log($"[ViewModel] HEARD Event: {triggerId}");

            if (triggerId == triggers.test_trigger_1) Debug.Log($"[ViewModel] TEST TRIGGER 1");
            if (triggerId == triggers.test_trigger_2) Debug.Log($"[ViewModel] TEST TRIGGER 2");
            if (triggerId == triggers.test_trigger_3) Debug.Log($"[ViewModel] TEST TRIGGER 3");
            if (triggerId == triggers.test_trigger_4) Debug.Log($"[ViewModel] TEST TRIGGER 4");
        }

        public void Dispose()
        {
            if (triggersEventBus != null) triggersEventBus.OnTriggerActivated -= HandleTrigger;
        }
    }
}