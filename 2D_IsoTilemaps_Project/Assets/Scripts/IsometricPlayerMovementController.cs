﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricPlayerMovementController : MonoBehaviour
{

    public float movementSpeed = 1f;
    IsometricCharacterRenderer isoRenderer;

    Rigidbody2D rbody;

    private void Awake()
    {
        state = State.Normal;
        rbody = GetComponent<Rigidbody2D>();
        destination = GetPosition();
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();
    }


    // Update is called once per frame
    //void FixedUpdate()
    //{
    //    Vector2 currentPos = rbody.position;
    //    float horizontalInput = Input.GetAxis("Horizontal");
    //    float verticalInput = Input.GetAxis("Vertical");
    //    Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
    //    Debug.Log("VEC " + inputVector);
    //    inputVector = Vector2.ClampMagnitude(inputVector, 1);
    //    Vector2 movement = inputVector * movementSpeed;
    //    Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
    //    isoRenderer.SetDirection(movement);
    //    rbody.MovePosition(newPos);

    //}

    private State state;
    private enum State
    {
        Normal,
        Moving,
        Attacking
    }

    private Vector3 destination;
    public void MoveTo(Vector3 targetPosition)
    {
        destination = targetPosition;
    }

    private void FixedUpdate()
    {
        //isoRenderer.SetDirection(destination); 
        rbody.MovePosition(Vector2.MoveTowards(rbody.position, destination, movementSpeed * Time.fixedDeltaTime));        
    }



    //public void MoveTo(Vector2 targetPosition, Action onReachedPosition)
    //{
    //    state = State.Moving;
    //    movePosition.SetMovePosition(targetPosition + new Vector2(1, 1), () =>
    //    {
    //        state = State.Normal;
    //        onReachedPosition();
    //    });
    //}

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
