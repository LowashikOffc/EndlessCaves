using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class MoveState : BaseState
{

    protected float speed = 150f;
    protected float rotationSpeed;

    private float horizontalInput;
    private float verticalInput;

    public MoveState(StateMachine stateMachine, CharacterController character) : base(stateMachine, character) { }


    public override void HandleInput()
    {
        base.HandleInput();
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
    }
    public override void OnEnter()
    {
        //Debug.Log("Move State Enter");
        horizontalInput = verticalInput = 0f;
        base.OnEnter();
    }

    public override void OnExit()
    {
        //Debug.Log("Move State Exit");
        base.OnExit();
    }

    public override void OnLogicUpdate()
    {
        base.OnLogicUpdate();
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            Machine.ChangeState(CharacterController.Standing);
        }
    }

    public override void OnPhysicsUpdate()
    {
        base.OnPhysicsUpdate();
        CharacterController.Move(verticalInput * speed, horizontalInput * speed);

    }
}
