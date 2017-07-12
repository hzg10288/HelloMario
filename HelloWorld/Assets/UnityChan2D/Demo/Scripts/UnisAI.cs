using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnisAI : MonoBehaviour {

    public float maxSpeed = 10f;
    public Rigidbody2D m_rigidbody2D;
    public float range = 5;
    Vector2 pos;

	// Use this for initialization
	void Start () {;
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_rigidbody2D.velocity = new Vector2(maxSpeed, 0);
        pos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.x < pos.x - range)
        {
            m_rigidbody2D.velocity = new Vector2(maxSpeed, 0);
        } else if (transform.position.x > pos.x + range)
        {
            m_rigidbody2D.velocity = new Vector2(-maxSpeed, 0);
        }
	}

    void FixedUpdate() { 
    }
}
