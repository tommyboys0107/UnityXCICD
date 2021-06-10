using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CliffLeeCL
{
    /// <summary>
    /// Define where the object should be spawn.
    /// </summary>
    public class SpawnArea : MonoBehaviour
    {
        /// <summary>
        /// Used to identify specific spawn area.
        /// </summary>
        public Color areaColor = Color.white;

        /// <summary>
        /// Used to define the size of the spawn area.
        /// </summary>
        public Vector3 areaSize = Vector3.one;

        /// <summary>
        /// Used to judge whether the spawn point overlay with other colliders or not approximately.
        /// </summary>
        public float overlaySphereRadius = 0.5f;

        /// <summary>
        /// Compute a random spawn point and return.
        /// </summary>
        /// <param name="canPreventOverlay">Is true when the spawn point should prevent the overlay problem.</param>
        /// <returns>A random spawn point in the spawn area. Return Vector3.zero if there are no valid spawn points.</returns>
        public Vector3 GetSpawnPoint(bool canPreventOverlay)
        {
            Vector3 halfAreaSize = areaSize / 2.0f;
            Vector3 localSpawnPoint = new Vector3(Random.Range(-halfAreaSize.x, halfAreaSize.x),
                Random.Range(-halfAreaSize.y, halfAreaSize.y), Random.Range(-halfAreaSize.z, halfAreaSize.z));
            Vector3 worldSpawnPoint = transform.TransformPoint(localSpawnPoint);

            if (canPreventOverlay)
            {
                int i = 0;

                for(i = 0; i < 10 && IsOverlaidWithObject(worldSpawnPoint); i++)
                {
                    localSpawnPoint = new Vector3(Random.Range(-halfAreaSize.x, halfAreaSize.x),
                        Random.Range(-halfAreaSize.y, halfAreaSize.y), Random.Range(-halfAreaSize.z, halfAreaSize.z));
                    worldSpawnPoint = transform.TransformPoint(localSpawnPoint);
                }

                if (i == 10)
                    return Vector3.zero; // means no valid spawn points.
            }

            return worldSpawnPoint;
        }

        /// <summary>
        /// Check whether a point is overlaid with other objects or not by sphere.
        /// </summary>
        /// <param name="worldSpawnPoint">A point to be checked.</param>
        /// <returns>Is true when the point is overlaid with other objects.</returns>
        bool IsOverlaidWithObject(Vector3 worldSpawnPoint)
        {
            return Physics.CheckSphere(new Vector3(worldSpawnPoint.x, worldSpawnPoint.y + overlaySphereRadius, worldSpawnPoint.z), overlaySphereRadius);
        }

        /// <summary>
        /// Gizmos are drawn only when the object is selected. Gizmos are not pickable. 
        /// This is used to ease setup. 
        /// For example an explosion script could draw a sphere showing the explosion radius.
        /// </summary>
        void OnDrawGizmosSelected()
        {
            Gizmos.color = areaColor;
            Gizmos.DrawWireCube(transform.position, areaSize);
            Gizmos.DrawWireSphere(transform.position + new Vector3(0.0f, overlaySphereRadius, 0.0f), overlaySphereRadius);
            Gizmos.color = new Color(areaColor.r, areaColor.g, areaColor.b, 0.3f);
            Gizmos.DrawCube(transform.position, areaSize);
        }
    }
}

