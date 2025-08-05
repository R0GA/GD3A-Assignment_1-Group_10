using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class Hearing_zone_Script : MonoBehaviour
{

    [Header("SphereCast Settings")]
    public float sphereRadius = 5f;
    private float castDistance = 0.10f;
    Vector3 direction = Vector3.forward;

    public float radius = 3f;
    public int segments = 100;
    private LineRenderer lineRenderer;

    [Header("Debug Colours")]
    public Color startColor = Color.green;
    public Color endColor = Color.yellow;
    public Color hitColor = Color.red;
    public Color rayColor = Color.cyan;

    private RaycastHit hitInfo;
    private bool didHit = false;



    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.positionCount = segments;

        Draw();
    }

    // Update is called once per frame
    void Update()
    {
        for (float i = 0; i < 1; i += 0.1f)
        {
            direction = new Vector3(0, 0, i);

            // Perform SphereCast
            didHit = Physics.SphereCast(transform.position, sphereRadius, direction, out hitInfo, castDistance);

            if (didHit)
            {
                Debug.Log("Touched");
            }

            // Draw debug ray in Scene view (play mode)
            //Debug.DrawRay(transform.position, direction * castDistance, rayColor);
        }
    }

    void Draw()
    {
        Vector3[] points = new Vector3[segments];

        for (int i = 0; i < segments; i++)
        {
            float angle = 2 * Mathf.PI * i / segments;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            points[i] = new Vector3(x, -1, z);
        }

        lineRenderer.SetPositions(points);
    }

    private void OnDrawGizmos()
    {
        Vector3 start = transform.position;
        Vector3 end = start + direction * castDistance;

        // Draw start and end spheres
        Gizmos.color = startColor;
        Gizmos.DrawWireSphere(start, sphereRadius);

        //Gizmos.color = endColor;
        //Gizmos.DrawWireSphere(end, sphereRadius);

        // If hit, draw hit sphere
        if (didHit)
        {
            Gizmos.color = hitColor;
            Gizmos.DrawWireSphere(hitInfo.point, 0.5f);
        }

        // Connect start and end with lines (approximate the cast "tube")
        Gizmos.color = rayColor;
        Gizmos.DrawLine(start, end);
    }
}
