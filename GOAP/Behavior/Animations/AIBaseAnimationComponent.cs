using _GameData_.Scripts.Entities.AI.GOAP.Behavior;
using _GameData_.Scripts.Entities.Player;
using Game.Scripts.Structs;
using UnityEngine;

namespace _GameData_.AI.GOAP
{
    public class AIBaseAnimationComponent : UnitAnimator
    {
        private AIBaseMoveBehavior moveBehavior;

        private void Awake()
        {
            moveBehavior = GetComponent<AIBaseMoveBehavior>();
        }

        private void OnEnable()
        {
            moveBehavior.UpdateSpeed += OnHandleSpeed;
        }

        private void OnDisable()
        {
            moveBehavior.UpdateSpeed -= OnHandleSpeed;
        }

        protected override void OnHandleSpeed(float speed)
        {
            if (speed == 0)
            {
                Animator.SetFloat(AnimationsTags.Speed, 0);
            }
            else
            {
                Animator.SetFloat(AnimationsTags.Speed, Mathf.Clamp01(speed / moveBehavior.settings.SpeedAvatar));
            }
        }
    }
}