using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XCore.Graphics
{
    public class ParallaxBackgroundController : MonoBehaviour
    {
        public Camera Camera
        {
            get 
            {  
                if (_camera == null)
                {
                    _camera = Camera.main;
                    _lastCameraPosition = _camera.transform.position;
                    return _camera;
                }
                return _camera;
            }
            set { _camera = value; }
        }
        [SerializeField]
        private Camera _camera;

        private List<ParallaxBackgroundLayer> _parallaxBackgroundLayers;
        private Vector3 _lastCameraPosition = Vector3.zero;

        private void Awake()
        {
            _parallaxBackgroundLayers = new List<ParallaxBackgroundLayer>();
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject gm = transform.GetChild(i).gameObject;
                ParallaxBackgroundLayer parallaxBackgroundLayer = gm.GetComponent<ParallaxBackgroundLayer>();
                if (parallaxBackgroundLayer != null)
                {
                    _parallaxBackgroundLayers.Add(parallaxBackgroundLayer);
                }
            }
        }

        private void LateUpdate()
        {
            Vector3 cameraPosition = Camera.transform.position;
            Vector3 deltaMovement = cameraPosition - _lastCameraPosition;
            _lastCameraPosition = cameraPosition;

            for(int i = 0; i < _parallaxBackgroundLayers.Count; i++)
            {
                ParallaxBackgroundLayer layer = _parallaxBackgroundLayers[i];

                layer.transform.position += new Vector3(deltaMovement.x * layer.DeltaSpeed.x, deltaMovement.y * layer.DeltaSpeed.y);

                if (layer.HorizontalMovement && Mathf.Abs(cameraPosition.x - layer.transform.position.x) >= layer.TextureUnitSize.x)
                {
                    float offsetPositionX = (cameraPosition.x - layer.transform.position.x) % layer.TextureUnitSize.x;
                    layer.transform.position = new Vector3(cameraPosition.x + offsetPositionX, layer.transform.position.y, layer.transform.position.z);
                }

                if (layer.VerticalMovement && Mathf.Abs(cameraPosition.y - layer.transform.position.y) >= layer.TextureUnitSize.y)
                {
                    float offsetPositionY = (cameraPosition.y - layer.transform.position.y) % layer.TextureUnitSize.y;
                    layer.transform.position = new Vector3(layer.transform.position.x, cameraPosition.y + offsetPositionY, layer.transform.position.z);
                }
            }
        }
    }
}
