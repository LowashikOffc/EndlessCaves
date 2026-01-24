using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public class BaseState
    {
        public StateMachine Machine;
        public CharacterController CharacterController;

        public BaseState(StateMachine stateMachine, CharacterController character) {
            Machine = stateMachine;
            CharacterController = character;
        }
        public virtual void OnEnter() {
            
        }

        public virtual void HandleInput()
        {

        }
        public virtual void OnExit() { }
        public virtual void OnPhysicsUpdate() { }
        public virtual void OnLogicUpdate() { }
    }
}
