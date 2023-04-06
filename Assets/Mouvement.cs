using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouvement : MonoBehaviour
{   
    public bool playerInControl = true; // Variable pour le contrôle du joueur
    private bool isGrounded = true; // Variable pour le saut
    public float sprintSpeed = 2f; // Variable pour la vitesse de sprint

    // Ajout des variables pour les caméras
    public Camera camFPS;
    public Camera camTPS;
    private Camera activeCam;

    private GameObject objectInRange = null; // Variable pour l'objet contrôlable le plus proche

    void OnCollisionEnter(Collision collision) {
        // L'on autorise le saut uniquement si le joueur touche de nouveau un objet
        isGrounded = true;

        // Ajout de l'objet contrôlable
        if (collision.gameObject.tag == "Controlable") {
            objectInRange = collision.gameObject;
            Debug.Log("[Player] Object in range : " + objectInRange.name);
        }
    }

    void OnCollisionExit(Collision collision) {
        // Suppression de l'objet contrôlable
        if (collision.gameObject.tag == "Controlable") {
            Debug.Log("[Player] Object out of range");
            objectInRange = null;
        }
    }

    // Start is called before the first frame update
    void Start() {
        // Désactivation des caméras
        camFPS.enabled = false;
        camTPS.enabled = false;
        activeCam = camFPS;

        // Activation de la caméra Active
        activeCam.enabled = true;
        Debug.Log("[Camera] Camera " + activeCam.name + " enabled");
    }

    // Update is called once per frame
    void Update() {
        // Si je joueur n'est pas au contrôle d'un objet
        if (playerInControl) playerControlMovement();
        
        // Rentrer et sortir du contrôle d'un objet
        if (Input.GetKeyUp(KeyCode.E)) {
            // Gestion de l'entrée dans le contrôle d'un objet si il y en a un dans la zone
            if (playerInControl && objectInRange != null) {
                Debug.Log("[Player] Enter in control of " + objectInRange.name);
                activeCam.enabled = false;
                playerInControl = false;
                objectInRange.SendMessage("takeControl");
            } else if (!playerInControl) {
                // Gestion de la sortie du contrôle d'un objet
                Debug.Log("[Player] Player is now in control of himself");
                playerInControl = true;
                activeCam.enabled = true;
                objectInRange.SendMessage("leaveControl");
            }
        }
    }

    private void controlCamera() {
        // Rotation Haut et bas de la caméra
        float verticalSpeed = 2.0f;
        // Get the mouse delta. This is not in the range -1...1
        float v = verticalSpeed * Input.GetAxis("Mouse Y");

        // Si rotate de activeCam avec -v ne dépasse pas 90° en bas et 60° en haut on peut tourner la caméra
        if (activeCam.transform.rotation.eulerAngles.x - v < 90 || activeCam.transform.rotation.eulerAngles.x - v > 300) {
            activeCam.transform.Rotate(-v, 0, 0);
        }
        

        // Gauche et droite (On ne veut pas tourner la caméra sur elle même donc on tourne le joueur)
        float horizontalSpeed = 2.0f;
        float h = horizontalSpeed * Input.GetAxis("Mouse X");
        transform.Rotate(0, h, 0);

        // Changement de camera
        if (Input.GetKeyDown(KeyCode.F5))
        {
            // On vérifie si le joueur est en mode FPS
            if (camFPS.enabled)
            {
                // On désactive la caméra FPS
                camFPS.enabled = false;
                // On active la caméra TPS
                camTPS.enabled = true;
                activeCam = camTPS;
            }
            else
            {
                // On désactive la caméra TPS
                camTPS.enabled = false;
                // On active la caméra FPS
                camFPS.enabled = true;
                activeCam = camFPS;
            }
        }
    }

    // Methode de contrôle du joueur
    private void playerControlMovement() {
        // Déplacement vers l'avant
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Z))
        {
            // On vérifie si l'on appuie sur la touche shift pour déplacer le joueur plus vite
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                // Application du déplacement avec une vitesse plus rapide.
                transform.Translate(Vector3.back * 0.01f * sprintSpeed);
            }
            else
            {
                transform.Translate(Vector3.back * 0.01f);
            }
        }

        // Déplacement vers l'arrière
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.forward * 0.005f);
        }

        // Déplacement vers la gauche
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Q))
        {
            transform.Translate(Vector3.right * 0.005f);
        }

        // Déplacement vers la droite
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.left * 0.005f);
        }

        // Saut
        if (Input.GetKey(KeyCode.Space))
        {
            // Application du saut uniquement si le joueur touche le sol
            if (isGrounded)  {
                // L'on applique le déplacement vertical
                GetComponent<Rigidbody>().velocity += new Vector3(0, 10f/2f, 0);

                // L'on désactive le saut jusqu'à la prochaine collision avec le sol
                isGrounded = false;
            }
        }

        // Contrôle de la caméra
        controlCamera();
    }
}
