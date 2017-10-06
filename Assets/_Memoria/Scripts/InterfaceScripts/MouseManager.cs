using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic;

public class MouseManager : MonoBehaviour {

    public Ray raycast;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    private void OnEnable()
    {
    
    }
}
