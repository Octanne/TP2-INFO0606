using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlGrue : MonoBehaviour
{
    public float torque = 250f;
    public float forceChariot = 500f;
    public float forceMoufle = 500f;

    // Ajout de la variable pour le contrôle du joueur
    public bool playerInControl = false;

    // Ajout des variables pour les articulations
    public ArticulationBody pivot;
    public ArticulationBody chariot;
    public ArticulationBody moufle;

    // Ajout des variables pour les caméras
    public List<Camera> cameras = new List<Camera>();
    public int camSel = 0;

    // Methode de mise à jour de la caméra
    private void updateCamera() {
        // Désactivation des caméras
        for (int i = 0; i < cameras.Count; i++) {
            cameras[i].enabled = false;
        }

        // Activation de la caméra choisie
        cameras[camSel].enabled = true;
        Debug.Log("[Camera] Camera " + cameras[camSel].name + " [" + camSel + "] enabled");
    }

    // Methode de contrôle de la grue
    private void grueControl() {
        // Commandes pour le pivot (rotation gauche et droite)
        if (Input.GetKey(KeyCode.LeftArrow)) {
            pivot.AddTorque(transform.up * -torque);
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            pivot.AddTorque(transform.up * torque);
        }

        // Commandes pour le chariot (avancer et reculer)
        if (Input.GetKey(KeyCode.UpArrow)) {
            chariot.AddRelativeForce(transform.right * forceChariot);
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            chariot.AddRelativeForce(transform.right * -forceChariot);
        }
        
        // Commandes pour la moufle (monter et descendre)
        if (Input.GetKey(KeyCode.LeftShift)) {
            moufle.AddRelativeForce(transform.up * forceMoufle);
        }
        if (Input.GetKey(KeyCode.LeftControl)) {
            moufle.AddRelativeForce(transform.up * -forceMoufle);
        }

        // Commandes pour quitter le contrôle de la grue
        if (Input.GetKey(KeyCode.E)) {
            playerInControl = false;
            Debug.Log("[Grue] Player leaves control of the grue");
            
        }
    }

    // Methode de changement de caméra
    private void changeCamera() {
        bool camChanged = false;

        // On incrémente la caméra sélectionnée
        if (Input.GetKeyDown(KeyCode.PageUp)) {
            camSel++;
            if (camSel > cameras.Count - 1) {
                camSel = 0;
            }
            camChanged = true;
        }
        // On décrémente la caméra sélectionnée
        if (Input.GetKeyDown(KeyCode.PageDown)) {   
            camSel--;
            if (camSel < 0) {
                camSel = cameras.Count - 1;
            }
            camChanged = true;
        }

        // On met à jour la caméra
        if (camChanged) updateCamera();
    }

    // Start is called before the first frame update
    void Start() {
        if (playerInControl) updateCamera();
        else {
            for (int i = 0; i < cameras.Count; i++) {
                cameras[i].enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (playerInControl) {
            grueControl();
            changeCamera();
        }
    }

    // method to enter Control.
    public void takeControl() {
        playerInControl = true;
        Debug.Log("[Grue] Player take control of the grue.");
        updateCamera();
    }

    // method to leave Control.
    public void leaveControl() {
        playerInControl = false;
        cameras[camSel].enabled = false;
        Debug.Log("[Grue] Player leave control of the grue.");
    }
}
