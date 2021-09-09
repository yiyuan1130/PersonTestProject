using UnityEngine;
using System.Collections;

public class MouseCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 targetOffset;
    public float distance = 5.0f;
    public float maxDistance = 20;
    public float minDistance = .6f;
    public float xSpeed = 200.0f;
    public float ySpeed = 200.0f;
    public int yMinLimit = -80;
    public int yMaxLimit = 80;
    public int zoomRate = 40;
    public float panSpeed = 0.3f;
    public float zoomDampening = 5.0f;
    public float autoRotate = 1f;
    public float autoRotateSpeed = 0.1f;

    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    private float currentDistance;
    private float desiredDistance;
    private Quaternion currentRotation;
    private Quaternion desiredRotation;
    private Quaternion rotation;
    private Vector3 position;
    private float idleTimer = 0.0f;
    private float idleSmooth = 0.0f;

    void Start() { Init(); }
    void OnEnable() { Init(); }

    public void Init()
    {
        //If there is no target, create a temporary target at 'distance' from the cameras current viewpoint
        if (!target)
        {
            target = GameObject.Find("CameraCenter").transform;
        }

        distance = Vector3.Distance(transform.position, target.position);
        currentDistance = distance;
        desiredDistance = distance;

        //be sure to grab the current rotations as starting points.
        position = transform.position;
        rotation = transform.rotation;
        currentRotation = transform.rotation;
        desiredRotation = transform.rotation;

        xDeg = Vector3.Angle(Vector3.right, transform.right);
        yDeg = Vector3.Angle(Vector3.up, transform.up);
        position = target.position - (rotation * Vector3.forward * currentDistance + targetOffset);
    }

    /*
     * Camera logic on LateUpdate to only update after all character movement logic has been handled.
     */
    void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            //Clamp the vertical axis for the orbit
            yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
            // set camera rotation
            desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
            currentRotation = transform.rotation;
            rotation = Quaternion.Lerp(currentRotation, desiredRotation, 0.02f * zoomDampening);
            transform.rotation = rotation;

            idleTimer = 0;
            idleSmooth = 0;

        }
        else
        {
            //Smooth idle rotation
           idleTimer+= 0.01f;
            if (idleTimer > autoRotate && autoRotate > 0)
            {
                idleSmooth += (0.02f + idleSmooth) * 0.005f;
                idleSmooth = Mathf.Clamp(idleSmooth, 0, 1);
                xDeg += xSpeed * Time.deltaTime * idleSmooth * autoRotateSpeed;
            }

            // Smooth idle rotation ends

            // Smooth exit
            //Clamp the vertical axis for the orbit
            yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
            // set camera rotation
            desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
            currentRotation = transform.rotation;
            rotation = Quaternion.Lerp(currentRotation, desiredRotation, 0.02f * zoomDampening * 2);
            transform.rotation = rotation;

            // Smooth exit ends

        }

        // affect the desired Zoom distance if we roll the scrollwheel
        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * 0.02f * zoomRate * Mathf.Abs(desiredDistance);
        //clamp the zoom min/max
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        // For smoothing of the zoom, lerp distance
        currentDistance = Mathf.Lerp(currentDistance, desiredDistance, 0.02f * zoomDampening);
        // calculate position based on the new currentDistance
        position = target.position - (rotation * Vector3.forward * currentDistance + targetOffset);
        transform.position = position;
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}