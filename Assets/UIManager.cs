using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Creature targetCreature;
    [SerializeField] private Transform hpBarFill;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // gonna do int math and return 1 or 0???????
        float percentHpRemaining = (float)targetCreature.GetHitPoints() / targetCreature.GetMaxHitPoints();
        if (percentHpRemaining < 0) {
            percentHpRemaining = 0;
        }
        
        hpBarFill.localScale = new Vector3(percentHpRemaining, hpBarFill.localScale.y, hpBarFill.localScale.z);
    }
}
