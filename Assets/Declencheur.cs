using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Declencheur : MonoBehaviour
{
    // Compteur de blocs
    private int countBlocks = 0;

    // Récupération du composant Text a mettre à jour
    public Text my_text;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Mise à jour du texte
        my_text.text = "Nombre de blocs : " + countBlocks;
    }

    // Méthode appelée quand un objet entre dans le trigger
    private void OnTriggerEnter(Collider other)
    {
        countBlocks++; // On incrémente le compteur
        Debug.Log("Un bloc est entré,  " + countBlocks + " blocs au total.");
    }

    // Méthode appelée quand un objet sort du trigger
    private void OnTriggerExit(Collider other)
    {
        countBlocks--; // On décrémente le compteur
        Debug.Log("Un bloc est sorti,  " + countBlocks + " blocs restants");
    }
}
