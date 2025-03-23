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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            player.MakeNoise(gameObject, player.CoinNoise, 3);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
