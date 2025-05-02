using UnityEngine;

public class InitialPosRotBehaviour : MonoBehaviour
{
    public Vector3 GetPos;
    
    public Vector3 GetLocalPos;
    
    public Quaternion GetRot;

    public Vector3 GetLocalScale;

    public Vector2 SizeDelta;

/*    public enum scaleobject{
        Big,
        Small
    }

    public scaleobject ScaleObject;*/

    public void Awake ()
    {
        if (transform.GetComponent<RectTransform>() != null)
        {
            SizeDelta = transform.GetComponent<RectTransform>().sizeDelta;
        }

        var objTransform = transform;
        
        GetLocalScale = objTransform.localScale;
        GetPos = objTransform.position;
        GetLocalPos = objTransform.localPosition;
        GetRot = objTransform.rotation;
    }
}