using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpriteAnimationManager : MonoBehaviour
{
    [SerializeField] public SpriteTextureManager spriteTextureManager;
    [SerializeField] public InputActionAsset playerActionAsset;
    
    private Material characterMaterial;
    private Texture2D currentSpritesheetTexture;
    private InputAction playerAction;
    private PLAYER_STATE currentState = PLAYER_STATE.IDLE;

    private void Start() // Start comes after Awake
    {
        if (!spriteTextureManager)
            Debug.LogError($"No SpriteTextureManager attached to {gameObject}'s SpriteAnimationManager");
        if (!playerActionAsset)
            Debug.LogError($"No InputActionAsset attached to {gameObject}'s SpriteAnimationManager");
        characterMaterial = spriteTextureManager.GetCharacterMaterial();
        currentSpritesheetTexture = spriteTextureManager.GetCurrentTexture2D();
        playerAction = playerActionAsset.FindActionMap("Player").FindAction("Move");
    }

    private void Update()
    {
        SetCurrentState();
        Debug.Log(currentState);
    }

    private void SetCurrentState()
    {
        var objInput = playerAction.ReadValueAsObject();
        if (objInput is null)
        {
            currentState = PLAYER_STATE.IDLE;
            return;
        }
        
        var vec2input = (Vector2) objInput;
        var xInput = vec2input[0];
        var yInput = vec2input[1];

        currentState = yInput switch
        {
            1.0f => PLAYER_STATE.WALKING_FRONT,
            -1.0f => PLAYER_STATE.WALKING_BACK,
            _ => xInput switch
            {
                1.0f => PLAYER_STATE.WAKING_RIGHT,
                -1.0f => PLAYER_STATE.WALINKG_LEFT,
                _ => PLAYER_STATE.IDLE
            }
        };
    }


    private enum PLAYER_STATE
    {
        IDLE,
        WAKING_RIGHT,
        WALINKG_LEFT,
        WALKING_FRONT,
        WALKING_BACK
    }
}