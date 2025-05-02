using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to fix UI position if that UI is off screen & yang bikin fungsinya skill issue.
/// </summary>
public static class UIOffScreenHandler
{
    /// <summary>
    /// Re-adjust or reposition UI if the UI position is off screen.
    /// </summary>
    /// <param name="rectTransform"></param>
    /// <param name="camera">Pass with null if Canvas isn't world space (e.g. Screen Space - Overlay).</param>
    /// <returns></returns>
    public static RectTransform FixUIOffScreen(this RectTransform rectTransform, Camera camera)
    {
        Rect screenBounds = new Rect(0f, 0f, Screen.width, Screen.height);
        Vector3[] objectCorners = new Vector3[4];
        rectTransform.GetWorldCorners(objectCorners);
        
        Vector3 tempScreenSpaceCorner;

        for (int i = 0; i < objectCorners.Length; i++)
        {
            if (camera != null)
                tempScreenSpaceCorner = camera.WorldToScreenPoint(objectCorners[i]);
            else
                tempScreenSpaceCorner = objectCorners[i];

            if (!screenBounds.Contains(tempScreenSpaceCorner) && tempScreenSpaceCorner.x > screenBounds.max.x + 10 || tempScreenSpaceCorner.x < -10
                || tempScreenSpaceCorner.y > screenBounds.max.y + 10 || tempScreenSpaceCorner.y < -10)
            {
                rectTransform.MoveUIPartially(screenBounds, tempScreenSpaceCorner);
                Debug.Log(tempScreenSpaceCorner);
                Debug.Log(objectCorners[i]);
                break;
            }
            
            Debug.Log($"Aman");
        }

        return rectTransform;
    }

    /// <summary>
    /// Check if UI isn't off screen, return an int and break the loop once it finds the corner that is off screen.
    /// </summary>
    /// <param name="rectTransform"></param>
    /// <param name="camera">Pass with null if Canvas isn't world space (e.g. Screen Space - Overlay).</param>
    /// <returns></returns>
    public static int CheckUIOffScreen(this RectTransform rectTransform, Camera camera)
    {
        Rect screenBounds = new Rect(0f, 0f, Screen.width, Screen.height);
        Vector3[] objectCorners = new Vector3[4];
        rectTransform.GetWorldCorners(objectCorners);

        int clearCorner = 0;
        Vector3 tempScreenSpaceCorner;

        for (int i = 0; i < objectCorners.Length; i++)
        {
            if (camera != null)
                tempScreenSpaceCorner = camera.WorldToScreenPoint(objectCorners[i]);
            else
                tempScreenSpaceCorner = objectCorners[i];

            if (!screenBounds.Contains(tempScreenSpaceCorner) && tempScreenSpaceCorner.x > screenBounds.max.x + 10 || tempScreenSpaceCorner.x < -10
                || tempScreenSpaceCorner.y > screenBounds.max.y + 10 || tempScreenSpaceCorner.y < -10)
                break;

            clearCorner++;
        }

        return clearCorner;
    }

    private static void ReadjustUIPosition(this RectTransform rectTransform, TooltipFixDirection direction, Rect screenBound, Vector3 cornerPos)
    {
        float gap = 0;
        
        //Vector3 localPosition = rectTransform.InverseTransformPoint(cornerPos);

        Debug.Log($"Before, offset max is {rectTransform.offsetMax} offset min is {rectTransform.offsetMin}");

        switch (direction)
        {
            //Tooltip's off screen is on the left side of the screen. So move ui to right
            case TooltipFixDirection.HorizontalLeft:
                Debug.Log($"Horizontal left");
                
                gap = Math.Abs(screenBound.min.x - cornerPos.x);
                
                rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x + gap, rectTransform.offsetMin.y);
                rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x + gap, rectTransform.offsetMax.y);
                
                //rectTransform.localPosition = new Vector3(localPosition.x + gap, localPosition.y, localPosition.z);

                /*if (rectTransform.offsetMin.x >= -10 && rectTransform.offsetMin.x < 0)
                    rectTransform.offsetMin = new Vector2(1, rectTransform.offsetMin.y);*/
                
                break;
            
            //On right side of the screen. So move ui to the left
            case TooltipFixDirection.HorizontalRight:
                Debug.Log($"Horizontal right");
                
                gap = Math.Abs(screenBound.max.x - cornerPos.x);
                
                rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x - gap, rectTransform.offsetMin.y);
                rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x - gap, rectTransform.offsetMax.y);

                //rectTransform.localPosition = new Vector3(localPosition.x - gap, localPosition.y, localPosition.z);

                /*if (rectTransform.offsetMax.x <= rectTransform.offsetMax.x + 10 && rectTransform.offsetMax.x > screenBound.max.x)
                    rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x - 1, rectTransform.offsetMax.y);*/
                
                break;
            
            //On bottom side of the screen. So move ui to top
            case TooltipFixDirection.VerticalBottom:
                Debug.Log($"Vertical bottom");
                gap = Math.Abs(screenBound.min.y - cornerPos.y);
                
                rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, rectTransform.offsetMin.y + gap);
                rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, rectTransform.offsetMax.y + gap);
                
                /*Debug.Log(cornerPos);
                Debug.Log($"OKe");*/
                
                //rectTransform.localPosition = new Vector3(localPosition.x + gap, localPosition.y + gap, localPosition.z);

                /*if (rectTransform.offsetMin.y >= -10 && rectTransform.offsetMin.y < 0)
                    rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, 1);*/
                
                break;
            
            //On top side of the screen. So move ui to bottom
            case TooltipFixDirection.VerticalTop:
                Debug.Log($"Vertical top");
                
                gap = Math.Abs(screenBound.max.y - cornerPos.y);
                
                rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, rectTransform.offsetMin.y - gap);
                rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, rectTransform.offsetMax.y - gap);
                
                //rectTransform.localPosition = new Vector3(localPosition.x, localPosition.y - gap, localPosition.z);
                
                /*if (rectTransform.offsetMax.y <= rectTransform.offsetMax.y + 10 && rectTransform.offsetMax.y > screenBound.max.y)
                    rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x - 1, rectTransform.offsetMax.y);*/
                
                break;
        }

        Debug.Log($"After, offset max is {rectTransform.offsetMax} offset min is {rectTransform.offsetMin}");
    }

    /// <summary>
    /// Check which side that the UI is off screen and run a method to fix it with it's directional
    /// </summary>
    /// <param name="rectTransform"></param>
    /// <param name="screenBound"></param>
    /// <param name="cornerPos"></param>
    /// <returns></returns>
    public static RectTransform MoveUIPartially(this RectTransform rectTransform, Rect screenBound, Vector3 cornerPos)
    {
        if (cornerPos.x > screenBound.max.x)
            rectTransform.ReadjustUIPosition(TooltipFixDirection.HorizontalRight, screenBound, cornerPos);
        else if (cornerPos.x < screenBound.min.x)
            rectTransform.ReadjustUIPosition(TooltipFixDirection.HorizontalLeft, screenBound, cornerPos);

        if (cornerPos.y > screenBound.max.y)
            rectTransform.ReadjustUIPosition(TooltipFixDirection.VerticalTop, screenBound, cornerPos);
        else if (cornerPos.y < screenBound.min.y)
            rectTransform.ReadjustUIPosition(TooltipFixDirection.VerticalBottom, screenBound, cornerPos);

        return rectTransform;
    }
    
    /// <summary>
    /// Off screen's direction for the UI
    /// </summary>
    public enum TooltipFixDirection
    {
        HorizontalLeft,
        HorizontalRight,
        VerticalBottom,
        VerticalTop
    }
}
