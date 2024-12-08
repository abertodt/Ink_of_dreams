using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayState : State<GameManager>
{
    public GameplayState(GameManager context) : base(context) {}

    public override void Enter()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        Debug.Log("Exited Gameplay State");
        // Logic for cleanup (e.g., disabling gameplay action map)
        //InputManager.Instance.SetActionMap(null);
    }

    public override void Update()
    {
        
    }
}
