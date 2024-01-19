using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns.State
{
    public class WalkState : IState
    {
        private CharacterController player;

        public WalkState(CharacterController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            Debug.Log("Entering Walk State");
        }

        public void Update()
        {
            if (!player.IsGrounded)
            {
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.jumpState);
            }

            if (Mathf.Abs(player.MyRigidbody.velocity.x) < 0.1f)
            {
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.idleState);
            }
        }

        public void Exit()
        {
            Debug.Log("Exiting Walk State");
        }
    }
}