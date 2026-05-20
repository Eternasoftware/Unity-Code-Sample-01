using System;
using _GameData_.AI.GOAP;
using _GameData_.AI.GOAP.Actions;
using _GameData_.Scripts.Entities.Player;
using _GameData_.Scripts.ScriptableObjects.Items;
using _GameData_.Scripts.Stack.Pool;
using CrashKonijn.Goap.Behaviours;
using Dythervin.ObjectPool.Component;
using UnityEngine;
using Zenject;

namespace _GameData_.Scripts.Entities.AI.GOAP.Behavior.Items
{
    public class AIHammerComponent : MonoBehaviour, IInstrumentComponent
    {
        [Inject] private AIInstrumentsSwitcher aiInstrumentsSwitcher;

        public InstrumentItem instrumentItem;
        public SFXObject sfxHammerHit;
        public TrailRenderer trailRenderer;

        private int defaultDamage;
        private WeaponInstrumentItem weaponInstrumentItem;
        private UnitAnimator unitAnimator;
        private AgentBehaviour agentBehaviour;
        
        private void Awake()
        {
            weaponInstrumentItem = instrumentItem as WeaponInstrumentItem;
            defaultDamage = weaponInstrumentItem.DefaultDamage;
            unitAnimator = aiInstrumentsSwitcher.GetComponent<UnitAnimator>();
            agentBehaviour = aiInstrumentsSwitcher.GetComponent<AgentBehaviour>();
        }

        public void HitHammer(Vector3 position)
        {
            SFXManager.Main.Play(sfxHammerHit, position);
            ItemPool.Instance.VfxHitPool.Get(position, Quaternion.identity,
                space: Space.World).Play(true);
        }

        private void OnEnable()
        {
            unitAnimator.EndAttackHammerAction += OnHandleAttack;
        }

        private void OnDisable()
        {
            unitAnimator.EndAttackHammerAction -= OnHandleAttack;
        }

        private void OnHandleAttack()
        {
            if (agentBehaviour.CurrentActionData is AttackFurnitureAction.Data data)
            {
                data.TargetCache.Hit(GetDamage());
                HitHammer(data.Target.Position);
            }
        }

        protected int GetLevel()
        {
            return 0;
            // return toolsDataComponent.GetLevel(instrumentItem.obstaclesLayers[0]) + instrumentItem.middleIndex;
        }
        
        public int GetLevelUpgrades()
        {
            return 0;
            // return toolsDataComponent.GetLevel(instrumentItem.obstaclesLayers[0]);
        }
        
        public int GetDamage()
        {
            int lvlUpgrade = GetLevel();
            int deDamage = (int)weaponInstrumentItem.power[Math.Clamp(lvlUpgrade, 0, weaponInstrumentItem.power.Count)];
            return defaultDamage + deDamage;
        }

        public float GetSpeed()
        {
            int lvlUpgrade = GetLevel();
            float deSpeed = weaponInstrumentItem.speed[Math.Clamp(lvlUpgrade, 0, weaponInstrumentItem.speed.Count)];
            return deSpeed;
        }
    }
}