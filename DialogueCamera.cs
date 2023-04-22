using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCamera : MonoBehaviour
{
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 1F;
    public float sensitivityY = 1F;

    public float minimumX = -5F;
    public float maximumX = 5;

    public float minimumY = -5;
    public float maximumY = 5;

    public float rotationX = 26.5F;
    public float rotationY = 10.5F;

    public Transform target;
    private bool lookAt;

    private void Awake()
    {
        rotationY *= -1;

        //rotationY = transform.rotation.y;
        //rotationX = transform.rotation.x;

        minimumY = rotationY + minimumY;
       maximumY = rotationY + maximumY;

        minimumX = rotationX + minimumX;
        maximumX = rotationX + maximumX;
    }
    public void LookAt(Transform _target)
    {
        target = _target;
        lookAt = true;
    }
    public void StopLookAt()
    {
        lookAt = false;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            transform.LookAt(target);
        }

        else
        {
            if (axes == RotationAxes.MouseXAndY)
            {
                rotationX += Input.GetAxis("Mouse X") * sensitivityX;

                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
                rotationX = Mathf.Clamp(rotationX, minimumX, maximumX);
                transform.localEulerAngles = AngleLerp(transform.localEulerAngles, new Vector3(-rotationY, rotationX, 0), Time.deltaTime);
            }
            else if (axes == RotationAxes.MouseX)
            {
                transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
            }
            else
            {
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
            }
        }
    }
    Vector3 AngleLerp(Vector3 StartAngle, Vector3 FinishAngle, float t)
    {
        float xLerp = Mathf.LerpAngle(StartAngle.x, FinishAngle.x, t);
        float yLerp = Mathf.LerpAngle(StartAngle.y, FinishAngle.y, t);
        float zLerp = Mathf.LerpAngle(StartAngle.z, FinishAngle.z, t);
        Vector3 Lerped = new Vector3(xLerp, yLerp, zLerp);
        return Lerped;
    }


}
