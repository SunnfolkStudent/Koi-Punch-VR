using UnityEngine;

public class TrajectoryTester : MonoBehaviour
{
    public float punchForce = 10f;
    public Vector3 punchDirection = new Vector3(1, 1, 0).normalized;
    public GameObject projectilePrefab;
    private Vector3 _landingPosAscend;
    private Vector3 _landingPosDescend;

    // Reference to LineRenderer component
    private LineRenderer lineRenderer;
    private float totalTime;

    // Variable to track whether the spacebar has been pressed
    private bool launchRequested = false;

    void Start()
    {
        totalTime = Time.fixedDeltaTime * 10; // Adjust the multiplier as needed
        InitializeLineRenderer();
    }

    private void FixedUpdate()
    {
        // Calculate the trajectory points for rendering
        Vector3 startPoint = CalculateTrajectoryPoint(totalTime / 2);
        Vector3 endPoint = CalculateTrajectoryPoint(totalTime);

        // Update the LineRenderer in real-time
        UpdateLineRenderer(transform.position, startPoint, endPoint);

        // Check if the spacebar has been pressed and launch the projectile
        if (launchRequested)
        {
            CalculatePunchedTrajectory(punchForce, punchDirection);
            LaunchProjectile();
            launchRequested = false; // Reset the flag
        }
    }

    private void CalculatePunchedTrajectory(float punchForce1, Vector3 fishLaunchDir)
    {
        var gravity = 9.81f;
        var timeToTarget = (2 * punchForce1 * fishLaunchDir.normalized.y) / gravity;

        var startPosFish = transform.position;
        _landingPosAscend = startPosFish + punchForce1 * fishLaunchDir.normalized * timeToTarget - 0.5f * gravity *
            Mathf.Pow(timeToTarget, 2) * Vector3.up;

        // Calculate for the descending part
        var timeToTop = punchForce1 * fishLaunchDir.normalized.y / gravity;
        var timeDescend = timeToTarget - timeToTop;
        _landingPosDescend = _landingPosAscend + punchForce1 * fishLaunchDir.normalized * timeDescend + 0.5f * gravity *
            Mathf.Pow(timeDescend, 2) * Vector3.down;
    }

    private Vector3 CalculateTrajectoryPoint(float time)
    {
        float gravity = 9.81f;

        if (time <= totalTime / 2)
        {
            // Ascending phase
            float horizontal = punchForce * punchDirection.normalized.x * time;
            float vertical = punchForce * punchDirection.normalized.y * time - 0.5f * gravity * Mathf.Pow(time, 2);
            return transform.position + new Vector3(horizontal, vertical, 0);
        }
        else
        {
            // Descending phase
            float timeDescend = time - totalTime / 2;
            float horizontal = punchForce * punchDirection.normalized.x * timeDescend;
            float vertical = punchForce * punchDirection.normalized.y * timeDescend + 0.5f * gravity * Mathf.Pow(timeDescend, 2);
            return _landingPosDescend + new Vector3(horizontal, vertical, 0);
        }
    }

    private void InitializeLineRenderer()
    {
        // Create a new GameObject and add LineRenderer component
        GameObject lineObject = new GameObject("TrajectoryLine");
        lineRenderer = lineObject.AddComponent<LineRenderer>();

        // Set LineRenderer properties
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;
        lineRenderer.positionCount = 2; // Set to 2 for start and end points
    }

    private void UpdateLineRenderer(Vector3 initialPoint, Vector3 ascendPoint, Vector3 descendPoint)
    {
        // Update LineRenderer positions
        lineRenderer.SetPosition(0, initialPoint);
        lineRenderer.SetPosition(1, ascendPoint);

        // If the spacebar is pressed, add a new position for the descending point
        if (launchRequested)
        {
            lineRenderer.positionCount = 3; // Set to 3 for start, ascend, and descend points
            lineRenderer.SetPosition(2, descendPoint);
            launchRequested = false; // Reset the flag

            // Log the positions for debugging
            Debug.Log($"Initial: {initialPoint}, Ascend: {ascendPoint}, Descend: {descendPoint}");
        }
    }

    private void LaunchProjectile()
    {
        if (projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

            if (projectileRb != null)
            {
                projectileRb.AddForce(punchForce * punchDirection.normalized, ForceMode.Impulse);
            }
            else
            {
                Debug.LogError("Rigidbody not found on the projectile prefab!");
            }
        }
        else
        {
            Debug.LogError("Projectile prefab not assigned!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            launchRequested = true;
            LaunchProjectile(); // Call LaunchProjectile immediately when spacebar is pressed
        }
    }
    
    /* private void CalculatePunchedTrajectory(float punchForce, Vector3 fishLaunchDir)
    {
        // Use the following variables for calculating trajectory:
        // Landing position = origin + time * velocity a.k.a. _rigidbody.position + timeToTarget * punchForce.
    
        // We likely have to split calculations into 3 parts - 2 vertical calculations (to apex, and from apex to landing)
        // And 1 horizontal calculation (velocity.forward).
        
        // Velocity (v) = punchForceMultiplier,
        // Direction (d) = cubeLaunchDir.normalized,
        // Angle = arcSin(direction.y)
        // Gravity = 9.81 = Newton.
    
        // Time = (2*v*sin (a))/gravity.
    
        // Sin x where is x = arcSin x, a.k.a. "sin (arcSin x)" is just x.
        // Therefore we can just write in fishLaunchDir.normalized.y.
        // t = (2*punchForce*fishLaunchDir.normalized.y)/9.81.
        
        // does this gravity have to match the fish's own gravity? Probably.
        var gravity = 9.81f;
        var timeToTarget = (2 * punchForce * fishLaunchDir.normalized.y) / gravity;
    
        var startPosFish = transform.position;
        var landingPos = startPosFish + punchForce * fishLaunchDir.normalized * timeToTarget - 
                         0.5f * gravity * Mathf.Pow(timeToTarget, 2) * Vector3.up;
    
        if (showDebugLines)
        {
            Debug.DrawLine(startPosFish, landingPos, Color.yellow, timeToTarget);
        }
    } */
    
}