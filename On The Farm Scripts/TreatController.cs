using UnityEngine;
using System.Collections;
using System;


public class TreatController : MonoBehaviour
{
    public ParticleSystem sparkles; 
    public bool pickedUpBefore = false;
    public bool isHeld = false;
    public static event Action<GameObject> OnTreatConsumed;

    private IEnumerator coroutine;
    public void SetHeld(bool held)
    {
        pickedUpBefore = true;
        isHeld = held;
        
    }


    public void Consume()
    {
        emit();
        OnTreatConsumed?.Invoke(this.gameObject);
        Destroy(this.gameObject, .45f);
        
    }

    public void emit(){
        sparkles.Play();
    }


}
