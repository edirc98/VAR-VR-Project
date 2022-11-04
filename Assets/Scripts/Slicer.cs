using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class Slicer : MonoBehaviour
{
    #region PUBLIC PARAMETERS
    public GameObject hullsParent;
    public Material crossSectionMaterial;
    public bool isCutting = false;
    //public GameObject ObjectToCut; 
    #endregion
    #region PROTECTED PARAMETERS
    [SerializeField]
    public GameObject _cuttingPlane;
    #endregion

    #region PRIVATE PARAMETERS
    private GameObject resultUp;
    private GameObject resultLow;
     
    #endregion
    

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name +"||"+ other.gameObject.tag + "||" + other.gameObject.layer);
        if (!isCutting)
        {
            isCutting = true;
            if (other.gameObject.layer == 6 && other.CompareTag("SpawnedMat"))
            {
                SlicedHull hull = sliceObject(other.gameObject, crossSectionMaterial);
                if (hull != null)
                {
                    resultLow = hull.CreateLowerHull(other.gameObject, crossSectionMaterial);
                    resultUp = hull.CreateUpperHull(other.gameObject, crossSectionMaterial);

                }
                else
                {
                    Debug.Log("Hull was null");
                }
                if (resultUp != null && resultLow != null)
                {
                    Destroy(other.gameObject);
                    updateResultObject(resultUp);
                    updateResultObject(resultLow);
                }
                else
                {
                    Destroy(resultLow);
                    Destroy(resultUp);
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
        if(result != null)
        {
            //result.layer = 6;
            result.transform.parent = hullsParent.transform;
            //Add Collider to the result
            MeshCollider collider =  result.AddComponent<MeshCollider>();
            if(collider != null)
            {
                collider.convex = true;
                //Add rigidbody if collider is not null to avoid problems
                Rigidbody rb = result.AddComponent<Rigidbody>();
                if (rb != null)
                { 
                    rb.mass = 2;
                    rb.useGravity = true;
                    rb.AddForce(new Vector3(0.5f, 0.1f, 0), ForceMode.Impulse);
                }
            }
        }
        //if result was null destroy it
        else
        {
            Destroy(result);
        }
        
    }

}
