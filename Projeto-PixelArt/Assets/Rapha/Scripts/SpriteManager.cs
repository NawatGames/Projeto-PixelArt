using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpriteManager : MonoBehaviour
{
    [SerializeField] public Material characterMaterial;
    [SerializeField] public string spritesheetsPath = "spritesheets";
    [SerializeField] public List<Texture2D> spritesheets;

    private string _previousPath;
    private int _i = 0;
    
    void Awake()
    {
        _previousPath = spritesheetsPath;
        if (Directory.Exists(Application.dataPath + "/" + spritesheetsPath))
        {
            string[] filePaths = Directory.GetFiles(Application.dataPath + "/" + spritesheetsPath, "*.png");
            foreach (var fileName in filePaths)
            {
                spritesheets.Add(CreateTexture(fileName));
            }
        }

        characterMaterial.mainTexture = spritesheets[_i];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private Texture2D CreateTexture(string imagePath)
    {
        Texture2D texture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
        byte[] fileData = File.ReadAllBytes(imagePath);
        
        texture.LoadImage(fileData);

        Texture2D textureAlpha = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        textureAlpha.SetPixels(texture.GetPixels());
        textureAlpha.filterMode = FilterMode.Point;
        textureAlpha.Apply();
        return textureAlpha;
    }
}
