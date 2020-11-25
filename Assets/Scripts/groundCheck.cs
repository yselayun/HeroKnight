using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheck : MonoBehaviour
{
    // Start is called before the first frame update
    public Player player;
    void Start()
    {
        player = gameObject.GetComponentInParent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     void FixedUpdate()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        player._falled = false;
        player._grounded = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        player._falled = false;
        player._grounded = true;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        player._grounded = false;
    }
}
