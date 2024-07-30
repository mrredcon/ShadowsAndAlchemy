using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChaser : MonoBehaviour
{
    [SerializeField] private float leewayX = 0.1f;
    [SerializeField] private float speed = 2.0f;
    private bool cameraInMotion = false;
    [SerializeField] private Sprite newBackground;
    [SerializeField] private ScrollingBackground scroller;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraInMotion) {
            float cameraTargetX = transform.position.x;
            
            Camera.main.transform.position = new Vector3(
                Mathf.Lerp(Camera.main.transform.position.x, cameraTargetX, Time.deltaTime * speed),
                Camera.main.transform.position.y,
                Camera.main.transform.position.z
            );

            // Snap into place if we are close enough to our target position
            if ((cameraTargetX > Camera.main.transform.position.x && Camera.main.transform.position.x > (cameraTargetX - leewayX)) || 
                (cameraTargetX < Camera.main.transform.position.x && Camera.main.transform.position.x < (cameraTargetX + leewayX)))
            {
                Camera.main.transform.position = new Vector3(cameraTargetX, Camera.main.transform.position.y, Camera.main.transform.position.z);
                cameraInMotion = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player")) {
            float cameraTargetX = transform.position.x;
            if (Camera.main.transform.position.x == cameraTargetX) {
                cameraInMotion = false;
                return;
            }

            cameraInMotion = true;
            scroller.ChangeSprite(newBackground);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            // Player exited the train car, we should just give up
            cameraInMotion = false;
        }
    }
}
