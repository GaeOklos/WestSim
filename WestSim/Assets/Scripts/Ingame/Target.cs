using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Renderer _renderer;
    private Color _colorInit;
    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnMouseEnter() {
        _colorInit = _renderer.material.color;
        _renderer.material.color = Color.red;
    }

    private void OnMouseExit() {
        _renderer.material.color = _colorInit;
    }
}
