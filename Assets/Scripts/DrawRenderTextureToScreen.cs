using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRenderTextureToScreen : MonoBehaviour {

    public RenderTexture theTexture;

	void OnPreRender(){
        Camera.main.targetTexture = theTexture;
    }
 
    void OnPostRender() {
        Camera.main.targetTexture = null;
        Graphics.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), theTexture);
    }
}
