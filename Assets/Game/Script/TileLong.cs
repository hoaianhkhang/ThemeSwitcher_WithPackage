using System.Collections;
using UnityEngine;

public class TileLong : MonoBehaviour
{
    [SerializeField] private float tileLength = 16f;
    public SpriteRenderer fillRenderer;
    public GameObject bonusScore;

    private bool _replayScheduled;
    private Coroutine _routine;

    private void Awake() {}

    private void OnEnable()
    {
        ResetState();
        _replayScheduled = false;
    }

    private void OnDisable() => CancelRoutine();

    private void Update()
    {
        transform.position += Vector3.down * TileConfig.FallSpeed * Time.deltaTime;
        float y = transform.position.y;

        if (y <= 0f && y > -tileLength)
        {
            ApplyFill(Mathf.Clamp(-y, 0f, tileLength));

            if (!_replayScheduled)
            {
                _replayScheduled = true;
                _routine = StartCoroutine(ReplayAfterDelay());
            }
        }
        else if (y <= -tileLength)
        {
            ApplyFill(tileLength);
            if (bonusScore != null) bonusScore.SetActive(true);
        }
    }

    [ContextMenu("Replay")]
    public void Replay()
    {
        CancelRoutine();
        ResetState();
        _replayScheduled = false;
        Vector3 pos = transform.position;
        pos.y = TileConfig.SpawnY;
        transform.position = pos;
    }

    private void ResetState()
    {
        ApplyFill(0f);
        if (bonusScore != null) bonusScore.SetActive(false);
    }

    private void ApplyFill(float amount)
    {
        if (fillRenderer == null) return;
        Vector2 size = fillRenderer.size;
        size.y = amount;
        fillRenderer.size = size;
    }

    private IEnumerator ReplayAfterDelay()
    {
        yield return new WaitForSeconds(TileConfig.RespawnDelay);
        Replay();
    }

    private void CancelRoutine()
    {
        if (_routine != null) { StopCoroutine(_routine); _routine = null; }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (tileLength <= 0f) tileLength = 1f;
    }
#endif
}
