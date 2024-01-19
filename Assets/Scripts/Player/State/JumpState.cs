using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns.State
{
    public class JumpState : IState
    {
        private CharacterController player;

        private float jumpTime;
        private bool jumping;
        private bool jumpCancelled;
        // pass in any parameters you need in the constructors
        public JumpState(CharacterController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            // code that runs when we first enter the state
            Debug.Log("Entering Jump State");
            
            if (player.IsGrounded)
            {
                
                float jumpForce = Mathf.Sqrt(player.JumpHeight * -2 * (Physics2D.gravity.y * player.MyRigidbody.gravityScale));
                player.MyRigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                jumping = true;
                jumpCancelled = false;
                jumpTime = 0;
                player.jumpDisabled = false;
                //player.MyAnimator.Play(PLAYER_JUMP_Up);
            }
            
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            if (jumping)
            {
                
                jumpTime += Time.deltaTime;

                if (player.jumpDisabled)
                {
                    Debug.Log("jump canceled");
                    jumpCancelled = true;
                }

                if (jumpTime > player.ButtonHoldingTime)
                {
                    jumping = false;
                }
            }

            if (player.IsGrounded)
            {
                if (Mathf.Abs(player.MyRigidbody.velocity.x) < 0.1f && Mathf.Abs(player.MyRigidbody.velocity.y) < 0.1f)
                {
                    player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.idleState);
                }
                else if (player.moveVector.x != 0f && Mathf.Abs(player.MyRigidbody.velocity.y) < 0.1f)

                {
                    player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.walkState);
                }
            }
        }

        public void FixedUpdate()
        {
            Debug.Log(jumpCancelled);
            Debug.Log(jumping);
            if (jumpCancelled && jumping && player.MyRigidbody.velocity.y > 0)
            {
                Debug.Log("reduce jump force");
                player.MyRigidbody.AddForce(Vector2.down * player.CancelRate);
            }
        }

        public void Exit()
        {
            // code that runs when we exit the state
            Debug.Log("Exiting Jump State");
            player.jumpTrigger = false;
            player.jumpDisabled = false;
        }

    }
}