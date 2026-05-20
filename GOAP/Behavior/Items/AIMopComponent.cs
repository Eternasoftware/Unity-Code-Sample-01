using _GameData_.Scripts.Entities.AI.GOAP.Behavior;
using _GameData_.Scripts.Entities.AI.GOAP.Behavior.Items;
using _GameData_.Scripts.Entities.BushCutting;
using _GameData_.Scripts.Entities.Cleaning;
using _GameData_.Scripts.Entities.Player;
using _GameData_.Scripts.Entities.Player.Interfaces;
using _GameData_.Scripts.ScriptableObjects.Items;
using Game.Scripts.Core;
using UnityEngine;
using Zenject;
using ItemData = _GameData_.Scripts.Entities.Player.ItemData;

namespace _GameData_.AI.GOAP
{
    public class AIMopComponent : MonoBehaviour, IInstrumentComponent
    {
        [Inject] private GameplayComponent gameplayComponent;
        [Inject] private AIInstrumentsSwitcher aiInstrumentsSwitcher;
        [Inject] private FloorDirtySurfaceBlackboard dirtySurfaceBlackboard;

        [Header("General")]
        [SerializeField] private InstrumentItem instrumentItem;
        [SerializeField] private WashingComponent washingComponent;
        
        // [Header("SFX")]
        // [SerializeField] private List<SFXObject> sfxObjects;
        // private List<Engine> audioEngine = new List<Engine>();
        
        private BaseMovementComponent movementComponent;
        private InteractableTriggerComponent interactableTriggerComponent;
        private UnitAnimator unitAnimator;
        private static readonly int Mop = Animator.StringToHash("Mop");
        private bool isGround;
        private WaterCapacityBlackboard waterCapacityBlackboard;
        private FloorDirtSurface currentTarget;

        private bool Interactable { get; set; }

        private void Awake()
        {
            movementComponent = aiInstrumentsSwitcher.GetComponent<BaseMovementComponent>();
            unitAnimator = aiInstrumentsSwitcher.GetComponent<UnitAnimator>();
            waterCapacityBlackboard = aiInstrumentsSwitcher.GetComponent<WaterCapacityBlackboard>();
        }

        protected void OnEnable()
        {
            interactableTriggerComponent = aiInstrumentsSwitcher.gameObject.AddComponent<InteractableTriggerComponent>();
            interactableTriggerComponent.tagToCompare = "DirtySurface";
            gameplayComponent.PreFinishGameState += OnHandlePreFinishState;
            waterCapacityBlackboard.GetWaterStorage().FullCapacityAction += OnHandleFullCapacity;
            
            // if (audioEngine == null)
            // {
            //     audioEngine = new List<Engine>();
            // }
            //
            // foreach (var sfx in sfxObjects)
            // {
            //     audioEngine.Add(new Engine(sfx, movementComponent,new Vector2(0, sfx.SFXLayers[0].GetVolume())));
            // }
            
            washingComponent.WashingParticlesController.CurrentCharacter = aiInstrumentsSwitcher.transform;
            washingComponent.Setup(new Vector2Int(0,0));
            waterCapacityBlackboard.GetWaterStorage().enabled = true;
            PutOnSpine();
            Interactable = true;
        }

        private void OnHandleFullCapacity(bool obj)
        {
            if (obj)
            {
                Interactable = false;
                PutOnSpine();
            }
            else
            {
                Interactable = true;
            }
        }

        protected void OnDisable()
        {
            gameplayComponent.PreFinishGameState -= OnHandlePreFinishState;
            waterCapacityBlackboard.GetWaterStorage().FullCapacityAction -= OnHandleFullCapacity;

            waterCapacityBlackboard.GetWaterStorage().enabled = false;

            if (interactableTriggerComponent != null)
            {
                Destroy(interactableTriggerComponent);
            }
        }

        private void OnHandlePreFinishState(IDStateGame obj)
        {
            if (obj is IDStateGame.clearingFloor or IDStateGame.scrubberMachine)
            {
                Interactable = false;
                PutOnSpine();
            }
        }

        private void PutOnSpine()
        {
            unitAnimator.Animator.SetBool(Mop, false);
            transform.parent = aiInstrumentsSwitcher.GetItemData(instrumentItem).parentForIdlePosition.transform;

            waterCapacityBlackboard.GetWaterStorage().Interactable = false;
            
            PutOnZeroLocalCoordinates();
            // foreach (var aEngine in audioEngine)
            // {
            //     aEngine.Disable();
            // }
            washingComponent.Disable();
            interactableTriggerComponent?.ForceExitTrigger();

            if (isGround && currentTarget != null)
            {
                currentTarget.ExitTrigger();
                currentTarget = null;
            }
            isGround = false;
        }

        private void PutOnGround()
        {
            if (!Interactable)
            {
                return;
            }

            if (isGround)
            {
                return;
            }

            if (currentTarget != null)
            {
                currentTarget.ExitTrigger();
            }
            currentTarget = dirtySurfaceBlackboard.CurrentSurface;
            currentTarget.EnterTrigger(renderCamera:DirtSurfaceComponent.RenderCamera.Second);
            
            waterCapacityBlackboard.GetWaterStorage().Interactable = true;

            unitAnimator.Animator.SetBool(Mop, true);
            ItemData itemData = aiInstrumentsSwitcher.GetItemData(instrumentItem);
            transform.parent = itemData.parentForInstrument.transform;
            PutOnZeroLocalCoordinates();
            // foreach (var aEngine in audioEngine)
            // {
            //     aEngine.Enable();
            // }
            
            washingComponent.Enable();
            isGround = true;
        }

        private void PutOnZeroLocalCoordinates()
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        private void Update()
        {
            if (isGround)
            {
                movementComponent.speedModificator = instrumentItem.speed[instrumentItem.middleIndex];
            }
        }
        
        private void LateUpdate()
        {
            if (interactableTriggerComponent.CurrentZone != null &&
                dirtySurfaceBlackboard.CurrentSurface != null &&
                interactableTriggerComponent.CurrentZone == dirtySurfaceBlackboard.CurrentSurface.gameObject &&
                dirtySurfaceBlackboard.IsAvailable)
            {
                PutOnGround();
            }
            else
            {
                if (isGround)
                {
                    PutOnSpine();
                }
            }
        }
    }
}