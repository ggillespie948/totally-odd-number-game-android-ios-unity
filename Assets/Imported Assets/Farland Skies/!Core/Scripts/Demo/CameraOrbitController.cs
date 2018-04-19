using UnityEngine;
using UnityEngine.EventSystems;

namespace Borodar.FarlandSkies.Core.Demo
{
    public class CameraOrbitController : MonoBehaviour
    {
        public Transform Target;

        public float Distance = 5.0f;
        public float DistanceMin = .5f;
        public float DistanceMax = 15f;

        public Vector3 Speed = new Vector3(250f, 250f, 250f);
        public Vector2 VerticalRotationLimit = new Vector2(-90, 90);

        private Vector2 _angles;

        private bool _isPointerOverGui;

        //---------------------------------------------------------------------
        // Messages
        //---------------------------------------------------------------------

        protected void Awake()
        {
            var eulerAngles = transform.eulerAngles;
            _angles.x = eulerAngles.y;
            _angles.y = eulerAngles.x;
            
            var rotation = Quaternion.Euler(_angles.y, _angles.x, 0);
            var negDistance = new Vector3(0.0f, 0.0f, -Distance);
            var position = rotation * negDistance + Target.position;

            transform.rotation = rotation;
            transform.position = position;
        }

        protected void LateUpdate()
        {
            // Check if interacting with GUI
            if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject()) _isPointerOverGui = true;
            if (Input.GetMouseButtonUp(0)) _isPointerOverGui = false;
            if (_isPointerOverGui) return;

            // Handle mouse input
            var scrollValue = Input.GetAxis("Mouse ScrollWheel");
            if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1) && (Mathf.Abs(scrollValue) < 0.01f)) return;

            var mouseX = Input.GetAxis("Mouse X");
            var mouseY = Input.GetAxis("Mouse Y");

            _angles.x += mouseX * Speed.x * Time.deltaTime;
            _angles.y -= mouseY * Speed.y * Time.deltaTime;

            _angles.y = ClampAngle(_angles.y, VerticalRotationLimit.x, VerticalRotationLimit.y);

            var rotation = Quaternion.Euler(_angles.y, _angles.x, 0);
            Distance = Mathf.Clamp(Distance - scrollValue * Speed.z, DistanceMin, DistanceMax);

            var negDistance = new Vector3(0.0f, 0.0f, -Distance);
            var position = rotation * negDistance + Target.position;

            transform.rotation = rotation;
            transform.position = position;
        }

        //---------------------------------------------------------------------
        // Helpers
        //---------------------------------------------------------------------

        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F) angle += 360F;
            if (angle > 360F) angle -= 360F;
            return Mathf.Clamp(angle, min, max);
        }
    }
}
