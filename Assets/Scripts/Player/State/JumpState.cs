using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns.State
{
    public class JumpState : IState
    {
        private CharacterController player;

        public JumpState(CharacterController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            Debug.Log("Entering Jump State");
        }

        public void Update()
        {
            if (player.IsGrounded)
            {
                if (Mathf.Abs(player.MyRigidbody.velocity.x) < 0.1f && Mathf.Abs(player.MyRigidbody.velocity.y) < 0.1f)
                {
                    player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.idleState);
                }
                else if (Mathf.Abs(player.MyRigidbody.velocity.x) > 0.1f && Mathf.Abs(player.MyRigidbody.velocity.y) < 0.1f)
                {
                    player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.walkState);
                }
            }
        }

        public void Exit()
        {
            Debug.Log("Exiting Jump State");
        }

    }
}