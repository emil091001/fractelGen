using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class FractalSettings : MonoBehaviour
{
    public ComputeShader shader;

    public int MAX_MARCH_ITERATIONS = 200;

    public float MIN_DISTANCE = 0.00001f;

    public float epsillon = 8.0f;

    public float a1 = 0;

    public float a2 = 0;

    public float a3 = 0;

    public float b1 = 0;

    public float b2 = 0;

    public float b3 = 0;

    public float CX = 1;

    public float CY = 1;

    public float CZ = 1;

    public float scale = 3;

    public Color colorMultiplier = Color.white;

    public Color colorOffset = Color.black;

    private RenderTexture screenCanvas;

    private float distence;

    public float getDistence()
    {
        return distence;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (screenCanvas == null)
        {
            screenCanvas = new RenderTexture(Screen.width, Screen.height, 24);
            screenCanvas.enableRandomWrite = true;
            screenCanvas.Create();
        }

        shader.SetTexture(0, "Result", screenCanvas);
        shader.SetMatrix("_CameraToWorld", Camera.current.cameraToWorldMatrix);
        shader
            .SetMatrix("_CameraInverseProjection",
            Camera.current.projectionMatrix.inverse);
        shader
            .SetVector("_LightDirection",
            FindObjectOfType<Light>().transform.forward);

        shader.SetInt("MAX_MARCH_ITERATIONS", MAX_MARCH_ITERATIONS);
        shader.SetFloat("MIN_DISTANCE", MIN_DISTANCE);
        shader.SetFloat("epsillon", epsillon);

        shader.SetFloat("a1", a1);
        shader.SetFloat("a2", a2);
        shader.SetFloat("a3", a3);

        shader.SetFloat("b1", b1);
        shader.SetFloat("b2", b2);
        shader.SetFloat("b3", b3);

        shader.SetFloat("CX", CX);
        shader.SetFloat("CY", CY);
        shader.SetFloat("CZ", CZ);

        shader.SetFloat("scale", scale);

        shader.SetVector("colorMultiplier", colorMultiplier);
        shader.SetVector("colorOffset", colorOffset);

        shader.Dispatch(0, Screen.width / 8, Screen.height / 8, 1);

        GetAlphaValueAtPixel();

        Graphics.Blit (screenCanvas, destination);
    }

    void GetAlphaValueAtPixel()
    {
        // Set the read target to the RenderTexture
        RenderTexture.active = screenCanvas;

        // Create a texture2D and read the pixels
        Texture2D tex = new Texture2D(screenCanvas.width, screenCanvas.height);
        tex
            .ReadPixels(new Rect(0, 0, screenCanvas.width, screenCanvas.height),
            0,
            0);
        tex.Apply();

        // Get the alpha value at the specified pixel
        Color pixelColor = tex.GetPixel(0, 0);
        distence = pixelColor.a;

        //Debug.Log (distence);
        // Reset active render texture
        RenderTexture.active = null;

        // Now 'alpha' contains the alpha value of the specified pixel
    }
}
