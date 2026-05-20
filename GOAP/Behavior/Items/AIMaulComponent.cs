using System;
using _GameData_.AI.GOAP.Actions;
using _GameData_.Scripts.Entities;
using _GameData_.Scripts.Entities.AI.GOAP.Behavior.Items;
using _GameData_.Scripts.Entities.Player;
using _GameData_.Scripts.ScriptableObjects.Items;
using _GameData_.Scripts.Stack.Pool;
using CrashKonijn.Goap.Behaviours;
using Dythervin.ObjectPool.Component;
using UnityEngine;
using Zenject;

namespace _GameData_.AI.GOAP
{
    public class AIMaulComponent : MonoBehaviour, IInstrumentComponent
    {
        [Inject] private ToolsDataComponent toolsDataComponent;
        [Inject] private AIInstrumentsSwitcher aiInstrumentsSwitcher;
        
        [Header("SFX")]
        [SerializeField] private SFXObject sfxHit;
        [SerializeField] private Transform pivotForVFX;
        public WeaponInstrumentItem weaponInstrumentItem;

        public Vector3 PivotForHit => pivotForVFX.position;

        private UnitAnimator unitAnimator;
        private AgentBehaviour agentBehaviour;
        private static readonly int BigHammer = Animator.StringToHash("BigHammer");

        private void Awake()
        {
            sfxHit = Instantiate(sfxHit);
            unitAnimator = aiInstrumentsSwitcher.GetComponent<UnitAnimator>();
            agentBehaviour = aiInstrumentsSwitcher.GetComponent<AgentBehaviour>();
        }

        private void OnDestroy()
        {
            Destroy(sfxHit);
        }

        public void Hit()
        {
            ItemPool.Instance.VfxHitPool.Get(pivotForVFX.position, Quaternion.identity, space: Space.World).Play(true);
            SFXManager.Main.Play(sfxHit,pivotForVFX.position);
        }

        private void OnEnable()
        {
            unitAnimator.EndAttackHammerAction += OnHandleAttack;
            unitAnimator.Animator.SetBool(BigHammer, true);
        }

        private void OnDisable()
        {
            unitAnimator.EndAttackHammerAction -= OnHandleAttack;
            unitAnimator.Animator.SetBool(BigHammer, false);
        }

        private void OnHandleAttack()
        {
            if (agentBehaviour.CurrentActionData is AttackOldWallAction.Data data)
            {
                data.TargetCache.Hit(GetDamage(), PivotForHit, level:GetLevelUpgrades());
                Hit();
            }
        }

        protected int GetLevel()
        {
            return 0;
            // return toolsDataComponent.GetLevel(instrumentItem.obstaclesLayers[0]) + instrumentItem.middleIndex;
        }
        
        public int GetLevelUpgrades()
        {
            return toolsDataComponent.GetLevel(weaponInstrumentItem.obstaclesLayers[0]);
        }
        
        public int GetDamage()
        {
            int lvlUpgrade = GetLevel();
            int deDamage = (int)weaponInstrumentItem.power[Math.Clamp(lvlUpgrade, 0, weaponInstrumentItem.power.Count)];
            return weaponInstrumentItem.DefaultDamage + deDamage;
        }

        public float GetSpeed()
        {
            int lvlUpgrade = GetLevel();
            float deSpeed = weaponInstrumentItem.speed[Math.Clamp(lvlUpgrade, 0, weaponInstrumentItem.speed.Count)];
            return deSpeed;
        }
    }
}