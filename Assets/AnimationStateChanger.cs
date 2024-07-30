using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateChanger : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private string defaultState = "Idle";
    [SerializeField] private string currentState = "";

    public void ChangeState(string newState)
    {
        if (newState == currentState) {
            return;
        }

        animator.Play(newState);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        ChangeState(defaultState);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
