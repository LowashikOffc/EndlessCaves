using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class IdleState : BaseState
    {

        public IdleState(StateMachine stateMachine, CharacterController character) : base(stateMachine, character) { }
        public override void OnEnter()
        {
            //Debug.Log("Idle State Enter");
            base.OnEnter();
        }
        public override void HandleInput()
        {
            base.HandleInput();

        }
        public override void OnExit()
        {
            //Debug.Log("Idle State Exit");
            base.OnExit();
        }

        public override void OnLogicUpdate()
        {
            if (Input.GetAxis("Horizontal")!=0 || Input.GetAxis("Vertical") != 0)
            {
                Machine.ChangeState(CharacterController.Moving);
            }
            base.OnLogicUpdate();
        }

        public override void OnPhysicsUpdate()
        {
            base.OnPhysicsUpdate();
        }
    }
}
