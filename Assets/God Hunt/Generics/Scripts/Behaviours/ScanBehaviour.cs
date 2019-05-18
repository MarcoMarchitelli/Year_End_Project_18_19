using UnityEngine;

public class ScanBehaviour : BaseBehaviour
{
    #region Inspector Variables
    public enum ScanType { fieldOfView, circularArea };

    [SerializeField] ScanType scanType;
    [SerializeField] bool canSeeThroughObstacles;
    [Tooltip("If in Fov scan mode, use a spotlight to visualize scan.")]
    [SerializeField] bool previewWithSpotlight;

    [SerializeField] MonoBehaviour scanTarget;
    [SerializeField] float timeToScan = 0f;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float scanAreaLenght = 5f;

    [SerializeField] float fovAngle = 45;
    [SerializeField] Color spotLightColor, detectedSpotlightColor;

    [SerializeField] float scanAreaRadius = 5f;

    [SerializeField] UnityTransformEvent OnTargetSpotted, OnTargetLost;
    #endregion

    #region Private Variables
    Transform target;
    Light spotLight;
    SphereCollider sphereCollider;
    float spotlightRangeDifference = 3f;
    float targetVisibleTimer;
    bool hasSpottedTargetOnce = false;
    #endregion

    protected override void CustomSetup()
    {
        timeToScan = Mathf.Abs(timeToScan);
        if (timeToScan == 0) timeToScan = 0.001f;

        if (scanType == ScanType.fieldOfView)
        {
            sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
            sphereCollider.radius = scanAreaLenght;

            if (previewWithSpotlight)
            {
                spotLight = gameObject.AddComponent<Light>();
                spotLight.type = LightType.Spot;
                spotLight.range = scanAreaLenght + spotlightRangeDifference;
                spotLight.spotAngle = fovAngle;
                spotLight.color = spotLightColor;
                spotLight.intensity = 50f;
                spotLight.shadows = LightShadows.Hard;
                spotLight.shadowBias = 0f;
            }

        }
        else if (scanType == ScanType.circularArea)
        {
            sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
            sphereCollider.radius = scanAreaRadius;
        }
    }

    #region MonoBehaviour methods
    public override void OnUpdate()
    {
        if (CanSeeTarget())
        {
            targetVisibleTimer += Time.deltaTime;
        }
        else
        {
            targetVisibleTimer -= Time.deltaTime;
        }

        targetVisibleTimer = Mathf.Clamp(targetVisibleTimer, 0, timeToScan);

        if (spotLight)
            spotLight.color = Color.Lerp(spotLightColor, detectedSpotlightColor, targetVisibleTimer / timeToScan);

        if (targetVisibleTimer >= timeToScan && !hasSpottedTargetOnce)
        {
            OnTargetSpotted.Invoke(target);
            hasSpottedTargetOnce = true;
        }
        if (targetVisibleTimer < timeToScan && hasSpottedTargetOnce)
        {
            OnTargetLost.Invoke(target);
            hasSpottedTargetOnce = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (scanType == ScanType.fieldOfView)
            Gizmos.DrawRay(transform.position, transform.forward * scanAreaLenght);
        else
            Gizmos.DrawWireSphere(transform.position, scanAreaRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent(scanTarget.GetType()))
        {
            target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == target)
        {
            target = null;
        }
    }

    #endregion

    bool CanSeeTarget()
    {
        if (!IsSetupped)
        {
            return false;
        }

        if (!target)
        {
            return false;
        }
        if (scanType == ScanType.fieldOfView)
        {
            if (Vector3.Distance(transform.position, target.position) < scanAreaLenght)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                float angleBetweenScannerAndTarget = Vector3.Angle(transform.forward, dirToTarget);
                if (angleBetweenScannerAndTarget < fovAngle / 2f)
                {
                    if (canSeeThroughObstacles)
                        return true;
                    if (!Physics.Linecast(transform.position, target.position, obstacleLayer))
                    {
                        return true;
                    }
                }
            }
        }
        else
        if (scanType == ScanType.circularArea)
        {
            if (Vector3.Distance(transform.position, target.position) < scanAreaRadius)
            {
                if (canSeeThroughObstacles)
                    return true;
                if (!Physics.Linecast(transform.position, target.position, obstacleLayer))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
