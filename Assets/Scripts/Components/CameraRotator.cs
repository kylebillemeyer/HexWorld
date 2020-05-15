using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexWorld.Components
{
    public class CameraRotator : MonoBehaviour
    {
        private Camera mainCamera;

        // Start is called before the first frame update
        void Start()
        {
            mainCamera = this.GetComponentInChildren<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            var move = new Vector3(0, 0, 0);

            if (Input.GetKey("a"))
            {
                move += new Vector3(-5f * Time.deltaTime, 0, 0);
            }

            if (Input.GetKey("d"))
            {
                move += new Vector3(5f * Time.deltaTime, 0, 0);
            }

            if (Input.GetKey("w"))
            {
                move += new Vector3(0, 0, 5f * Time.deltaTime);
            }

            if (Input.GetKey("s"))
            {
                move += new Vector3(0, 0, -5f * Time.deltaTime);
            }

            this.gameObject.transform.Translate(move, Space.Self);



            if (Input.GetKey("q"))
            {
                this.gameObject.transform.Rotate(0, 180 * Time.deltaTime, 0);
            }

            if (Input.GetKey("e"))
            {
                this.gameObject.transform.Rotate(0, -180 * Time.deltaTime, 0);
            }


            if (Input.GetKey("r"))
            {
                mainCamera.gameObject.transform.Translate(0, 0, 5f * Time.deltaTime);
            }

            if (Input.GetKey("f"))
            {
                mainCamera.gameObject.transform.Translate(0, 0, -5f * Time.deltaTime);
            }
        }
    }
}
