using _GameData_.Scripts.Entities.AI.GOAP.Behavior;
using _GameData_.Scripts.Entities.AI.GOAP.Behavior.Items;
using _GameData_.Scripts.Entities.BushCutting;
using _GameData_.Scripts.Entities.Cleaning;
using _GameData_.Scripts.Entities.GrassCutting;
using _GameData_.Scripts.Entities.Items.Interfaces;
using _GameData_.Scripts.Entities.Player;
using _GameData_.Scripts.Entities.Player.Interfaces;
using _GameData_.Scripts.ScriptableObjects.Items;
using _GameData_.Scripts.Stack.Pool;
using Dythervin.ObjectPool.Component;
using Game.Scripts.Core;
using UnityEngine;
using Zenject;
using ItemData = _GameData_.Scripts.Entities.Player.ItemData;

namespace _GameData_.AI.GOAP
{
    public class AILawnMowerComponent : MonoBehaviour, IInstrumentComponent, IInteractableInstrument
    {
        [Inject] private GrassPackSpawner grassPackSpawner;
        [Inject] private GameplayComponent gameplayComponent;
        [Inject] private AIInstrumentsSwitcher aiInstrumentsSwitcher;
        [Inject] private GrassSurfaceBlackboard grassSurfaceBlackboard;

        [Header("General")]
        [SerializeField] private InstrumentItem instrumentItem;
        [SerializeField] private WashingComponent washingComponent;
        
        [Header("Blade")]
        [SerializeField] private Transform blade;
        [SerializeField] private float speedRotate = 27;
        
        // [Header("Audio")]
        // [SerializeField] private List<SFXObject> sfxObjects;
        // private List<Engine> audioEngine;

        private InteractableTriggerComponent interactableTriggerComponent;
        private BaseMovementComponent movementComponent;
        private UnitAnimator unitAnimator;

        private bool interactable { get; set; }
        private bool isGround;
        private float timeFromLastVFX;
        private static readonly int Lawnmover = Animator.StringToHash("Lawnmover");

        public Transform Transform => aiInstrumentsSwitcher.transform;
        public bool Interactable => isGround && interactable;
        private GrassSurfaceComponent currentTarget;

        
        private void Awake()
        {
            movementComponent = aiInstrumentsSwitcher.GetComponent<BaseMovementComponent>();
            unitAnimator = aiInstrumentsSwitcher.GetComponent<UnitAnimator>();
        }

        private void OnEnable()
        {
            gameplayComponent.PreFinishGameState += OnHandlePreFinishState;
            washingComponent.WashingParticlesController.MoveWashingParticle += OnHandleCutMoving;

            grassPackSpawner.AddInteractableInstrument(this);

            interactableTriggerComponent = aiInstrumentsSwitcher.gameObject.AddComponent<InteractableTriggerComponent>();
            interactableTriggerComponent.tagToCompare = "Grass";
            
            // audioEngine = new List<Engine>();
            // foreach (var sfx in sfxObjects)
            // {
            //     audioEngine.Add(new Engine(sfx, movementComponent,new Vector2(sfx.SFXLayers[0].GetVolume(), sfx.SFXLayers[0].GetVolume() * 2), 0.2f));
            // }
            
            washingComponent.WashingParticlesController.CurrentCharacter = aiInstrumentsSwitcher.transform;
            washingComponent.Setup(new Vector2Int(0,0));
            PutOnSpine();
            interactable = true;
        }
        
        private void OnHandleCutMoving()
        {
            if (isGround)
            {
                if (Time.unscaledTime - timeFromLastVFX > 0.1f)
                {
                    ItemPool.Instance.VfxCutGrassPool.Get(transform.position + Vector3.up,
                        Quaternion.identity, space: Space.World).Play(true);
                    timeFromLastVFX = Time.unscaledTime;
                }
            }
        }

        private void OnDisable()
        {
            gameplayComponent.PreFinishGameState -= OnHandlePreFinishState;
            washingComponent.WashingParticlesController.MoveWashingParticle -= OnHandleCutMoving;

            grassPackSpawner.DeleteInteractableInstrument(this);
            
            if (interactableTriggerComponent != null)
            {
                Destroy(interactableTriggerComponent);
            }
        }

        private void OnHandlePreFinishState(IDStateGame obj)
        {
            if (obj is IDStateGame.grassCutting or IDStateGame.tractor)
            {
                interactable = false;
                PutOnSpine();
            }
        }

        private void PutOnSpine()
        {
            unitAnimator.Animator.SetBool(Lawnmover, false);
            transform.parent = aiInstrumentsSwitcher.GetItemData(instrumentItem).parentForIdlePosition.transform;

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
            if (!interactable)
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
            currentTarget = grassSurfaceBlackboard.CurrentSurface;
            currentTarget.EnterTrigger(renderCamera:DirtSurfaceComponent.RenderCamera.Second);
            
            unitAnimator.Animator.SetBool(Lawnmover, true);
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

        public void Update()
        {
            if (isGround)
            {
                movementComponent.speedModificator = instrumentItem.speed[instrumentItem.middleIndex];
                
                if (isGround)
                {
                    RotateBlade();
                }
            }
        }
        
        private void RotateBlade()
        {
            blade.transform.Rotate(Vector3.forward, speedRotate);
            blade.localRotation = Quaternion.Euler(0, 0, blade.eulerAngles.z);
        }
        
        public void LateUpdate()
        {
            if (interactableTriggerComponent.CurrentZone != null && grassSurfaceBlackboard.IsAvailable &&
                interactableTriggerComponent.CurrentZone == grassSurfaceBlackboard.CurrentSurface.gameObject)
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