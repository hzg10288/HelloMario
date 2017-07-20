using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterProperties : MonoBehaviour {

    public float HP = 100;
    public float MP = 100;
    public float immediateDamages = 10;
    public float ongoingDamages = 0;
    public float defends = 1;

    public string[] interactTags;

    public float incounterOngoingDamages = 0;

    [SceneName]
    public string deathScene;

    [SerializeField]
    private Slider m_slider;

    private Collider2D m_collider;
    private Rigidbody2D m_rigidbody;

    private bool isDeath = false;

	// Use this for initialization
	void Start () {
        m_collider = GetComponent<Collider2D>();
        m_rigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (HP <= 0)
        {
            if (!isDeath)
            {
                m_collider.enabled = false;
                m_rigidbody.velocity = new Vector2(0, 10.0f);
            }
            if (transform.position.y < -10)
                Application.LoadLevel(deathScene);
            isDeath = true;
        }
	    if (incounterOngoingDamages > 0)
        {
            HP -= incounterOngoingDamages * Time.deltaTime;
        }
        if (m_slider)
            m_slider.value = HP;
	}

    void OnTriggerEnter2D(Collider2D other)
    { 
        foreach (string interacttag in interactTags){
            if (interacttag == other.tag)
            {
                CharacterProperties otherProperties = other.GetComponent<CharacterProperties>();
                if (otherProperties != null)
                {
                    HP -= CalculateImmediateDamage(this, otherProperties);
                    incounterOngoingDamages += otherProperties.ongoingDamages;
                }
            }
            
        }
    }

    void OnTriggerExit2D(Collider2D other)
    { 
        foreach (string interacttag in interactTags){
            if (interacttag == other.tag)
            {
                CharacterProperties otherProperties = other.GetComponent<CharacterProperties>();
                if (otherProperties != null)
                {
                    incounterOngoingDamages -= otherProperties.ongoingDamages;
                }
            }
            
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    { 
        foreach (string interacttag in interactTags){
            if (interacttag == other.gameObject.tag)
            {
                CharacterProperties otherProperties = other.gameObject.GetComponent<CharacterProperties>();
                if (otherProperties != null)
                {
                    HP -= CalculateImmediateDamage(this, otherProperties);
                    Debug.Log(tag + " HP is " + HP.ToString());
                    incounterOngoingDamages += otherProperties.ongoingDamages;
                }
            }
            
        }
    }

    void OnCollisionExit2D(Collision2D other)
    { 
        foreach (string interacttag in interactTags){
            if (interacttag == other.gameObject.tag)
            {
                CharacterProperties otherProperties = other.gameObject.GetComponent<CharacterProperties>();
                if (otherProperties != null)
                {
                    incounterOngoingDamages -= otherProperties.ongoingDamages;
                }
            }
            
        }
    }

    float CalculateImmediateDamage(CharacterProperties self, CharacterProperties other)
    {
        float immediateDamages = other.immediateDamages - self.defends;
        return immediateDamages > 0.0f ? immediateDamages : 0.0f;
    }
}
