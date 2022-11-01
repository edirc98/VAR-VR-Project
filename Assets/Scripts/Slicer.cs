using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class Slicer : MonoBehaviour
{
    #region PUBLIC PARAMETERS
    public Material crossSectionMaterial;
    public bool isCutting = false;
    //public GameObject ObjectToCut; 
    #endregion
    #region PROTECTED PARAMETERS
    [SerializeField]
    public GameObject _cuttingPlane;
    #endregion

    #region PRIVATE PARAMETERS
    private EzySlice.Plane cutter; 
    private GameObject _objInColision;
    private GameObject resultUp;
    private GameObject resultLow;
     
    #endregion
    

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if (!isCutting)
        {
            isCutting = true;
            if (other.gameObject.layer == 10)
            {
                SlicedHull hull = sliceObject(other.gameObject, crossSectionMaterial);
                if (hull != null)
                {
                    resultLow = hull.CreateLowerHull(other.gameObject, crossSectionMaterial);
                    resultUp = hull.CreateUpperHull(other.gameObject, crossSectionMaterial);

                    other.gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("Hull was null");
                }
                if (resultUp != null && resultLow != null)
                {
                    updateResultObject(resultUp);
                    updateResultObject(resultLow);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isCutting = false; 
    }

    private SlicedHull sliceObject(GameObject obj,Material crossSectionMat)
    {
        return obj.Slice(_cuttingPlane.transform.position, _cuttingPlane.transform.up, crossSectionMat);
    }

    private void updateResultObject(GameObject result)
    {
        result.layer = 10; 
        result.AddComponent<BoxCollider>();
        Rigidbody rb = result.AddComponent<Rigidbody>();
        rb.mass = 2;
        rb.useGravity = true;
    }

}
