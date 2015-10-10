using UnityEngine;
using System.Collections;

public class CameraFollower : MonoBehaviour 
{
    public Transform player;
    public float distance;
    public float height;
    public float rotationDamping;
    public float heightDamping;
    public float speed;

    void LateUpdate()
    {
        // Calculate the current rotation angles
        float wantedRotationAngle = player.eulerAngles.y;
        float wantedHeight = player.transform.position.y + height;

        //float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        // Damp the rotation around the y-axis
        //currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
        Quaternion currentRotation = Quaternion.Euler(0, 0, currentHeight);

        // Convert the angle into a rotation
        //Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        Vector3 newPosition = player.position;
        newPosition -= currentRotation * Vector3.forward * distance;

        // Set the height of the camera
        transform.position = new Vector3(newPosition.x, currentHeight, newPosition.z);
    }
}
