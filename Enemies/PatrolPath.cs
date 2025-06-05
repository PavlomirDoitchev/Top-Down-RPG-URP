
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] Transform[] waypoints;
        [SerializeField] Color gizmoColor;
        [SerializeField] float gizmoSize = 0.2f;
        private void Start()
        {
            Debug.Assert(waypoints.Length > 0, "PatrolPath must have at least one waypoint assigned.");
        }
        private void OnDrawGizmos()
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i] == null) continue;
                int j = GetNextWaypoint(i);
                Gizmos.color = gizmoColor;
                Gizmos.DrawSphere(GetWaypoint(i), gizmoSize);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        public int GetNextWaypoint(int i)
        {
            return (i + 1) % waypoints.Length;
        }

        public Vector3 GetWaypoint(int i)
        {
            return waypoints[i].position;
        }
        public int GetWaypointCount()
        {
            return waypoints.Length;
        }
    }
}
