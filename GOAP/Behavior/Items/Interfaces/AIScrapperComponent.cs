using System;
using _GameData_.Scripts.Entities.AI.GOAP.Behavior.Items;
using _GameData_.Scripts.Entities.Items.AIItems.GOAP.Behavior;
using _GameData_.Scripts.Entities.Player;
using _GameData_.Scripts.ScriptableObjects.Items;
using _GameData_.Scripts.Stack.Pool;
using Dythervin.ObjectPool.Component;
using Game.Scripts.Core;
using GameData.Scripts.Entities.Trash;
using UnityEngine;
using Zenject;
using ItemData = _GameData_.Scripts.Entities.Player.ItemData;

namespace _GameData_.AI.GOAP
{
    public abstract class AIScrapperComponent<T> : MonoBehaviour, IInstrumentComponent where T : TrashComponent
    {
        [Inject] protected AIInstrumentsSwitcher AIInstrumentsSwitcher;
        [Inject] protected GameplayComponent GameplayComponent;
        [field:SerializeField] public InstrumentItem InstrumentItem { get; private set; }

        [SerializeField] private string animationName = "Scraper";
        [SerializeField] private int layerToCollision = 6;

        protected AIMoveBehavior AIMoveBehavior;
        protected IDStateGame CacheStateGame;

        private void Awake()
        {
            AIMoveBehavior = AIInstrumentsSwitcher.GetComponent<AIMoveBehavior>();
        }

        private void Start()
        {
            ChangeGameState(GameplayComponent.GetCurrentState());
        }

        protected virtual  void OnEnable()
        {
            GameplayComponent.ChangeGameState += ChangeGameState;
            PutOnSpine();
        }

        private void ChangeGameState(IDStateGame obj)
        {
            CacheStateGame = obj;
        }

        protected virtual void OnDisable()
        {
            GameplayComponent.ChangeGameState -= ChangeGameState;
        }

        public void Hit(TrashComponent trashComponent)
        {
            ItemPool.Instance.VfxHitPool.Get(trashComponent.transform.position, Quaternion.identity, space: Space.World).Play(true);
            if (trashComponent.sfxHitGroup != null)
            {
                trashComponent.sfxHitGroup.Play(trashComponent.transform.position);
            }
            trashComponent.Hit();
            trashComponent.Impulse(Vector3.up, 3);
            UpdateProgress();
            AIMoveBehavior.speedModificator = CollisionPower(trashComponent);
        }

        public virtual void UpdateProgress()
        {
            if (CacheStateGame == IDStateGame.bulldozer)
            {
                GameplayComponent.CallUpdateSubProgressValue();
            }
            else
            {
                GameplayComponent.CallUpdateProgressValue();
            }
        }

        public void PutOnGround()
        {
            var UnitAnimator = AIInstrumentsSwitcher.GetComponent<UnitAnimator>();
            UnitAnimator.Animator.SetBool(animationName, true);
            ItemData itemData = AIInstrumentsSwitcher.GetItemData(InstrumentItem);
            transform.parent = itemData.parentForInstrument.transform;
            Bzero();
        }

        public void PutOnSpine()
        {
            var UnitAnimator = AIInstrumentsSwitcher.GetComponent<UnitAnimator>();
            UnitAnimator.Animator.SetBool(animationName, false);
            transform.parent = AIInstrumentsSwitcher.GetItemData(InstrumentItem).parentForIdlePosition.transform;
            Bzero();
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == layerToCollision)
            {
                if (other.gameObject.TryGetComponent(out T trashComponent))
                {
                    if (!trashComponent.enabled && !trashComponent.IsDestroy)
                    {
                        Hit(trashComponent);
                    }
                }
            }
        }

        private void Bzero()
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        
        public int GetLevel(TrashComponent trashComponent)
        {
            return InstrumentItem.middleIndex;// + toolsDataComponent.GetLevel(trashComponent.ObstacleLayer);
        }
        
        public float GetSpeed(int lvl)
        {
            float deSpeed = InstrumentItem.speed[Math.Clamp(lvl, 0, InstrumentItem.speed.Count)];
            return deSpeed;
        }

        public float CollisionPower(TrashComponent trashComponent)
        {
            int lvl = GetLevel(trashComponent); 
            return GetSpeed(lvl);
        }
    }
}