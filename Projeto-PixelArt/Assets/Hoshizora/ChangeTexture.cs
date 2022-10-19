using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeTexture : MonoBehaviour
{
    [SerializeField] public Material materialTexture;
    [SerializeField] public List<Texture2D> textures;
    [SerializeField] public InputAction changeAction;
    [SerializeField] public InputAction changeBackAction;
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
        materialTexture.mainTexture = textures[i];
        
    }
    
    public void ChangeBack()
    {
        if (i > 0)
        {
            i--;
        }
        materialTexture.mainTexture = textures[i];   
    }

    public void Start()
    {
        i = 0;
        materialTexture.mainTexture = textures[i];
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
