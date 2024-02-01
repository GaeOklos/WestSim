using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hello_World : MonoBehaviour
{

    public int scorePlayer = 1000;
    public float healthPoint = 37.5f;
    public string message = "Coucou Gamesup";

    public Transform TransformCube;
    public float speed = 0.1f;




    void Start()
    {
        print("Coucou gamesup, super le print");
        print(scorePlayer);
        // string letter = String.Copy(str);
        // int i = str.Length;

    }

    void Update()
    {
        // TransformCube.Translate(0,0,speed);
        // TransformCube.Rotate(0,0,speed,Space.World);
        // CubeTransform.Rotate(Vector3.up * 0.1f, Space.World);
    }



    /*
    string strcpy(string str)
    {
        if (str != null) {
            }
        else {
            return(str);
        }

        return(letter);

    }
    */
}