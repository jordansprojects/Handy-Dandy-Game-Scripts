using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/***************
I created this script by following this tutorial! :)  : https://www.youtube.com/watch?v=iasDPyC0QOg

***************/
[RequireComponent(typeof(Rigidbody))]
public class Buoyancy : MonoBehaviour
{

    [SerializeField] Transform [] floaters;
    [SerializeField] float underWaterDrag = 3f;
    [SerializeField] float underWaterAngularDrag =1f;
    [SerializeField] float airDrag =0f;
    [SerializeField] float angularAirDrag = 0.05f;

    // Start is called before the first frame update
    [SerializeField] float floatingPower =15f;

    [SerializeField] float waterHeight =0f;

    bool underwater;

    int floatersUnderwater;

    Rigidbody m_rigidbody;
    void Start(){
        m_rigidbody  = GetComponent<Rigidbody>();
    }

    void FixedUpdate(){

        floatersUnderwater = 0;
        for( int i =0; i < floaters.Length; i++){
        float difference = floaters[i].position.y - waterHeight;

        if (difference <  0){
            GetComponent<Rigidbody>().AddForceAtPosition( Vector3.up * floatingPower * Mathf.Abs(difference), floaters[i].position, ForceMode.Force);
            floatersUnderwater++;
            if (!underwater){
                underwater = true;
                SwitchState(true);
            }

        } 
    }
    if (underwater && floatersUnderwater == 0){
            underwater = false;
            SwitchState(true);
        }


    } // End of FixedUpdate


    void SwitchState( bool isUnderwater){
        if(isUnderwater){
            m_rigidbody.drag = underWaterDrag;
            m_rigidbody.angularDrag = underWaterAngularDrag;
        }else{
             m_rigidbody.drag = airDrag;
            m_rigidbody.angularDrag = angularAirDrag;
        }

    }


    void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        for (int i = 0; i < floaters.Length; i++)
            Gizmos.DrawWireSphere(floaters[i].position,0.25f);
    }

}// end of Buoyancy script
