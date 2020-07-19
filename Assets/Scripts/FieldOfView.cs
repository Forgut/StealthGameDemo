using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class FieldOfView : MonoBehaviour
    {
        public Transform Player;
        public float MaxAngle;
        public float MaxRadious;

        private bool SpotsPlayer = false;


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, MaxRadious);

            Vector3 fovLine1 = Quaternion.AngleAxis(MaxAngle, transform.up) * transform.forward * MaxRadious;
            Vector3 fovLine2 = Quaternion.AngleAxis(-MaxAngle, transform.up) * transform.forward * MaxRadious;

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, fovLine1);
            Gizmos.DrawRay(transform.position, fovLine2);

            if (SpotsPlayer)
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.green;
            
            Gizmos.DrawRay(transform.position, (Player.position - transform.position).normalized * MaxRadious);

            Gizmos.color = Color.black;
            Gizmos.DrawRay(transform.position, transform.forward * MaxRadious);
        }

        void Update()
        {
            SpotsPlayer = InFOV(transform, Player, MaxAngle, MaxRadious);
        }

        public static bool InFOV(Transform checkingObject, Transform target, float maxAngle, float maxRadious)
        {
            Collider[] overlaps = new Collider[10];
            int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadious, overlaps);


            foreach (var overlap in overlaps)
            {
                if (overlap != null && overlap.transform == target)
                {
                    Vector3 directionBetweenTarget = (target.position - checkingObject.position).normalized;
                    directionBetweenTarget.y = 0;
                    float angle = Vector3.Angle(checkingObject.forward, directionBetweenTarget);
                    if (angle <= maxAngle)
                    {
                        Ray ray = new Ray(checkingObject.position, target.position - checkingObject.position);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, maxRadious))
                        {
                            if (hit.transform == target)
                            {
                                return true; 
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
