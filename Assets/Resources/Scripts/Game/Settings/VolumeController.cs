using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using XCore;

namespace PawnsAndGuns.Settings
{
    public class VolumeController : MonoBehaviour
    {
        public static VolumeController Instance { get; private set; }

        private Volume _volume;
        private VolumeProfile _profile;

        private void Awake()
        {
            Instance = this;
            _volume = GetComponent<Volume>();
            _profile = _volume.profile;
        }

        public T GetVolumeComponent<T>() where T : VolumeComponent
        {
            _profile.TryGet(out T component);
            return component;
        }

        public void ToggleComponent<T>(bool value) where T : VolumeComponent
        {
            if (_profile.TryGet(out T component))
            {
                component.active = value;
            }
        }
        private void OnDestroy()
        {
            Instance = null;
        }
    }
}