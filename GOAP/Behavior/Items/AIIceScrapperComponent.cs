using _GameData_.AI.GOAP;
using _GameData_.Scripts.Entities.BushCutting;
using _GameData_.Scripts.ScriptableObjects.Items;
using GameData.Scripts.Entities.Trash;

namespace _GameData_.Scripts.Entities.AI.GOAP.Behavior.Items
{
    public class AIIceScrapperComponent : AIScrapperComponent<TrashComponent>
    {
        private InteractableTriggerComponent interactableTriggerComponent;

        public override void UpdateProgress()
        {
            if (CacheStateGame == IDStateGame.iceCleaning)
            {
                GameplayComponent.CallUpdateProgressValue();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            
            interactableTriggerComponent = AIInstrumentsSwitcher.gameObject.AddComponent<InteractableTriggerComponent>();
            interactableTriggerComponent.tagToCompare = "IceSurface";
            interactableTriggerComponent.TriggerAction += OnHandleInteractableZone;
        }

        private void OnHandleInteractableZone(bool obj)
        {
            if (obj)
            {
                PutOnGround();
            }
            else
            {
                PutOnSpine();
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (interactableTriggerComponent != null)
            {
                interactableTriggerComponent.TriggerAction -= OnHandleInteractableZone;
                Destroy(interactableTriggerComponent);
            }
        }
    }
}