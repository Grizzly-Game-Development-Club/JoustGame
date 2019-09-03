using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    #region Variable
    private bool m_IsAvailable = false;
    #endregion

    #region Getter & Setter
    public bool IsAvailable
    {
        get
        {
            return m_IsAvailable;
        }

        set
        {
            m_IsAvailable = value;
        }
    }
    #endregion

    private void OnEnable()
    {
        EventManager.StartListening(E_EventName.Set_Initial, AddToSpawnManager);
    }
    private void OnDisable()
    {
        EventManager.StopListening(E_EventName.Set_Initial, AddToSpawnManager);
    }

    //Add Spawner Reference to spawn manager
    private void AddToSpawnManager(EventParam obj)
    {
        EventObject eo = new EventObject();
        eo.TypeGameObject = new List<GameObject>() { this.gameObject };

        EventManager.TriggerEvent(E_EventName.Add_Spawner, eo);
        EventManager.TriggerEvent(E_EventName.Spawner_Available, eo);
    }

    //Spawn Enemy
    public void Spawn(GameObject enemyPrefab)
    {
        //Adjust the spawnpoint to be align with the bottom of the box collider
        float enemyBottomEdge = enemyPrefab.GetComponent<BoxCollider2D>().size.y / 2;
        Vector2 adjustedSpawnPoint = new Vector2(transform.position.x, transform.position.y + enemyBottomEdge);

        //Spawn the enemy and store it as a reference
        GameObject enemyReference = Instantiate(enemyPrefab, adjustedSpawnPoint, Quaternion.identity) as GameObject;

        //Trigger event enemy spawn and pass in enemy as reference
        EventObject eo = new EventObject();
        eo.TypeGameObject = new List<GameObject>() {enemyReference};
        EventManager.TriggerEvent(E_EventName.Enemy_Spawned, eo);
    }

    //If an object is blocking the spawnpoint
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Set Current Spawner value to unavaible
        IsAvailable = false;

        //Send out signal that this spawner is unavailable
        EventObject eo = new EventObject();
        eo.TypeGameObject = new List<GameObject>() { this.gameObject };
        EventManager.TriggerEvent(E_EventName.Spawner_Unavailable, eo);
    }

    //If the object blocking the spawnpoint left
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Set Current Spawner value to unavaible
        IsAvailable = true;

        //Send out signal that this spawner is unavailable
        EventObject eo = new EventObject();
        eo.TypeGameObject = new List<GameObject>() { this.gameObject };
        EventManager.TriggerEvent(E_EventName.Spawner_Available, eo);
    }
}
