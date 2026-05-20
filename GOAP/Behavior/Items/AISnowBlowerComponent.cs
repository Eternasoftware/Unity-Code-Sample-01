using _GameData_.Scripts.Entities.SnowClearing;
using UnityEngine;
using Zenject;

namespace _GameData_.Scripts.Entities.AI.GOAP.Behavior.Items
{
    public class AISnowBlowerComponent : AIClearingInstrumentComponent<SnowSurfaceComponent>
    {
        [SerializeField] private Transform pivotTrube;
        [SerializeField] private GameObject vfxWind;
        private float yH;

        [Inject]
        private void Init(SnowSurfaceBlackboard blackboard)
        {
            SurfaceBlackboard = blackboard;
        }

        protected override void Awake()
        {
            base.Awake();
            vfxWind.transform.parent = AIInstrumentsSwitcher.transform;
            vfxWind.transform.localRotation = Quaternion.Euler(0, 0, 0);
            vfxWind.SetActive(false);
            yH = 0.6f;
        }

        protected override void PutOnSpine()
        {
            UnitAnimator.Animator.SetBool(AnimationName, true);
            transform.parent = AIInstrumentsSwitcher.GetItemData(instrumentItem).parentForIdlePosition.transform;
            
            PutOnZeroLocalCoordinates();
            DisableAudio();

            washingComponent.Disable();
            interactableTriggerComponent?.ForceExitTrigger();

            if (IsGround && CurrentTarget != null)
            {
                CurrentTarget.ExitTrigger();
                CurrentTarget = null;
            }

            IsGround = false;
        }

        public override void EnableAudio()
        {
            base.EnableAudio();
            vfxWind.SetActive(true);

        }

        public override void DisableAudio()
        {
            base.DisableAudio();
            vfxWind.SetActive(false);
        }

        public override void LateUpdate()
        {
            base.LateUpdate();
            if (vfxWind.gameObject.activeSelf)
            {
                vfxWind.transform.position = new Vector3(pivotTrube.transform.position.x, yH, pivotTrube.transform.position.z);
            }
        }
    }
}