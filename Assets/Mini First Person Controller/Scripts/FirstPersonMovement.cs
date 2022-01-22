﻿using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;
    public bool canWalk = true;
    [Header("Running")]
    public bool canRun = true;
    public bool inVechile;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;

    Rigidbody rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();
    [SerializeField] GameObject vehicleTarget;
    Vector2 vehicleVelocity;
    [SerializeField] float vehicleSpeed;
    [SerializeField] float vehicleRotateSpeed;
    void Awake()
    {
        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        

        if (inVechile && vehicleTarget != null)
        {
            vehicleVelocity.x = Input.GetAxis("Horizontal") * vehicleSpeed;
            vehicleVelocity.y = Input.GetAxis("Vertical") * vehicleSpeed;

            // Apply movement.
            /*
            if (vehicleVelocity.y == 0 && vehicleVelocity.x > 0)
            {
                vehicleVelocity.y = 1 * vehicleSpeed;
            }
            vehicleTarget.GetComponent<Rigidbody>().velocity = transform.rotation * new Vector3(vehicleVelocity.x, rigidbody.velocity.y, vehicleVelocity.y);
            */
            vehicleTarget.GetComponent<Rigidbody>().velocity = vehicleTarget.transform.rotation * new Vector3(0, 0, vehicleVelocity.y);
            vehicleTarget.transform.Rotate(0.0f, Input.GetAxis("Horizontal") * vehicleRotateSpeed, 0.0f);
            return;
        }

        if (!canWalk)
        {
            return;
        }
        // Update IsRunning from input.
        IsRunning = canRun && Input.GetKey(runningKey);

        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector2 targetVelocity =new Vector2( Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

        // Apply movement.
        rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y);
    }
}