using System;
using _GameData_.Scripts.Core;
using _GameData_.Scripts.Core.Interfaces;
using _GameData_.Scripts.Entities.AI.GOAP.Behavior.Interfaces;
using _GameData_.Scripts.Entities.Player;
using _GameData_.Scripts.Entities.Player.Interfaces;
using _GameData_.Scripts.ScriptableObjects.Settings;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using Pathfinding;
using UnityEngine;
using Zenject;

namespace _GameData_.Scripts.Entities.AI.GOAP.Behavior
{
    // class for simple minions for simple task
    [RequireComponent(typeof(AgentBehaviour))]
    public class AIBaseMoveBehavior : BaseMovementComponent, IConfigContainer, IRelaxSourcer
    {
        [SerializeField] public PlayerSettings settings;
        [SerializeField] private AgentBehaviour agent;
        [SerializeField] private AILerp aiLerp;
        private ITarget currentTarget;
        private bool shouldMove;
        
        public override event Action<float> UpdateSpeed;
        
        private EffectBonusComponent effectBonusComponent;
        [Inject]
        private void InitComponent(EffectBonusComponent effectBonusComponent)
        {
            this.effectBonusComponent = effectBonusComponent;
            ConfigDuplicateProtection();
        }

        public override float GetSpeed()
        {
            float speed = settings.SpeedAvatar + effectBonusComponent.MinionMovementBonus;
            if (speedModificator > 1)
            {
                speed = (settings.SpeedAvatar + effectBonusComponent.MinionMovementBonus) / speedModificator;
                if (speed <= 0f)
                {
                    speed = settings.MinSpeed;
                }
            }
            return speed;
        }

        public void ConfigDuplicateProtection()
        {
            settings = ScriptableObjectService.Get<PlayerSettings>(settings.name);
        }
        
        protected override void OnEnable()
        {
            agent.Events.OnTargetInRange += OnTargetInRange;
            agent.Events.OnTargetChanged += OnTargetChanged;
            agent.Events.OnTargetOutOfRange += OnTargetOutOfRange;
        }

        protected override void OnDisable()
        {
            agent.Events.OnTargetInRange -= OnTargetInRange;
            agent.Events.OnTargetChanged -= OnTargetChanged;
            agent.Events.OnTargetOutOfRange -= OnTargetOutOfRange;
        }
        
        private void OnTargetInRange(ITarget target)
        {
            shouldMove = false;
            speed = 0;
            aiLerp.speed = 0;
            UpdateSpeed?.Invoke(0);
        }
        
        private void OnTargetChanged(ITarget target, bool inRange)
        {
            currentTarget = target;
            shouldMove = !inRange;
            if (currentTarget == null)
            {
                aiLerp.canMove = false;
            }
            else
            {
                aiLerp.canMove = true;
                aiLerp.destination = currentTarget.Position;
            }
        }
        
        private void OnTargetOutOfRange(ITarget target)
        {
            currentTarget = target;
            shouldMove = true;
            aiLerp.canMove = true;
            aiLerp.destination = currentTarget.Position;
        }
        
        public override void Run()
        {
            if (!shouldMove)
                return;
        
            if (currentTarget == null)
                return;
            
            float speedTarget = GetSpeed();
            speed = Mathf.Lerp(speed, speedTarget, 0.1f);
            aiLerp.speed = speed;
            UpdateSpeed?.Invoke(speed);
        }
        
        public override void FixedUpdate()
        {
            Run();
            speedModificator = Mathf.Lerp(speedModificator, 0, 0.05f);
            if (speedModificator < 0.01f)
            {
                speedModificator = 0;
            }
        }
        
        public void SetupNewOriginPosition(Vector3 originPosition)
        {
            this.originPosition = originPosition;
            cacheOriginPosition = new PositionTarget(originPosition);
        }

        public float TimeToRelax => 0.5f;
    }
}