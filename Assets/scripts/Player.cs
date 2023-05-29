using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
    #region Components
    public Animator Anim { get; private set; }
    #endregion

    #region States
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    #endregion

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(StateMachine, this, "idle");

        MoveState = new PlayerMoveState(StateMachine, this, "move");
    }


    private void Start()
    {
        Anim = GetComponentInChildren<Animator>();

        StateMachine.Initialize(IdleState);
     

    }

    private void Update()
    {
        StateMachine.CurrentState.Update(); 
    }

}
