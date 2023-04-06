using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        // If space is pressed, destroy the fixed joint
        if (Input.GetKey(KeyCode.Space))
        {
            // Suppression du fixed joint
            Destroy(this.gameObject.GetComponent<FixedJoint>());
        }
    }

    // When collide with something
    void OnCollisionEnter(Collision Collision)
    {
        // Si l'objet est un articulation body, on crée un fixed joint
        if (Collision.gameObject.GetComponent<ArticulationBody>() != null)
        {
            // Création du fixed joint
            FixedJoint joint = this.gameObject.AddComponent<FixedJoint>();
            joint.connectedArticulationBody = Collision.articulationBody;
        }
    }
}
