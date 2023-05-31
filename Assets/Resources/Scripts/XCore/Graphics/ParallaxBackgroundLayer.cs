using System.Collections;
using UnityEngine;

namespace XCore.Graphics
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ParallaxBackgroundLayer : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer { get; private set; }

        public Sprite Sprite { get; private set; }

        public Vector2 DeltaSpeed = new Vector2(1, 1);
        public bool HorizontalMovement, VerticalMovement;

        public Vector2 TextureUnitSize { get; private set; }

        private void Awake()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
            Sprite = SpriteRenderer.sprite;
            TextureUnitSize = new Vector2(Sprite.texture.width / Sprite.pixelsPerUnit, Sprite.texture.height/ Sprite.pixelsPerUnit);
        }
    }
}

