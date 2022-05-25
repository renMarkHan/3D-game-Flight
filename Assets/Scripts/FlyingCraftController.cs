using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FlyingCraftController : MonoBehaviour
{
    private float rotationAngleDegree;
    private float rotationSpeedDegree;
    private int rotationDirection;
    public Vector3 motionVelocity;
    public float motionSpeedXZ;
    private int motionDirectionXZ;
    public float motionSpeedY;
    private int motionDirectionY;
    public GameController gameController;
    private bool gameOn,inGap;

    // Start is called before the first frame update
    void Start()
    {
        rotationAngleDegree = 0;
        rotationSpeedDegree = 40;
        rotationDirection = 0;
        motionVelocity = Vector3.zero;
        motionSpeedXZ = 3;
        motionDirectionXZ = 0;
        motionSpeedY = 1f;
        motionDirectionY = 0;
        gameOn = true;
        inGap = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (gameOn)
        {
            if (Input.GetKey("right"))
            {
                rotationDirection = 1;
                Rotate();
            }

            else if (Input.GetKey("left"))
            {
                rotationDirection = -1;
                Rotate();
            }

            motionDirectionXZ = 1;
            Move();


            if (Input.GetKey("up"))
            {
                motionDirectionY = 1;
                Move();
            }

            else if (Input.GetKey("down"))
            {
                motionDirectionY = -1;
                Move();
            }
        }
        
    }

    private void Rotate()
    {
        float rotationVelocityDegree = rotationSpeedDegree * rotationDirection;
        rotationAngleDegree += rotationVelocityDegree * Time.deltaTime;

        //make sure that rotationAngleDegree within 0-360
        rotationAngleDegree = (rotationAngleDegree + 360) % 360;
        transform.Rotate(Vector3.up, rotationVelocityDegree * Time.deltaTime); //Vector.up is Y-Axis
    }
    private void Move()
    {
        //convert degree to radian
        double rotationAngleRadian = ((float)rotationAngleDegree / 360.0) * (Math.PI * 2.0);
        float motionX = (float)Math.Sin(rotationAngleRadian) * motionDirectionXZ;
        float motionZ = (float)Math.Cos(rotationAngleRadian) * motionDirectionXZ;
        float motionY = motionDirectionY;

        motionVelocity = new Vector3(motionX, 0, motionZ);

        //nomoralized vector to represent the directions of motionVelocity
        motionVelocity.Normalize();
        

        motionVelocity = new Vector3(motionVelocity.x * motionSpeedXZ, motionY * motionSpeedY, motionVelocity.z * motionSpeedXZ);
        transform.position += motionVelocity * Time.deltaTime;

        rotationDirection = 0;
        motionDirectionXZ = 0;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Obstacle")
        {
            gameOn = false;
            gameController.GameOver();
        }
        else if (c.gameObject.tag == "Ground")
        {
            gameOn = false;
            gameController.GameOver();
        }
        if (c.gameObject.tag == "Gap")
        {
            inGap = true;
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (inGap)
        {
            inGap = false;

            //note the line below is necessary to access the whole Prefab
            //obstacle object. The collider that is return only refer to the 
            //the gap cube
            GameObject parentObject = c.gameObject.transform.parent.gameObject;

            //call the score method with the correct obstacle and
            //a reference to this FlyingCraft object
            gameController.Score(parentObject, gameObject);
        }
    }

    void Stop()
    {
        gameOn = false;
    }
}
