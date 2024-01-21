using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns.State
{
    public class IdleState : IState
    {

        private CharacterController player;

        public IdleState(CharacterController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            player.MyAnimator.Play("Idle_MainCharacter");
            Debug.Log("Entering Idle State");
        }

        public void Update()
        {
            if (!player.IsGrounded)
            {
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.jumpState);
            }

            if (Mathf.Abs(player.MyRigidbody.velocity.x) > 0.1f)
            {
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.walkState);
            }
        }

        public void Exit()
        {
            Debug.Log("Exiting Idle State");
        }
    }
}