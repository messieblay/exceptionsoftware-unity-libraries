﻿using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake() => DontDestroyOnLoad(gameObject);

}
