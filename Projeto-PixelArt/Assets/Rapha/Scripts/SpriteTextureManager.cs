using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpriteTextureManager : MonoBehaviour
{
    [SerializeField] public Material characterMaterial;
    [SerializeField] public string spritesheetsPath = "spritesheets";
    [SerializeField] public List<Texture2D> spritesheets;
    
    [SerializeField] private InputAction nextTextureAction;
    [SerializeField] private InputAction previousTextureAction;
    private string _previousPath;
    private int _i = 0;

    public Texture2D GetCurrentTexture2D()
    {
        return spritesheets[_i];
    }

    public Material GetCharacterMaterial()
    {
        return characterMaterial;
    }

    private void Awake()
    {
        GetDirectoryTextures();
    }

    // Update is called once per frame
    private void Update()
    {
        if (nextTextureAction.triggered) ChangeToNextTexture();
        if (previousTextureAction.triggered) ChangeToPreviousTexture();
    }

    private void ChangeToNextTexture()
    {
        if (_i + 2 <= spritesheets.Count)
        {
            _i++;
        }
        else _i = 0;
    }
    
    private void ChangeToPreviousTexture()
    {
        if (_i > 0)
        {
            _i--;
        }
        else _i = spritesheets.Count - 1;
    }
    
    private void OnEnable()
    {
        nextTextureAction.Enable();
        previousTextureAction.Enable();
    }

    private void OnDisable()
    {
        nextTextureAction.Disable();
        previousTextureAction.Disable();
    }

    private void GetDirectoryTextures()
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
    }
    
    private Texture2D CreateTexture(string imagePath)
    {
        Texture2D texture = new Texture2D(48, 128, TextureFormat.RGBA32, false);
        byte[] fileData = File.ReadAllBytes(imagePath);
        
        texture.LoadImage(fileData);

        Texture2D textureAlpha = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        textureAlpha.SetPixels(texture.GetPixels());
        textureAlpha.filterMode = FilterMode.Point;
        textureAlpha.Apply();
        return textureAlpha;
    }
}
