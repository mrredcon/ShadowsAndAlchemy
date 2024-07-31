using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private GameObject backgroundPrefab;
    private GameObject leftBackground;
    private GameObject rightBackground;
    private float sizeX;
    [SerializeField] private float speed;
    [SerializeField] private float yLevel;
    private bool stop = false;

    public void Stop()
    {
        stop = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        sizeX = backgroundPrefab.GetComponent<BoxCollider2D>().size.x;

        leftBackground = Instantiate(backgroundPrefab);
        leftBackground.transform.position = new Vector2(0, yLevel);

        rightBackground = Instantiate(backgroundPrefab);
        rightBackground.transform.position = new Vector2(sizeX, yLevel);
    }

    // Update is called once per frame
    void Update()
    {
        if (stop)
        {
            return;
        }

        leftBackground.transform.position = new Vector2(leftBackground.transform.position.x + speed * Time.deltaTime, leftBackground.transform.position.y);
        rightBackground.transform.position = new Vector2(rightBackground.transform.position.x + speed * Time.deltaTime, rightBackground.transform.position.y);

        Camera camera = Camera.main;

        float camHeight = 2f * camera.orthographicSize;
        float camWidth = camHeight * camera.aspect;
        float cameraEdge = Camera.main.transform.position.x - (camWidth / 2);

        float rightBackgroundEdge = rightBackground.transform.position.x - (sizeX / 2);
        
        if (rightBackgroundEdge < cameraEdge) {
            // Swap the backgrounds around
            leftBackground.transform.position = new Vector2(rightBackground.transform.position.x + sizeX, leftBackground.transform.position.y);
            
            GameObject temp = leftBackground;
        	leftBackground = rightBackground;
        	rightBackground = temp;
        }
    }

    public void ChangeSprite(Sprite newSprite)
    {
        leftBackground.GetComponent<SpriteRenderer>().sprite = newSprite;
        rightBackground.GetComponent<SpriteRenderer>().sprite = newSprite;
    }
}
