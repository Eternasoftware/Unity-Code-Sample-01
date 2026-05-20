using System;
using _GameData_.AI.GOAP;
using _GameData_.Scripts.Core;
using _GameData_.Scripts.Core.Interfaces;
using _GameData_.Scripts.Entities.AI.GOAP.Behavior.Interfaces;
using _GameData_.Scripts.Entities.Cleaning.Interfaces;
using _GameData_.Scripts.Entities.Player;
using _GameData_.Scripts.Entities.Player.Interfaces;
using _GameData_.Scripts.ScriptableObjects.Items;
using _GameData_.Scripts.ScriptableObjects.Settings;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using Game.Scripts.Structs;
using Pathfinding;
using UnityEngine;
using Zenject;

namespace _GameData_.Scripts.Entities.Items.AIItems.GOAP.Behavior
{
    // todo refactoring this class with AIBaseMoveBehavior to combine similar logic
    // this class need more for heavy minions
    [RequireComponent(typeof(AgentBehaviour))]
    public class AIMoveBehavior : BaseMovementComponent, ICleaningSource, IConfigContainer, IRelaxSourcer
    {
        private EffectBonusComponent effectBonusComponent;
        [SerializeField] public PlayerSettings settings;
        
        [Header("Pivots")]
        [SerializeField] private Transform pivotForCutParticles;
        [SerializeField] private Transform pivotForClearingParticles;
        [SerializeField] private AgentBehaviour agent;
        [SerializeField] private AILerp aiLerp;
        private ITarget currentTarget;
        private bool shouldMove;
        
        public override event Action<float> UpdateSpeed;
        public event Action<bool> InteractableZone;

        private IDStateGame cacheCurrentGameState;

        public Vector3 Destination => aiLerp.destination;

        public AILerp AILerp => aiLerp;

        public bool InInteractableZone { get; private set; }

        [Inject]
        private void InitComponent(EffectBonusComponent effectBonusComponent)
        {
            this.effectBonusComponent = effectBonusComponent;
            ConfigDuplicateProtection();
        }
        
        public void ConfigDuplicateProtection()
        {
            settings = ScriptableObjectService.Get<PlayerSettings>(settings.name);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!InInteractableZone && other.gameObject.layer == GameLayers.InteractableZone)
            {
                InInteractableZone = true;
                InteractableZone?.Invoke(InInteractableZone);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (InInteractableZone && other.gameObject.layer == GameLayers.InteractableZone)
            {
                InInteractableZone = false;
                InteractableZone?.Invoke(InInteractableZone);
            }
        }

        protected override void OnEnable()
        {
            agent.Events.OnTargetInRange += OnTargetInRange;
            agent.Events.OnTargetChanged += OnTargetChanged;
            agent.Events.OnTargetOutOfRange += OnTargetOutOfRange;
            gameplayComponent.ChangeGameState += OnHandleChangeGameState;
            gameplayComponent.PreFinishGameState += OnHandlePreFinishState;
        }

        protected override void OnDisable()
        {
            agent.Events.OnTargetInRange -= OnTargetInRange;
            agent.Events.OnTargetChanged -= OnTargetChanged;
            agent.Events.OnTargetOutOfRange -= OnTargetOutOfRange;
            gameplayComponent.ChangeGameState -= OnHandleChangeGameState;
            gameplayComponent.PreFinishGameState -= OnHandlePreFinishState;
        }
        
        private void OnHandlePreFinishState(IDStateGame idStateGame)
        {
            cacheCurrentGameState = idStateGame;
        }


        private void OnHandleChangeGameState(IDStateGame idStateGame)
        {
            cacheCurrentGameState = idStateGame;
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
                // var tmp= AstarPath.active.data.graphs[1].active.GetNearest(currentTarget.Position, NNConstraint.Walkable).node;
                // aiLerp.destination = (Vector3)tmp.position;
            }
        }

        private void OnTargetOutOfRange(ITarget target)
        {
            currentTarget = target;
            shouldMove = true;
            aiLerp.canMove = true;
            aiLerp.destination = currentTarget.Position;
            // var tmp= AstarPath.active.data.graphs[1].active.GetNearest(currentTarget.Position, NNConstraint.Walkable).node;
            // aiLerp.destination = (Vector3)tmp.position;
        }

        public void UpdateDestination(ITarget target)
        {
            OnTargetOutOfRange(target);
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
        
        public override void FixedUpdate()
        {
            Run();
            speedModificator = Mathf.Lerp(speedModificator, 0, 0.05f);
            if (speedModificator < 0.01f)
            {
                speedModificator = 0;
            }
        }

        public Vector3 GetPositionForClearingParticles()
        {
            if (cacheCurrentGameState == IDStateGame.grassCutting)
            {
                return pivotForCutParticles.position;
            }
            return pivotForClearingParticles.position;
        }

        public void Wave()
        {
            UpdateSpeed?.Invoke(0);
            GetComponent<AIAnimationComponent>().StartWave();
        }

        public float TimeToRelax => 2;
    }
}