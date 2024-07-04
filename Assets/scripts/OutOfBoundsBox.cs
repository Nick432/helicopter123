using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class OutOfBoundsBox : MonoBehaviour
{
    public Vector2 topRightBoxPoint;
    public Vector2 bottomLeftBoxPoint;

    // Draw gizmo for visualisation.
    void OnDrawGizmosSelected()
    {
        float xCentre = (topRightBoxPoint.x + bottomLeftBoxPoint.x) / 2f;
        float yCentre = (topRightBoxPoint.y + bottomLeftBoxPoint.y) / 2f;
        Vector2 centre = new Vector2(xCentre, yCentre);
        float width = topRightBoxPoint.x - bottomLeftBoxPoint.x;
        float height = topRightBoxPoint.y - bottomLeftBoxPoint.y;
        Vector3 size = new Vector3(width, height, 1f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(centre, size);
    }

}
