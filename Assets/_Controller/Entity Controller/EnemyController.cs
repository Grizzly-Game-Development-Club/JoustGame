using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region Variable
    [SerializeField] private int m_EnemyScore;
    #endregion

    #region Getter and Setter
    public int EnemyScore
    {
        get
        {
            return m_EnemyScore;
        }

        set
        {
            m_EnemyScore = value;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Test Example Coment
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
