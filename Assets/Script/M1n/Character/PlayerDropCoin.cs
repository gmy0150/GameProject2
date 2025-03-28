using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDropCoin : MonoBehaviour
{
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindAnyObjectByType<Player>();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
