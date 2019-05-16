﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDelay : MonoBehaviour
{
    public float delay = 1;
    void Start()
    {
        Destroy(gameObject, delay);
    }
}
