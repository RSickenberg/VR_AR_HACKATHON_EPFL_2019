using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityDetectorHider : MonoBehaviour
{

    public GameObject target;

    private Canvas objRenderer;
    [SerializeField] private Transform mainCamTransform; // Stores the FPS camera transform

    private readonly float distance_toggle = 3;
    private bool isHidden = false;

    private float updateEvery = 1;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        objRenderer = gameObject.GetComponent<Canvas>(); //Get render reference
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateEvery)
        {
            this.Checker();
            timer = 0;
        }
        
    }

    private void Checker() 
    {
        float distance = Vector3.Distance(mainCamTransform.position, transform.position);
        Debug.Log(distance);

        if (distance < distance_toggle)
        {
            objRenderer.enabled = true;
            isHidden = false;
        } else
        {
            objRenderer.enabled = false;
            isHidden = true;
        }
    }
}
