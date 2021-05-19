using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bombdroping : MonoBehaviour
{
    [SerializeField] private GameObject BombPrefab;
    [SerializeField] private Transform projectileOrigin;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bombClone = Instantiate(BombPrefab, projectileOrigin);
            Debug.Log("Space");
           // bombClone.getComponent<MeshRenderer>().enable = false;
            //Destroy()
        }
    }
}
