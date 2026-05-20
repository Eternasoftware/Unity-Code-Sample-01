using _GameData_.AI.GOAP;
using GameData.Scripts.Entities.Trash;

namespace _GameData_.Scripts.Entities.AI.GOAP.Behavior.Items
{
    public class AIFloorScrapperComponent : AIScrapperComponent<TrashComponent>
    {
        protected override  void OnEnable()
        {
            base.OnEnable();
            AIMoveBehavior.InteractableZone += OnHandleScraperZone;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            AIMoveBehavior.InteractableZone -= OnHandleScraperZone;
        }
        
        protected virtual void OnHandleScraperZone(bool state)
        {
            if (state)
            {
                PutOnGround();
            }
            else
            {
                PutOnSpine();
            }
        }
    }
}