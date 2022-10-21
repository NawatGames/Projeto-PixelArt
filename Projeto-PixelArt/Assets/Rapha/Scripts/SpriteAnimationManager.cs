using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpriteAnimationManager : MonoBehaviour
{
    [SerializeField] public SpriteTextureManager spriteTextureManager;
    [SerializeField] public InputActionAsset playerActionAsset;
    [SerializeField] public float timeBetweenFramesInSeconds = 0.1f;
    
    private Material _characterMaterial;
    private TexturePack _texturePack;
    private InputAction _playerAction;
    private PLAYER_STATE _currentState = PLAYER_STATE.IDLE;
    private bool _isAnimationPlaying;
    
    // Gambiarras
    private int _textureIndex;
    private bool _hasExecutedFirstTexture = false;

    private int _textureSpriteRow = 8;
    private int _textureSpriteCollumns = 3;

    private void Start() // Start comes after Awake
    {
        if (!spriteTextureManager)
            Debug.LogError($"No SpriteTextureManager attached to {gameObject}'s SpriteAnimationManager");
        if (!playerActionAsset)
            Debug.LogError($"No InputActionAsset attached to {gameObject}'s SpriteAnimationManager");
        _characterMaterial = spriteTextureManager.GetCharacterMaterial();
        _playerAction = playerActionAsset.FindActionMap("Player").FindAction("Move");
        _textureIndex = spriteTextureManager.GetTextureIndex();
        _isAnimationPlaying = false;
    }

    private void Update()
    {
        SetCurrentState();
        AnimatePlayer();
    }
    
    private void SetCurrentState()
    {
        var objInput = _playerAction.ReadValueAsObject();
        if (objInput is null)
        {
            _currentState = PLAYER_STATE.IDLE;
            return;
        }
        
        var vec2input = (Vector2) objInput;
        var xInput = vec2input[0];
        var yInput = vec2input[1];

        _currentState = yInput switch
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

    private void AnimatePlayer()
    {
        _texturePack = CutTexture2D();
        StartCoroutine(PlayAnimation(_texturePack));
    }
    
    private TexturePack CutTexture2D()
    {
        // Full texture format: 48p x 128p
        // Sprite format: 16p x 16p
        // Pivot -> 0,0 -> Down-Left cornet

        Resources.UnloadUnusedAssets();
        var currentTextureIndex = spriteTextureManager.GetTextureIndex();
        
        if (!_hasExecutedFirstTexture) _hasExecutedFirstTexture = true;
        else if (currentTextureIndex == _textureIndex) return _texturePack;
        
        _textureIndex = currentTextureIndex;
        
        var texture = spriteTextureManager.GetCurrentTexture2D();
        var textureList = new List<Texture2D>();
        
        var newTextureSize = new Vector2Int(16, 16);
        
        for (var row = _textureSpriteRow - 1; row >= 0; row--)
        {
            for (var column = 0; column < _textureSpriteCollumns; column++)
            {
                var auxTexture = new Texture2D(newTextureSize.x, newTextureSize.y);
                var coordinate = GetTextureSpriteCoordinate(
                    new Vector2Int(column, row),
                    newTextureSize);
                var textureColors = texture.GetPixels(coordinate.x, coordinate.y, newTextureSize.x, newTextureSize.y);
                auxTexture.SetPixels(textureColors);
                auxTexture.filterMode = FilterMode.Point;
                auxTexture.Apply();
                textureList.Add(auxTexture);
            }
        }

        return new TexturePack(textureList);
    }

    private Vector2Int GetTextureSpriteCoordinate( Vector2Int textureSpritePosition, Vector2Int newTextureSize)
    {
        var newCoordinate = textureSpritePosition * newTextureSize;
        return newCoordinate;
    }

    private IEnumerator PlayAnimation(TexturePack texture2Ds)
    {
        if (_isAnimationPlaying) yield break;
        _isAnimationPlaying = true; // Lock

        switch (_currentState)
        {
            case PLAYER_STATE.IDLE:
                _characterMaterial.mainTexture = texture2Ds.IdleTexture2Ds[0];
                yield return new WaitForSeconds(timeBetweenFramesInSeconds);
                _characterMaterial.mainTexture = texture2Ds.IdleTexture2Ds[1];
                yield return new WaitForSeconds(timeBetweenFramesInSeconds);
                _characterMaterial.mainTexture = texture2Ds.IdleTexture2Ds[2];
                yield return new WaitForSeconds(timeBetweenFramesInSeconds);
                break;
            case PLAYER_STATE.WAKING_RIGHT:
                _characterMaterial.mainTexture = texture2Ds.WalkingRightTexture2Ds[0];
                yield return new WaitForSeconds(timeBetweenFramesInSeconds);
                _characterMaterial.mainTexture = texture2Ds.WalkingRightTexture2Ds[1];
                yield return new WaitForSeconds(timeBetweenFramesInSeconds);
                _characterMaterial.mainTexture = texture2Ds.WalkingRightTexture2Ds[2];
                yield return new WaitForSeconds(timeBetweenFramesInSeconds);
                break;
            case PLAYER_STATE.WALINKG_LEFT:
                _characterMaterial.mainTexture = texture2Ds.WalkingLeftTexture2Ds[0];
                yield return new WaitForSeconds(timeBetweenFramesInSeconds);
                _characterMaterial.mainTexture = texture2Ds.WalkingLeftTexture2Ds[1];
                yield return new WaitForSeconds(timeBetweenFramesInSeconds);
                _characterMaterial.mainTexture = texture2Ds.WalkingLeftTexture2Ds[2];
                yield return new WaitForSeconds(timeBetweenFramesInSeconds);
                break;
            case PLAYER_STATE.WALKING_FRONT:
                _characterMaterial.mainTexture = texture2Ds.WalkingFrontTexture2Ds[0];
                yield return new WaitForSeconds(timeBetweenFramesInSeconds);
                _characterMaterial.mainTexture = texture2Ds.WalkingFrontTexture2Ds[1];
                yield return new WaitForSeconds(timeBetweenFramesInSeconds);
                _characterMaterial.mainTexture = texture2Ds.WalkingFrontTexture2Ds[2];
                yield return new WaitForSeconds(timeBetweenFramesInSeconds);
                break;
            case PLAYER_STATE.WALKING_BACK:
                _characterMaterial.mainTexture = texture2Ds.WalkingBackTexture2Ds[0];
                yield return new WaitForSeconds(timeBetweenFramesInSeconds);
                _characterMaterial.mainTexture = texture2Ds.WalkingBackTexture2Ds[1];
                yield return new WaitForSeconds(timeBetweenFramesInSeconds);
                _characterMaterial.mainTexture = texture2Ds.WalkingBackTexture2Ds[2];
                yield return new WaitForSeconds(timeBetweenFramesInSeconds);
                break;
            default:
                _characterMaterial.mainTexture = texture2Ds.IdleTexture2Ds[0];
                yield return new WaitForSeconds(timeBetweenFramesInSeconds);
                _characterMaterial.mainTexture = texture2Ds.IdleTexture2Ds[1];
                yield return new WaitForSeconds(timeBetweenFramesInSeconds);
                _characterMaterial.mainTexture = texture2Ds.IdleTexture2Ds[2];
                yield return new WaitForSeconds(timeBetweenFramesInSeconds);
                break;
        }

        _isAnimationPlaying = false; // Unlock
    }

    private enum PLAYER_STATE
    {
        IDLE,
        WAKING_RIGHT,
        WALINKG_LEFT,
        WALKING_FRONT,
        WALKING_BACK
    }

    private class TexturePack
    {
        public readonly List<Texture2D> IdleTexture2Ds;
        public readonly List<Texture2D> WalkingRightTexture2Ds;
        public readonly List<Texture2D> WalkingLeftTexture2Ds;
        public readonly List<Texture2D> WalkingFrontTexture2Ds;
        public readonly List<Texture2D> WalkingBackTexture2Ds;

        public TexturePack(IReadOnlyList<Texture2D> textures)
        {
            IdleTexture2Ds = new List<Texture2D>() { textures[12], textures[13], textures[14] };
            WalkingRightTexture2Ds = new List<Texture2D>() { textures[3], textures[4], textures[5] };
            WalkingLeftTexture2Ds = new List<Texture2D>() { textures[6], textures[7], textures[8] };
            WalkingFrontTexture2Ds = new List<Texture2D>() { textures[9], textures[10], textures[11] }; 
            WalkingBackTexture2Ds = new List<Texture2D>() { textures[0], textures[1], textures[2] };
        }

        public void CleanPack()
        {
            
        }
    }
}