using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimationManager : MonoBehaviour
{
    [SerializeField] public SpriteTextureManager spriteTextureManager;
    private Material characterMaterial;
    private Texture2D currentSpritesheetTexture;

    private void Start() // Start comes after Awake
    {
        if (!spriteTextureManager) Debug.LogError($"No SpriteTextureManager attached to {gameObject}'s SpriteAnimationManager");
        characterMaterial = spriteTextureManager.GetCharacterMaterial();
        currentSpritesheetTexture = spriteTextureManager.GetCurrentTexture2D();
    }
}
