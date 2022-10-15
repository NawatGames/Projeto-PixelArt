using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    public Material materialTexture;
    public List<Texture2D> textures;
    public int i;
[ContextMenu("change")]

    public void Change(){
        i++;
        materialTexture.mainTexture = textures[i];
        
    }
[ContextMenu("changeReturn")]
    public void ChangeReturn(){
        i--;
        materialTexture.mainTexture = textures[i];   
    }
}
