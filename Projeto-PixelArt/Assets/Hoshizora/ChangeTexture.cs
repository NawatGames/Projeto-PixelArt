using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeTexture : MonoBehaviour
{
    [SerializeField] public Material materialTexture;
    [SerializeField] public List<Texture2D> textures;
    [SerializeField] public InputAction changeAction;
    [SerializeField] public InputAction changeBackAction;
    [SerializeField] public string path = "textures";

    [HideInInspector] public string previousPath;

    private int i;

    void OnEnable()
    {
        changeAction.Enable();
        changeBackAction.Enable();
    }

    void OnDisable()
    {
        changeAction.Disable();
        changeBackAction.Disable();
    }

    public void Change()
    {
        if (i + 2 <= textures.Count)
        {
            i++;
        }
        else i = 0;
        materialTexture.mainTexture = textures[i];
    }
    
    public void ChangeBack()
    {
        if (i > 0)
        {
            i--;
        }
        else i = textures.Count - 1;
        materialTexture.mainTexture = textures[i];   
    }

    public void Awake()
    {
        previousPath = path;
        if (Directory.Exists(Application.dataPath + "/" + path))
        {
            string[] filePaths = Directory.GetFiles(Application.dataPath + "/" + path, "*.png");
            foreach (var fileName in filePaths)
            {
                textures.Add(CreateTexture(fileName));
            }
        }

        i = 0;
        materialTexture.mainTexture = textures[i];
    }

    public Texture2D CreateTexture(string imagePath)
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

    public void Update()
    {
        if (changeAction.triggered)
        {
            Change();
        }
        if (changeBackAction.triggered)
        {
            ChangeBack();
        }
    }
}
