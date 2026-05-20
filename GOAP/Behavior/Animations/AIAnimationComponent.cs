using _GameData_.Scripts.Entities.Items.AIItems.GOAP.Behavior;
using _GameData_.Scripts.Entities.Player;
using Game.Scripts.Structs;
using UnityEngine;

namespace _GameData_.AI.GOAP
{
    [RequireComponent(typeof(AIMoveBehavior))]
    public class AIAnimationComponent : UnitAnimator
    {
        private AIMoveBehavior AIMoveBehavior;
        private static readonly int Wave = Animator.StringToHash("Wave");

        private void Awake()
        {
            AIMoveBehavior = GetComponent<AIMoveBehavior>();
            sfxHammerWhoosh = Instantiate(sfxHammerWhoosh);
            sfxBigHammerWhoosh = Instantiate(sfxBigHammerWhoosh);
        }

        private void OnDestroy()
        {
            Destroy(sfxHammerWhoosh);
            Destroy(sfxBigHammerWhoosh);
        }

        private void OnEnable()
        {
            AIMoveBehavior.UpdateSpeed += OnHandleSpeed;
        }

        private void OnDisable()
        {
            AIMoveBehavior.UpdateSpeed -= OnHandleSpeed;
        }

        protected override void OnHandleSpeed(float speed)
        {
            if (speed == 0)
            {
                Animator.SetFloat(AnimationsTags.Speed, 0);
            }
            else
            {
                Animator.SetFloat(AnimationsTags.Speed, Mathf.Clamp01(speed / AIMoveBehavior.settings.SpeedAvatar));
            }
        }

        public void StartWave()
        {
            Animator.SetBool(Wave, true);
        }
    }
}