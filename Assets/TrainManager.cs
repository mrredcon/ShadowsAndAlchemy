using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private ContactFilter2D filter;
    private List<Collider2D> matches;
    private bool doorsOpen = false;
    [SerializeField] private GameObject[] doors;
    private int enemyCount = 0;
    private bool playerIsHere = false;
    private bool playerHasVisitedCar = false;
    [SerializeField] private Creature playerCreature;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        filter = new ContactFilter2D();
        matches = new List<Collider2D>();
        OpenDoors();
    }

    public int GetEnemyCount()
    {
        int enemies = 0;

        boxCollider.OverlapCollider(filter.NoFilter(), matches);
        foreach (Collider2D collider in matches)
        {
            if (collider.gameObject.CompareTag("Brigand"))
            {
                Creature creature = collider.gameObject.GetComponent<Creature>();
                if (creature.IsAlive())
                {
                    enemies++;
                }
            }
        }

        matches.Clear();
        return enemies;
    }

    private void DetectCreatures()
    {
        enemyCount = 0;
        playerIsHere = false;
        boxCollider.OverlapCollider(filter.NoFilter(), matches);

        foreach (Collider2D collider in matches)
        {
            if (collider.gameObject.CompareTag("Player")) {
                playerIsHere = true;

                // First time player has visited the train car, give a full heal
                if (!playerHasVisitedCar)
                {
                    playerCreature.FullyHeal();
                }
                playerHasVisitedCar = true;
            }

            if (collider.gameObject.CompareTag("Brigand"))
            {
                Creature creature = collider.gameObject.GetComponent<Creature>();
                if (creature.IsAlive())
                {
                    enemyCount++;
                }
            }
        }

        matches.Clear();
    }

    void FixedUpdate()
    {
        DetectCreatures();

        if (enemyCount > 0 && doorsOpen && playerIsHere) {
            CloseDoors();
        } else if (enemyCount == 0 && !doorsOpen) {
            OpenDoors();
        }
    }

    private void OpenDoors()
    {
        doorsOpen = true;
        foreach (GameObject door in doors)
        {
            door.SetActive(false);
        }
    }

    private void CloseDoors()
    {
        doorsOpen = false;
        foreach (GameObject door in doors)
        {
            door.SetActive(true);
        }
    }
}
