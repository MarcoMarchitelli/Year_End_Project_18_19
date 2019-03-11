using UnityEngine;

public class PathDrawer : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        if (transform.childCount > 0)
        {
            Gizmos.color = Color.cyan;
            Vector3 startPos = transform.GetChild(0).position;
            Vector3 lastPos = startPos;

            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(transform.GetChild(i).position, .3f);
                Gizmos.DrawLine(lastPos, transform.GetChild(i).position);
                lastPos = transform.GetChild(i).position;
            }

            Gizmos.DrawLine(lastPos, startPos);
        }
    }
}