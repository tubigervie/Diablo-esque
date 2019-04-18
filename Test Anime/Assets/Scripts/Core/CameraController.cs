using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] float speedH = 2.0f;
        public float smoothTime = .3f;
        Vector3 velocity = Vector3.zero;
        [SerializeField] GameObject camera;

        float y;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(1))
            {
                Vector3 target = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + (Input.GetAxis("Mouse X") * speedH), transform.eulerAngles.z);
                transform.eulerAngles = Vector3.SmoothDamp(transform.eulerAngles, target, ref velocity, smoothTime);
            }

        }
    }
}

