using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class StarfieldBackground : MonoBehaviour
{
    [Header("Pixel resolution (the smaller the number the faster)")]
    [SerializeField] private int textureWidth = 320;
    [SerializeField] private int textureHeight = 180;

    [Header("Stars")]
    [SerializeField] private int starCount = 220;

    [Tooltip("Chance a star is big")]
    [Range(0f, 1f)]
    [SerializeField] private float bigStarChance = 0.08f;

    [SerializeField] private int bigStarSize = 4;

    [Tooltip("Pixels per second")]
    [SerializeField] private float minSpeed = 20f;
    [SerializeField] private float maxSpeed = 80f;

    [Header("twinkle")]
    [SerializeField] private bool twinkle = false;
    [SerializeField] private float twinkleRate = 8f;

    private struct Star
    {
        public float x;
        public float y;
        public float speed;
        public int size;
        public byte brightness;  // try to keep it in the 180 to the 255 range. looks nice
        public float twPhase;
    }

    private RawImage _raw;
    private Texture2D _tex;
    private Color32[] _pixels;
    private Star[] _stars;

    private static readonly Color32 Black = new Color32(0, 0, 0, 255);

    private void Awake()
    {
        _raw = GetComponent<RawImage>();

        // this keeps the stars from interacting with things
        _raw.raycastTarget = false;

        CreateTexture();
        InitStars();
    }

    private void OnDestroy()
    {
        if (_tex != null)
        {
            Destroy(_tex);
        }
    }

    private void CreateTexture()
    {
        textureWidth = Mathf.Max(16, textureWidth);
        textureHeight = Mathf.Max(16, textureHeight);

        _tex = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        _tex.filterMode = FilterMode.Point;   // retro low res
        _tex.wrapMode = TextureWrapMode.Clamp;

        _pixels = new Color32[textureWidth * textureHeight];

        _raw.texture = _tex;

        /* keep the Canvas scaler resolution proportional to this texture resolution
           320 x 180 seems to work the best without creating weird glitches with the canvas
           At some point this needs to be fixed, especially for using this effect outside of the menu scene */
    }

    private void InitStars()
    {
        _stars = new Star[starCount];

        for (int i = 0; i < _stars.Length; i++)
        {
            _stars[i] = NewStar(randomY: true);
        }
    }

    private Star NewStar(bool randomY)
    {
        Star s = new Star();
        s.size = (UnityEngine.Random.value < bigStarChance) ? bigStarSize : 1;
        s.x = UnityEngine.Random.Range(0, textureWidth);
        s.y = randomY ? UnityEngine.Random.Range(0, textureHeight) : (textureHeight + UnityEngine.Random.Range(0f, textureHeight * 0.25f));
        s.speed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        s.brightness = (byte)UnityEngine.Random.Range(200, 256);
        s.twPhase = UnityEngine.Random.Range(0f, 1000f);
        return s;
    }

    private void Update()
    {
        float dt = Time.unscaledDeltaTime;

        // Move stars to the bottom
        for (int i = 0; i < _stars.Length; i++)
        {
            Star s = _stars[i];
            s.y -= s.speed * dt;

            // back to top once it leaves the bottom
            if (s.y < -s.size)
            {
                s = NewStar(randomY: false);
            }

            _stars[i] = s;
        }

        Render();
    }

    private void Render()
    {
        Array.Fill(_pixels, Black);

        for (int i = 0; i < _stars.Length; i++)
        {
            Star s = _stars[i];

            byte b = s.brightness;
            if (twinkle && s.size == 1)
            {
                float t = (Mathf.Sin((Time.unscaledTime + s.twPhase) * twinkleRate) + 1f) * 0.5f;
                b = (byte)Mathf.Clamp(Mathf.Lerp(160f, s.brightness, t), 0f, 255f);
            }

            Color32 c = new Color32(b, b, b, 255);
            DrawSquare((int)s.x, (int)s.y, s.size, c);
        }

        _tex.SetPixels32(_pixels);
        _tex.Apply(false);
    }

    private void DrawSquare(int x, int y, int size, Color32 color)
    {
        for (int dy = 0; dy < size; dy++)
        {
            int py = y + dy;
            if (py < 0 || py >= textureHeight) continue;

            int row = py * textureWidth;

            for (int dx = 0; dx < size; dx++)
            {
                int px = x + dx;
                if (px < 0 || px >= textureWidth) continue;

                _pixels[row + px] = color;
            }
        }
    }
}
