using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BGMood : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float swapInterval = 2f;
    [SerializeField] private bool playOnAwake = true;
    [SerializeField] private bool loop = true;

    private SpriteRenderer _renderer;
    private int _currentIndex;
    private Coroutine _routine;

    private void Awake() => _renderer = GetComponent<SpriteRenderer>();

    private void Start() { if (playOnAwake) Begin(); }

    private void OnDisable() => Halt();

    public void Begin()
    {
        Halt();
        if (sprites == null || sprites.Length == 0 || swapInterval <= 0f) return;
        _routine = StartCoroutine(Cycle());
    }

    public void Halt()
    {
        if (_routine != null) { StopCoroutine(_routine); _routine = null; }
    }

    public void ResetAndPlay() { _currentIndex = 0; Show(); Begin(); }

    public bool HasSprites => sprites != null && sprites.Length > 0;

    public void SetSpriteIndex(int index)
    {
        if (sprites == null || sprites.Length == 0) return;
        _currentIndex = Mathf.Clamp(index, 0, sprites.Length - 1);
        Show();
    }

    private IEnumerator Cycle()
    {
        Show();
        while (true)
        {
            yield return new WaitForSeconds(swapInterval);
            _currentIndex++;
            if (_currentIndex >= sprites.Length)
            {
                if (!loop) yield break;
                _currentIndex = 0;
            }
            Show();
        }
    }

    private void Show()
    {
        if (_renderer == null || sprites == null || _currentIndex >= sprites.Length) return;
        _renderer.sprite = sprites[_currentIndex];
    }


    public void ApplyMoodSprites(Sprite[] themeSprites)
    {
        if (themeSprites == null || themeSprites.Length == 0)
            return;
        sprites = (Sprite[])themeSprites.Clone();        

        for (int i = 0; i < themeSprites.Length; i++)
        {
            sprites[i] = themeSprites[i];
        }

        _currentIndex = 0;
        Begin();
    }

#if UNITY_EDITOR
    private void OnValidate() { if (swapInterval <= 0f) swapInterval = 0.1f; }
#endif
}
