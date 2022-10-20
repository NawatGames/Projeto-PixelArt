using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimationManager : MonoBehaviour
{
    [SerializeField] public SpriteTextureManager spriteTextureManager;
    private Material characterMaterial;
    private Texture2D currentSpritesheetTexture;

    private void Awake()
    {
        if (!spriteTextureManager) Debug.LogError($"No SpriteTextureManager attached to {gameObject}'s SpriteAnimationManager");
    }
}
