using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProperties : MonoBehaviour {

    public float HP = 100;
    public float MP = 100;
    public float immediateDamages = 10;
    public float ongoingDamages = 0;
    public float defends = 1;

    public string[] interactTags;

    public float incounterOngoingDamages = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (incounterOngoingDamages > 0)
        {
            HP -= incounterOngoingDamages * Time.deltaTime;
            Debug.Log(tag + " HP is " + HP.ToString());
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    { 
        foreach (string interacttag in interactTags){
            if (interacttag == other.tag)
            {
                CharacterProperties otherProperties = other.GetComponent<CharacterProperties>();
                HP -= CalculateImmediateDamage(this, otherProperties);
                Debug.Log(tag + " HP is " + HP.ToString());
                incounterOngoingDamages += otherProperties.ongoingDamages;
            }
            
        }
    }

    void OnTriggerExit2D(Collider2D other)
    { 
        foreach (string interacttag in interactTags){
            if (interacttag == other.tag)
            {
                CharacterProperties otherProperties = other.GetComponent<CharacterProperties>();
                incounterOngoingDamages -= otherProperties.ongoingDamages;
            }
            
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    { 
        foreach (string interacttag in interactTags){
            if (interacttag == other.gameObject.tag)
            {
                CharacterProperties otherProperties = other.gameObject.GetComponent<CharacterProperties>();
                HP -= CalculateImmediateDamage(this, otherProperties);
                Debug.Log(tag + " HP is " + HP.ToString());
                incounterOngoingDamages += otherProperties.ongoingDamages;
            }
            
        }
    }

    void OnCollisionExit2D(Collision2D other)
    { 
        foreach (string interacttag in interactTags){
            if (interacttag == other.gameObject.tag)
            {
                CharacterProperties otherProperties = other.gameObject.GetComponent<CharacterProperties>();
                incounterOngoingDamages -= otherProperties.ongoingDamages;
            }
            
        }
    }

    float CalculateImmediateDamage(CharacterProperties self, CharacterProperties other)
    {
        float immediateDamages = other.immediateDamages - self.defends;
        return immediateDamages > 0.0f ? immediateDamages : 0.0f;
    }

    float CalculateimmediateDamage(CharacterProperties self, CharacterProperties other)
    {
        float immediateDamages = other.immediateDamages - self.defends;
        return (immediateDamages > 0.0f ? immediateDamages : 0.0f) * Time.deltaTime;
    }
}
