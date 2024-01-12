using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns.State
{
    public class IdleState : IState
    {

        private CharacterController player;

        // pass in any parameters you need in the constructors
        public IdleState(CharacterController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            // code that runs when we first enter the state
            Debug.Log("Entering Idle State");
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            // if we're no longer grounded, transition to jumping
            
            if (player.IsGrounded && player.PlayerInput.jumpTrigger)
            {
                
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.jumpState);
            }

            // if we move above a minimum threshold, transition to walking
            /*if (Mathf.Abs(player.MyRigidbody.velocity.x) > 0.1f)
            {
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.walkState);
            }*/

            if (player.PlayerInput.moveVector.x != 0f)
            {
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.walkState);
            }
        }

        public void FixedUpdate()
        {
            
        }

        public void Exit()
        {
            // code that runs when we exit the state
            Debug.Log("Exiting Idle State");
        }
    }
}