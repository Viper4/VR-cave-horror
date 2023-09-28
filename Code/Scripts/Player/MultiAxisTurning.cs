using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiAxisTurning : MonoBehaviour
{
    [SerializeField] InputActionProperty leftSnapTurnAction;
    [SerializeField] InputActionProperty rightSnapTurnAction;

    [SerializeField] InputActionProperty leftSmoothTurnAction;
    [SerializeField] InputActionProperty rightSmoothTurnAction;

    void Start()
    {
        
    }

    void Update()
    {
        Vector2 leftSnapTurn = leftSnapTurnAction.action.ReadValue<Vector2>();
        Vector2 rightSnapTurn = rightSnapTurnAction.action.ReadValue<Vector2>();
        //Debug.Log("Left: " + leftSnapTurn);
        //Debug.Log("Right: " + rightSnapTurn);
    }
}
