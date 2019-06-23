using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ConfigurableJoint))]
public class PhysGrab : MonoBehaviour
{
    ConfigurableJoint CFJoint;
    Rigidbody Rb;
    private GameObject CurObject; //var for the object you are currently holding. 

    public List<GameObject> NearestObj = new List<GameObject>();

    private void Awake()
    {
        CFJoint = GetComponent<ConfigurableJoint>();
        Rb = GetComponent<Rigidbody>(); 
       
    }

    private void Update()
    {
        Movement();

        if (Input.GetKeyDown(KeyCode.K))
        {
            Grip();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Drop(); 
        }
    }

    void Movement()
    {
        float hor = Input.GetAxis("Horizontal") * 10f * Time.deltaTime;
        float ver = Input.GetAxis("Vertical") * 10f * Time.deltaTime;

        transform.Translate(hor, ver, 0);
    }

    private void Grip()
    {
        if (NearestObj.Count > 0)
        {
            if (CurObject == null)
            {
                Debug.Log("Object is now being held");
                CurObject = ClosestObj();
                CFJoint.connectedBody = CurObject.GetComponent<Rigidbody>();

            }
        }
    }

    private void Drop()
    {
        if (CurObject != null)
        {
            Debug.Log("I am dropping the object");
            CFJoint.connectedBody = null;
            CurObject.GetComponent<Rigidbody>().velocity = Rb.velocity;
            
            CurObject = null;
        }
    }

    public GameObject ClosestObj() // This checks which object is closest to you. 
    {
        GameObject obj = null;
        float shortestDis = float.MaxValue;
        Debug.Log("n objects: " + NearestObj.Count);
        if (NearestObj.Count > 0)
        {
            foreach (GameObject gameObj in NearestObj)
            {
                float currentDis = Vector3.Distance(gameObj.transform.position, transform.position);
                if (currentDis < shortestDis)// checks if the objects currentdistance is smaller then the shortest distance. 
                {
                    obj = gameObj;
                    shortestDis = currentDis; // this sets a new shortest distance. 
                }
            }
        }
        return obj;
    }

    #region Triggers

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Interactable")) //Checks if the object you are overlapping with has the correct tag. 
        {
            Debug.Log("I am overlapping a Interactable");
            NearestObj.Add(other.gameObject); //This adds the object to a list of NearestObj. 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            Debug.Log("I have removed a Interactable");
            NearestObj.Remove(other.gameObject); //This removes the items not overlapping with the triggers. 
        }
    }
    #endregion

}
