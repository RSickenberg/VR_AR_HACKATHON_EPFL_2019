// HandGabbing.cs
// Romain Sickenberg <haux49@gmail.com> 2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR; //needs to be UnityEngine.VR in version before 2017.2

public class HandGrabbing : MonoBehaviour
{

    public string InputName;
    public XRNode NodeType;
    public Vector3 ObjectGrabOffset;
    public float GrabDistance = 0.1f;
    public string GrabTag = "Grab";
    public float ThrowMultiplier = 1.5f;

    private Transform _currentObject;
    private Vector3 _lastFramePosition;

    // Start is called before the first frame update
    void Start()
    {
        _currentObject = null;
        _lastFramePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //update hand position and rotation
        transform.localPosition = InputTracking.GetLocalPosition(NodeType);
        transform.localRotation = InputTracking.GetLocalRotation(NodeType);

        //if we don't have an active object in hand, look if there is one in proximity

        if (_currentObject == null)
        {
            // check for colliders proximity
            Collider[] colliders = Physics.OverlapSphere(transform.position, GrabDistance);

            if (colliders.Length > 0)
            {
                // if there are colliders, take the first one if we press the grab btn and it has the tag for grabbing
                if (Input.GetAxis(InputName) >= 0.01f && colliders[0].transform.CompareTag(GrabTag))
                {
                    // set current objects to the object we have picked
                    _currentObject = colliders[0].transform;

                    // if there is no rigidbody to the grabbed object attached, add one

                    if (_currentObject.GetComponent<Rigidbody>() == null)
                    {
                        _currentObject.gameObject.AddComponent<Rigidbody>();
                    }

                    // set grab object to kinematic (disable physics)
                    _currentObject.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
        else
        {
            // we have object in hand, update it's position with the current hand position (+defined offset from it)
            _currentObject.position = transform.position + ObjectGrabOffset;

            // if we release the grab button, release current object
            if (Input.GetAxis(InputName) < 0.01f)
            {
                // set grab object to non-kinematic (enable physics)
                Rigidbody _objectRGB = _currentObject.GetComponent<Rigidbody>();
                _objectRGB.isKinematic = false;

                Vector3 CurrentVelocity = (transform.position - _lastFramePosition) / Time.deltaTime;
                _objectRGB.velocity = CurrentVelocity * ThrowMultiplier;

                // release the reference
                _currentObject = null;
            }
        }

        // save the current position for calculation of velocity in next frame
        _lastFramePosition = transform.position;
    }
}
