using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;
    private float stopwatch;

    // Start is called before the first frame update
    void Start()
    {
        //set the cursor origin to its centre. (default is upper left corner)
        Vector2 cursorOffset = new Vector2(cursorTexture.width/2, cursorTexture.height/2);
     
        //Sets the cursor to the Crosshair sprite with given offset 
        //and automatic switching to hardware default if necessary
        Cursor.SetCursor(cursorTexture, cursorOffset, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        stopwatch += Time.deltaTime;
    }

    public float GetStopwatch()
    {
        return stopwatch;
    }
}
