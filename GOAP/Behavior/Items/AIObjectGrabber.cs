using _GameData_.AI.GOAP;
using _GameData_.Scripts.Entities.Demolish;
using _GameData_.Scripts.Entities.Items.Interfaces;
using _GameData_.Scripts.Entities.Player;
using _GameData_.Scripts.Entities.Player.Interfaces;
using Zenject;

namespace _GameData_.Scripts.Entities.AI.GOAP.Behavior.Items
{
    public class AIObjectGrabber : BaseObjectGrabber, IInstrumentComponent
    {
        private AIInstrumentsSwitcher aiInstrumentsSwitcher;

        [Inject]
        private void Init(AIInstrumentsSwitcher aiInstrumentsSwitcher)
        {
            this.aiInstrumentsSwitcher = aiInstrumentsSwitcher;
        }
        
        private void Awake()
        {
            UnitAnimator = aiInstrumentsSwitcher.GetComponent<UnitAnimator>();
        }
        
        public override void OnEnable()
        {
            movementComponent = UnitAnimator.GetComponent<BaseMovementComponent>();
        }

        public override void OnDisable() {}

        public override bool StartGrabObject(DynamicFurnitureComponent dynamicFurnitureComponent)
        {
            if (!dynamicFurnitureComponent.Enter(this))
            {
                return false;
            }
            GrabObject(dynamicFurnitureComponent);
            return true;
        }
    }
}