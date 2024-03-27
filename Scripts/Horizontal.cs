using System;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UIElements;

public class Horizontal : MonoBehaviour
{
    private Rigidbody rb;
    private Quaternion targetRotation;
    public float autoMoveSpeed = 30.0f;

    public int SizeRaycast = 20;
    public int SizeRaycastSide = 10;
    float raycastHeight = 8f; // Ajustez cette valeur en fonction de vos besoins

    private float rotationLerpSpeed = 100f;

    private bool StartMyGame = false;


    private bool StratGame()
    {
        float ValueTrigger = Input.GetAxis("Horizontal");
        if (ValueTrigger > 0.1f || ValueTrigger < 0)
        {
            return true;
        }

        return false;
    }



    // Ajustez ces valeurs selon vos besoins

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetRotation = transform.rotation; // Initialisez la rotation cible avec la rotation actuelle.
        //raycastHeight = (-transform.localScale.y / 2.0f) + 0.1f;
    }

    void FixedUpdate()
    {

        if (StartMyGame == false)
        {
            StartMyGame = StratGame();
        }
        else if (StartMyGame == true)
        {
            // Déplacement automatique vers l'avant
            Vector3 autoMovement = transform.forward * autoMoveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + autoMovement);
            DrawingRaycast();
            Direction();
        }
    }

    void DrawingRaycast()
    {

        Vector3 rayStart = new Vector3(transform.position.x, transform.position.y + raycastHeight, transform.position.z);

        Debug.DrawRay(rayStart, transform.forward * SizeRaycast, Color.green);
        // Raycast de gauche
        Debug.DrawRay(rayStart, Quaternion.Euler(0, 45, 0) * (-transform.right) * SizeRaycastSide, Color.yellow);
        // Raycast de droite
        Debug.DrawRay(rayStart, Quaternion.Euler(0, -45, 0) * transform.right * SizeRaycastSide, Color.yellow);
    }


    void PerformRaycasts(Vector3 rayStart, Vector3 direction, bool[] raycastArray, float angleMultiplier, int sizeRaycast)
    {
        for (int i = raycastArray.Length - 1; i >0; i--)
        {
            RaycastHit hit;

            // Effectuer le raycast
            bool hitSomething = Physics.Raycast(rayStart, Quaternion.Euler(0, angleMultiplier * i, 0) * direction, out hit, sizeRaycast);

            // Dessiner le rayon dans la scène pour le débogage
            Debug.DrawRay(rayStart, Quaternion.Euler(0, angleMultiplier * i, 0) * transform.forward * sizeRaycast, hitSomething ? Color.red : Color.red);

            // Mettre à jour le tableau en fonction du résultat du raycast
            raycastArray[i] = hitSomething && hit.collider.gameObject.name != "Route";
        }
    }


    int CountTrueBooleans(bool[] boolArray)
    {
        int count = 0;

        foreach (bool b in boolArray)
        {
            if (b == true)
            {
                count++;
            }
        }

        return count;
    }

    void RotateLeftSmoothly()
    {
        // Tournez l'objet de 90 degrés vers la gauche (soustrayez 90 degrés)
        float targetAngle = transform.rotation.eulerAngles.y - 90;

        // Créez une rotation cible en fonction de l'angle calculé
        targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
    }

    void RotateRightSmoothly()
    {
        // Tournez l'objet de X degrés vers la gauche
        float targetAngle = transform.rotation.eulerAngles.y + 90;

        // Créez une rotation cible en fonction de l'angle calculé
        targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
    }

    bool Condition_Turn_Right_And_Middle_Ray_Cast_Actif(bool[] LeftRaycastTab, bool[] RightRaycastTab, bool ExtremLeftRayCast, bool ExtremRightRayCast)
    {
        if ((CountTrueBooleans(LeftRaycastTab) > CountTrueBooleans(RightRaycastTab) && ExtremLeftRayCast && !ExtremRightRayCast) ||
                (CountTrueBooleans(LeftRaycastTab) > CountTrueBooleans(RightRaycastTab) && !ExtremLeftRayCast && !ExtremRightRayCast) ||
                (CountTrueBooleans(LeftRaycastTab) < CountTrueBooleans(RightRaycastTab) && ExtremLeftRayCast && !ExtremRightRayCast))
        {
            return true;
        }

        return false;
    }

    bool Condition_Turn_Left_And_Middle_Ray_Cast_Actif(bool[] LeftRaycastTab, bool[] RightRaycastTab, bool ExtremLeftRayCast, bool ExtremRightRayCast)
    {
        if ((CountTrueBooleans(LeftRaycastTab) < CountTrueBooleans(RightRaycastTab) && !ExtremLeftRayCast && ExtremRightRayCast) ||
                (CountTrueBooleans(LeftRaycastTab) < CountTrueBooleans(RightRaycastTab) && !ExtremLeftRayCast && !ExtremRightRayCast) ||
                (CountTrueBooleans(LeftRaycastTab) > CountTrueBooleans(RightRaycastTab) && !ExtremLeftRayCast && ExtremRightRayCast))
        {
            return true;
        }

            return false;
    }

    bool Handle_Rotation_Middle_Ray_Cast_Actif(bool[] LeftRaycastTab, bool[] RightRaycastTab, bool ExtremLeftRayCast, bool ExtremRightRayCast, bool middleRayCast)
    {
        if (middleRayCast && Condition_Turn_Right_And_Middle_Ray_Cast_Actif(LeftRaycastTab, RightRaycastTab, ExtremLeftRayCast, ExtremRightRayCast))
        {
            RotateRightSmoothly();
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationLerpSpeed * Time.deltaTime);
            Debug.Log("Decale droite 1");
            return true;
        }
        else if (middleRayCast && Condition_Turn_Left_And_Middle_Ray_Cast_Actif(LeftRaycastTab, RightRaycastTab, ExtremLeftRayCast, ExtremRightRayCast))
        {
            RotateLeftSmoothly();
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationLerpSpeed * Time.deltaTime);
            Debug.Log("Decale gauche 1");
            return true;
        }
        return false;
    }

    bool HandleExtremeRayCastRotation(bool ExtremLeftRayCast, bool ExtremRightRayCast)
    {
        if (ExtremRightRayCast && !ExtremLeftRayCast)
        {
            RotateLeftSmoothly();
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationLerpSpeed * Time.deltaTime);
            Debug.Log("Decale gauche 2");
            return true;
        }
        else if (!ExtremRightRayCast && ExtremLeftRayCast)
        {
            RotateRightSmoothly();
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationLerpSpeed * Time.deltaTime);
            Debug.Log("Decale droite 2");
            return true;
        }
        return false;
    }

    bool HandleAdditionalRotation(bool[] LeftRaycastTab, bool[] RightRaycastTab, bool ExtremLeftRayCast, bool ExtremRightRayCast, int IndexLastElement)
    {
        if ((ExtremLeftRayCast && CountTrueBooleans(LeftRaycastTab) > 0 && !ExtremRightRayCast && !RightRaycastTab[IndexLastElement]) ||
            (ExtremLeftRayCast && !ExtremRightRayCast && !RightRaycastTab[IndexLastElement] && CountTrueBooleans(LeftRaycastTab) <= 0) ||
            (!ExtremLeftRayCast && !ExtremRightRayCast && !RightRaycastTab[IndexLastElement] && CountTrueBooleans(LeftRaycastTab) > 0))
        {
            RotateRightSmoothly();
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationLerpSpeed * Time.deltaTime);
            Debug.Log("Decale droite 3");
            return true;
        }
        else if ((ExtremRightRayCast && CountTrueBooleans(RightRaycastTab) > 0 && !ExtremLeftRayCast && !LeftRaycastTab[IndexLastElement]) ||
            (ExtremRightRayCast && !ExtremLeftRayCast && !LeftRaycastTab[IndexLastElement] && CountTrueBooleans(RightRaycastTab) <= 0) ||
            (!ExtremLeftRayCast && !ExtremRightRayCast && !LeftRaycastTab[IndexLastElement] && CountTrueBooleans(RightRaycastTab) > 0))
        {
            RotateLeftSmoothly();
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationLerpSpeed * Time.deltaTime);
            Debug.Log("Decale à gauche 3");
            return true;
        }
        return false;
    }

    void Handle_Horizontal_Mouvements(bool[] LeftRaycastTab, bool[] RightRaycastTab, bool ExtremLeftRayCast, bool ExtremRightRayCast, bool middleRayCast, int IndexLastElement)
    {
        bool rotationHandled = false;

        // Si le rayon du milieu est actif
        rotationHandled = Handle_Rotation_Middle_Ray_Cast_Actif(LeftRaycastTab, RightRaycastTab, ExtremLeftRayCast, ExtremRightRayCast, middleRayCast);

        // Si le rayon du milieu n'a pas été actif et les rayons de face extrême gauche et droite sont actifs en même temps
        if (!rotationHandled)
        {
            rotationHandled = HandleExtremeRayCastRotation(ExtremLeftRayCast, ExtremRightRayCast);
        }

        // Si le rayon du milieu et les rayons de face extrême gauche et droite ne sont pas actifs en même temps
        if (!rotationHandled)
        {
            HandleAdditionalRotation(LeftRaycastTab, RightRaycastTab, ExtremLeftRayCast, ExtremRightRayCast, IndexLastElement);
        }
    }


    void Direction()
    {
        RaycastHit hit;

        Vector3 rayStart = new Vector3(transform.position.x, transform.position.y + raycastHeight, transform.position.z);
        bool[] LeftRaycastTab = new bool[28];
        bool[] RightRaycastTab = new bool[28];
        int IndexLastElement = LeftRaycastTab.Length - 1;


        //Creation du Raycast du milieu
        bool MiddleRayCast = Physics.Raycast(rayStart, transform.forward, out hit, SizeRaycast);
        //Creaction du Raycast de cote gauche et cote droit
        bool ExtremRightRayCast = Physics.Raycast(rayStart, Quaternion.Euler(0, -45, 0) * (transform.right), out hit, SizeRaycastSide) && hit.collider.gameObject.name != "Route";

        bool ExtremLeftRayCast = Physics.Raycast(rayStart, Quaternion.Euler(0, 45, 0) * (-transform.right), out hit, SizeRaycastSide) && hit.collider.gameObject.name != "Route";

        //Creation de RayCast Gauche et Droite
        PerformRaycasts(rayStart, transform.forward, RightRaycastTab, 0.9f, SizeRaycast);
        PerformRaycasts(rayStart, transform.forward, LeftRaycastTab, -0.9f, SizeRaycast);
        //Gestionnaire de mouvement horizontal
        Handle_Horizontal_Mouvements(LeftRaycastTab, RightRaycastTab, ExtremLeftRayCast, ExtremRightRayCast, MiddleRayCast, IndexLastElement);
    }
}
