
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deplacement : MonoBehaviour
{

    public Transform TransformCube;
    public float speed = 0;
    private bool Clear = true;
    public int lifePoint = 75;
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        Spe();
    }

    void Spe() {
        if (Input.GetKey(KeyCode.E) && Clear) {
            print(Clear);
            Clear = false;
        }
    }

    void movement()
    {
        // Movement X Y

        
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.Q) == false) {
            TransformCube.Translate(speed,0,0);
        }
        else if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.D) == false) {
            TransformCube.Translate(-speed,0,0);
        }
        if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.S) == false) {
            TransformCube.Translate(0,0,speed);
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Z) == false) {
            TransformCube.Translate(0,0,-speed);
        }
    }

    public void PickupTaken(int HealthAmount)
    {
        lifePoint += HealthAmount;
        Debug.Log("Hp at" + lifePoint);
    }

}
