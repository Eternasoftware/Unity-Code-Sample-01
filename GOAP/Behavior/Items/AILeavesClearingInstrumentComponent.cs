using _GameData_.Scripts.Entities.LeavesClearing;
using UnityEngine;
using Zenject;

namespace _GameData_.Scripts.Entities.AI.GOAP.Behavior.Items
{
    public class AILeavesClearingInstrumentComponent : AIClearingInstrumentComponent<LeavesSurfaceComponent>
    {
        [Inject] private FallenLeavesMaterialController fallenLeavesMaterialController;
        [SerializeField] private Transform pivotTrube;
        [SerializeField] private GameObject vfxWind;
        private float yH;
        
        [Inject]
        private void Init(LeavesSurfaceBlackboard blackboard)
        {
            SurfaceBlackboard = blackboard;
        }
        
        protected override void Awake()
        {
            base.Awake();
            vfxWind.transform.parent = AIInstrumentsSwitcher.transform;
            vfxWind.transform.localRotation = Quaternion.Euler(0,0,0);
            vfxWind.SetActive(false);
            yH = 0.6f;
        }

        protected override void PutOnGround()
        {
            base.PutOnGround();
            fallenLeavesMaterialController.AddTransform(pivotTrube);
        }

        protected override void PutOnSpine()
        {
            base.PutOnSpine();
            fallenLeavesMaterialController.DeleteTransform(pivotTrube);
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