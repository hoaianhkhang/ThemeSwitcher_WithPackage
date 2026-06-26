using System.Collections;
using UnityEngine;

public class TileShort : MonoBehaviour
{
    [SerializeField] private float stopY = 0f;
    public ParticleSystem hitEffect;
    public SpriteRenderer spriteRenderer;

    private bool _falling;
    private Coroutine _routine;

    private void Awake() {}

    private void OnEnable() => _falling = true;

    private void OnDisable() { _falling = false; CancelReplay(); }

    private void Update()
    {
        if (!_falling) return;

        Vector3 pos = transform.position;
        pos.y -= TileConfig.FallSpeed * Time.deltaTime;

        if (pos.y <= stopY)
        {
            pos.y = stopY;
            transform.position = pos;
            Land();
        }
        else
        {
            transform.position = pos;
        }
    }

    [ContextMenu("Replay")]
    public void Replay()
    {
        CancelReplay();
        if (spriteRenderer != null) spriteRenderer.enabled = true;
        Vector3 pos = transform.position;
        pos.y = TileConfig.SpawnY;
        transform.position = pos;
        _falling = true;
    }

    private void Land()
    {
        _falling = false;
        if (spriteRenderer != null) spriteRenderer.enabled = false;
        if (hitEffect != null) hitEffect.Play();
        _routine = StartCoroutine(ReplayAfterDelay());
    }

    private IEnumerator ReplayAfterDelay()
    {
        yield return new WaitForSeconds(TileConfig.RespawnDelay);
        Replay();
    }

    private void CancelReplay()
    {
        if (_routine != null) { StopCoroutine(_routine); _routine = null; }
    }
}
