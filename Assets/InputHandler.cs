using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Creature creature;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Bullet time
        // if (Time.timeScale == 1.0f && Input.GetMouseButton(0)) {
        //     Time.timeScale = 0.2f;
        // } else {
        //     Time.timeScale = 1.0f;
        // }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("MainMenu");
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            creature.Jump();
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            creature.ReleaseJump();
        }

        if (Input.GetKeyDown(KeyCode.Delete)) {
            creature.Kill();
        }

        if (Input.GetMouseButtonDown(0)) {
            creature.Attack();
        }

        if (Input.GetKey(KeyCode.S)) {
            creature.Crouch();
        }
        
        if (Input.GetKeyUp(KeyCode.S)) {
            creature.ReleaseCrouch();
        }

        Vector3 mousePointer = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        creature.AimWeaponAtGradually(mousePointer);
    }

    // FixedUpdate is called once per each frame containing a PHYSICS UPDATE
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A)) {
            creature.MoveLeft();
        } else if (Input.GetKey(KeyCode.D)) {
            creature.MoveRight();
        }
    }
}
