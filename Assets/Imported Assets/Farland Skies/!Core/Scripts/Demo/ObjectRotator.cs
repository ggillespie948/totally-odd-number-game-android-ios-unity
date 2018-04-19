using UnityEngine;

namespace Borodar.FarlandSkies.Core.Demo
{
    public class ObjectRotator : MonoBehaviour
    {
        [SerializeField]
        protected Vector3 EulerAngles;

        //---------------------------------------------------------------------
        // Messages
        //---------------------------------------------------------------------

        protected void Update()
        {
            transform.Rotate(EulerAngles * Time.deltaTime);
        }
    }
}
