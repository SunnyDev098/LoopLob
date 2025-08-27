using UnityEngine;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;

[BurstCompile]
public class BurstColorTransition : MonoBehaviour
{
    [SerializeField] private float _duration = 5f;
    [SerializeField] private Renderer _renderer;

    // Callback events
    public event System.Action<Color> OnColorChanged;
    public event System.Action OnTransitionComplete;

    private MaterialPropertyBlock _propBlock;
    private ColorTransitionJob _job;
    private JobHandle _jobHandle;
    private NativeArray<Color> _resultColor;
    private NativeArray<bool> _callbackFlags; // [0]=colorChanged, [1]=transitionComplete
    private bool _isTransitioning = false;

    private void Start() => Initialize();

    private void Initialize()
    {
        if (_renderer == null) _renderer = GetComponent<Renderer>();

        _propBlock = new MaterialPropertyBlock();
        _resultColor = new NativeArray<Color>(1, Allocator.Persistent);
        _callbackFlags = new NativeArray<bool>(2, Allocator.Persistent);
        ResetColor();
    }

    public void StartTransition()
    {
        if (!_isTransitioning)
        {
            _job = new ColorTransitionJob
            {
                StartTime = Time.time,
                Duration = _duration,
                ResultColor = _resultColor,
                CallbackFlags = _callbackFlags,
                LastProgress = 0f
            };
            _isTransitioning = true;
        }
    }

    public void ResetColor()
    {
        _jobHandle.Complete();
        _isTransitioning = false;
        _resultColor[0] = Color.white;
        ApplyCurrentColor();
    }

    private void Update()
    {
        if (!_isTransitioning) return;

        if (_jobHandle.IsCompleted)
        {
            _jobHandle.Complete();

            // Process callbacks
        
            if (_callbackFlags[1]) // Transition complete
            {
                OnTransitionComplete?.Invoke();
                _callbackFlags[1] = false;
                _isTransitioning = false;
                return;
            }

            ApplyCurrentColor();

            // Update job for next frame
            _job.CurrentTime = Time.time;
            _jobHandle = _job.Schedule();
        }
    }

    private void ApplyCurrentColor()
    {
        _renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_Color", _resultColor[0]);
        _renderer.SetPropertyBlock(_propBlock);
    }

    private void OnDestroy()
    {
        _jobHandle.Complete();
        if (_resultColor.IsCreated) _resultColor.Dispose();
        if (_callbackFlags.IsCreated) _callbackFlags.Dispose();
    }

    [BurstCompile]
    private struct ColorTransitionJob : IJob
    {
        public float CurrentTime;
        public float StartTime;
        public float Duration;
        public float LastProgress;
        public NativeArray<Color> ResultColor;
        public NativeArray<bool> CallbackFlags;

        public void Execute()
        {
            float progress = Mathf.Clamp01((CurrentTime - StartTime) / Duration);

            // Color calculation
            if (progress < 0.33f)
            {
                ResultColor[0] = Color.Lerp(Color.white, Color.yellow, progress / 0.33f);
            }
            else if (progress < 0.66f)
            {
                ResultColor[0] = Color.Lerp(Color.yellow, new Color(1f, 0.5f, 0f), (progress - 0.33f) / 0.33f);
            }
            else
            {
                ResultColor[0] = Color.Lerp(new Color(1f, 0.5f, 0f), Color.red, (progress - 0.66f) / 0.34f);
            }

            // Trigger callbacks
            if (Mathf.Abs(progress - LastProgress) > 0.05f) // Threshold for color change
            {
                CallbackFlags[0] = true;
            }

            if (progress >= 0.99f)
            {
                CallbackFlags[1] = true;
            }

            LastProgress = progress;
        }
    }
}



