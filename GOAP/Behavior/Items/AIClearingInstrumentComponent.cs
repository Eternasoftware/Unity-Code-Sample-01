using System.Collections.Generic;
using _GameData_.AI.GOAP;
using _GameData_.Scripts.Entities.BushCutting;
using _GameData_.Scripts.Entities.Cleaning;
using _GameData_.Scripts.Entities.GrassCutting;
using _GameData_.Scripts.Entities.Items.Interfaces;
using _GameData_.Scripts.Entities.Player;
using _GameData_.Scripts.Entities.Player.Interfaces;
using _GameData_.Scripts.Entities.Player.Machines;
using _GameData_.Scripts.ScriptableObjects.Items;
using Game.Scripts.Core;
using UnityEngine;
using Zenject;

namespace _GameData_.Scripts.Entities.AI.GOAP.Behavior.Items
{
    public class AIClearingInstrumentComponent<T> : MonoBehaviour, IInstrumentComponent, IInteractableInstrument where T : DirtSurfaceComponent
    {
        [Inject] private GrassPackSpawner grassPackSpawner;
        [Inject] private GameplayComponent gameplayComponent;
        [Inject] protected AIInstrumentsSwitcher AIInstrumentsSwitcher;
        
        [Header("General")]
        [SerializeField] protected InstrumentItem instrumentItem;
        [SerializeField] protected WashingComponent washingComponent;
        [SerializeField] protected string AnimationName = "Lawnmover";
        [SerializeField] private string tagToCollision = "Grass";
        [SerializeField] private List<IDStateGame> availableState;

        
        [Header("Audio")] [SerializeField] private List<SFXObject> sfxObjects;
        private List<Engine> audioEngine;

        protected InteractableTriggerComponent interactableTriggerComponent;
        protected BaseMovementComponent movementComponent;
        protected UnitAnimator UnitAnimator;
        protected DirtySurfaceBlackboard<T> SurfaceBlackboard;
        protected T CurrentTarget { get; set; } 

        protected bool interactable { get; set; }
        public bool IsGround { get; protected set; }
        
        public Transform Transform => AIInstrumentsSwitcher.transform;
        public bool Interactable => IsGround && interactable;

        protected virtual void Awake()
        {
            movementComponent = AIInstrumentsSwitcher.GetComponent<BaseMovementComponent>();
            UnitAnimator = AIInstrumentsSwitcher.GetComponent<UnitAnimator>();
        }

        protected virtual void OnEnable()
        {
            gameplayComponent.PreFinishGameState += OnHandlePreFinishState;
            grassPackSpawner.AddInteractableInstrument(this);

            interactableTriggerComponent = AIInstrumentsSwitcher.gameObject.AddComponent<InteractableTriggerComponent>();
            interactableTriggerComponent.tagToCompare = tagToCollision;

            audioEngine = new List<Engine>();
            foreach (var sfx in sfxObjects)
            {
                audioEngine.Add(new Engine(sfx, movementComponent, new Vector2(sfx.SFXLayers[0].GetVolume(), sfx.SFXLayers[0].GetVolume() * 2), 0.2f));
            }

            washingComponent.WashingParticlesController.CurrentCharacter = AIInstrumentsSwitcher.transform;
            washingComponent.Setup(new Vector2Int(0, 0));
            PutOnSpine();
            interactable = true;
        }

        private void OnDisable()
        {
            gameplayComponent.PreFinishGameState -= OnHandlePreFinishState;
            grassPackSpawner.DeleteInteractableInstrument(this);
            if (interactableTriggerComponent != null)
            {
                Destroy(interactableTriggerComponent);
            }
            UnitAnimator.Animator.SetBool(AnimationName, false);

        }

        protected virtual void OnHandlePreFinishState(IDStateGame obj)
        {
            if (availableState.Contains(obj))
            {
                interactable = false;
                PutOnSpine();   
            }
        }

        public virtual void EnableAudio()
        {
            foreach (var aEngine in audioEngine)
            {
                aEngine.Enable();
            }
        }

        public virtual void DisableAudio()
        {
            foreach (var aEngine in audioEngine)
            {
                aEngine.Disable();
            }
        }
        
        protected virtual void PutOnSpine()
        {
            UnitAnimator.Animator.SetBool(AnimationName, false);
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

        protected virtual void PutOnGround()
        {
            if (!interactable)
            {
                return;
            }

            if (IsGround)
            {
                return;
            }

            if (CurrentTarget != null)
            {
                CurrentTarget.ExitTrigger();
            }

            CurrentTarget = SurfaceBlackboard.CurrentSurface;
            CurrentTarget.EnterTrigger(renderCamera: DirtSurfaceComponent.RenderCamera.Second);

            UnitAnimator.Animator.SetBool(AnimationName, true);
            var itemData = AIInstrumentsSwitcher.GetItemData(instrumentItem);
            transform.parent = itemData.parentForInstrument.transform;
            PutOnZeroLocalCoordinates();
            EnableAudio();
            
            washingComponent.Enable();
            IsGround = true;
        }

        protected void PutOnZeroLocalCoordinates()
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        public virtual void Update()
        {
            if (IsGround)
            {
                movementComponent.speedModificator = instrumentItem.speed[instrumentItem.middleIndex];
            }
        }

        public virtual void LateUpdate()
        {
            if (interactableTriggerComponent.CurrentZone != null && SurfaceBlackboard.IsAvailable &&
                interactableTriggerComponent.CurrentZone == SurfaceBlackboard.CurrentSurface.gameObject)
            {
                PutOnGround();
            }
            else
            {
                if (IsGround)
                {
                    PutOnSpine();
                }
            }
        }
    }
}