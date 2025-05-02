using System.Collections;
using System.Collections.Generic;
using Paroxe.PdfRenderer;
using UnityEngine;

public class PDFChangeRotation : MonoBehaviour
{
    public Transform RectPdfContainerView;
    public PDFViewer PdfViewer;
    [Tooltip("Determine whether each pages are now horizontal (rotated) or vertical.")]
    [HideInInspector] public bool NowHorizontal;
    private void PDFChangeRotate(bool _bIsClockwise)
    {
        //List<Transform> _listTransPdfViewRawImage = new List<Transform>();

        foreach (Transform _transChild in RectPdfContainerView)
        {
            if (_bIsClockwise) {
                _transChild.rotation = Quaternion.Euler(0f, 0f, _transChild.rotation.eulerAngles.z + 90f);
            } else {
                _transChild.rotation = Quaternion.Euler(0f, 0f, _transChild.rotation.eulerAngles.z + -90f);
            }
        }
        NowHorizontal = !NowHorizontal;
        PdfViewer.NowHorizontal = NowHorizontal;
        PdfViewer.m_Internal.PageContainer.sizeDelta = PdfViewer.GetDocumentSize();
        PdfViewer.CheckDocAndContainer();
        PdfViewer.UpdateScrollBarVisibility();
        
        //_pdfViewerCs.CustomComputePageOffsets();
    }

    public void RotateLeft()
    {
        PDFChangeRotate(true);
    }

    public void RotateRight()
    {
        PDFChangeRotate(false);
    }
}
