using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns.State
{
    public class AttackState : IState
    {
        private CharacterController player;

        public AttackState(CharacterController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            Debug.Log("Entering Attack State");
        }

        public void Update()
        {

        }

        public void Exit()
        {
            Debug.Log("Exiting Attack State");
        }
    }
}