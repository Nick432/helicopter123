using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class WaveGizmos : MonoBehaviour
{
    public float lengthOfWave = 20f;

    void OnDrawGizmos()
    {
        float cameraWidth = Camera.main.orthographicSize * Camera.main.aspect * 2f;
        float minX = transform.position.x - cameraWidth / 2f;
        float maxX = transform.position.x + cameraWidth / 2f;

        float yPosition = transform.position.y;

        Gizmos.color = Color.magenta;

        Gizmos.DrawLine(new Vector3(minX, yPosition, 0f), new Vector3(minX, yPosition - 100f, 0f));
        Gizmos.DrawLine(new Vector3(maxX, yPosition, 0f), new Vector3(maxX, yPosition - 100f, 0f));

        Gizmos.color = Color.red;

        Gizmos.DrawLine(new Vector3(minX, yPosition - lengthOfWave, 0f), 
                        new Vector3(maxX, yPosition - lengthOfWave, 0f));
    }
}
