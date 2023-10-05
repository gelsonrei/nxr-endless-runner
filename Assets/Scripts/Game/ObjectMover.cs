using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public enum Axis
    {
        X, Y, Z
    }

    public bool autoInit = false;

    [Header("Rotation")]
    public bool doRotation = true;
    public Axis rotationAxis = Axis.Y;
    public float rotationSpeed = 10f;

    [Header("Motion")]
    public bool doMotion = false;
	public float motionMagnitude = 0.001f;
    public Axis motionAxis = Axis.Y;

    void Update()
    {
        if (doRotation && Time.timeScale > 0 && autoInit)
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;

            Vector3 axis;
            switch (rotationAxis)
            {
                case Axis.X:
                    axis = Vector3.right;
                    break;

                case Axis.Y:
                    axis = Vector3.up;
                    break;

                case Axis.Z:
                    axis = Vector3.forward;
                    break;

                default:
                    axis = Vector3.up;
                    break;
            }

            transform.Rotate(axis, rotationAmount);
        }

        if (doMotion && Time.timeScale > 0 && autoInit)
        {
            Vector3 axis;
            switch (motionAxis)
            {
                case Axis.X:
                    axis = Vector3.right;
                    break;

                case Axis.Y:
                    axis = Vector3.up;
                    break;

                case Axis.Z:
                    axis = Vector3.forward;
                    break;

                default:
                    axis = Vector3.up;
                    break;
            }

            transform.Translate (axis * Mathf.Cos(Time.timeSinceLevelLoad) * motionMagnitude );
        }
    }
}