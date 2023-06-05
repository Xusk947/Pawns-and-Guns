using DG.Tweening;
using PawnsAndGuns.Settings;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XCore;
using XCore.Extensions;

namespace PawnsAndGuns.Game.Levels
{
    public class MainMenu : MonoBehaviour
    {
        private Vector3 _movementSpeed;
        private FollowCamera _camera;

        private Transform _buttonLayout;
        private Button _button_start, _button_settings, _button_exit;

        private Transform _settingsLayout, _elementsLayout, _audioLayout, _graphicsLayout;
        private Button _button_audio, _button_graphics, _button_return;
        public Toggle Bloom, Vignette, LensDistortion;

        private void Awake()
        {
            GameSettings.Load();
            Content.Load();
            GetButtons();
        }

        private void Start()
        {
            SetupCamera();
            SetupSettings();
        }

        private void SetupSettings()
        {
            Bloom.isOn = GameSettings.GetBool("bloom");
            LensDistortion.isOn = GameSettings.GetBool("lensDistortion");
            Vignette.isOn = GameSettings.GetBool("vignette");

            print(GameSettings.GetBool("bloom"));
            print(VolumeController.Instance);
            VolumeController.Instance?.ToggleComponent<Bloom>(Bloom.isOn);
            VolumeController.Instance?.ToggleComponent<LensDistortion>(LensDistortion.isOn);
            VolumeController.Instance?.ToggleComponent<Vignette>(Vignette.isOn);
        }

        private void SetupCamera()
        {
            _camera = FollowCamera.Instance;
            _camera.transform.position = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), _camera.transform.position.z);
            _movementSpeed = Vector2Extensions.Random(.1f);
        }

        private void GetButtons()
        {
            _buttonLayout = transform.Find("ButtonsLayout");

            _button_start = _buttonLayout.transform.Find("Play").GetComponent<Button>();
            _button_start.onClick.AddListener(OnButtonStartClick);

            _button_settings = _buttonLayout.transform.Find("Settings").GetComponent<Button>();
            _button_settings.onClick.AddListener(OnButtonSettingsClick);

            _button_exit = _buttonLayout.transform.Find("Exit").GetComponent<Button>();
            _button_exit.onClick.AddListener(OnButtonExitClick);

            _settingsLayout = transform.Find("SettingsLayout");

            _elementsLayout = _settingsLayout.Find("ElementsLayout");
            _elementsLayout.gameObject.SetActive(false);

            _audioLayout = _elementsLayout.Find("Audio");

            _graphicsLayout = _elementsLayout.Find("Graphics");
            Bloom.onValueChanged.AddListener(OnToggleBloom);
            Vignette.onValueChanged.AddListener(OnToggleVignette);
            LensDistortion.onValueChanged.AddListener(OnToggleLensDistortion);

            Transform settingsButtons = _settingsLayout.Find("Buttons");

            _button_audio = settingsButtons.Find("Audio").GetComponent<Button>();
            _button_audio.onClick.AddListener(OnButtonAudioClick);

            _button_graphics = settingsButtons.Find("Graphics").GetComponent<Button>();
            _button_graphics.onClick.AddListener(OnButtonGraphicsClick);

            _button_return = settingsButtons.Find("Return").GetComponent<Button>();
            _button_return.onClick.AddListener(OnButtonReturnClick);

            ShowMainMenu();
        }

        private void OnButtonStartClick()
        {
            if (GameSettings.GetBool("tutorialFinished"))
            {
                SceneManager.LoadScene(2);
            }
            else
            {
                SceneManager.LoadScene(1);
            }
        }

        private void OnButtonSettingsClick()
        {
            ShowSettingsMenu();
        }

        private void OnButtonAudioClick()
        {
            _elementsLayout.gameObject.SetActive(true);
            _graphicsLayout.gameObject.SetActive(false);
            _audioLayout.gameObject.SetActive(true);
        }

        private void OnButtonGraphicsClick()
        {
            _elementsLayout.gameObject.SetActive(true);
            _audioLayout.gameObject.SetActive(false);
            _graphicsLayout.gameObject.SetActive(true);
        }

        private void OnToggleBloom(bool value)
        {
            GameSettings.SetBool("bloom", value);
            VolumeController.Instance?.ToggleComponent<Bloom>(value);
        }

        private void OnToggleVignette(bool value)
        {
            GameSettings.SetBool("vignette", value);
            VolumeController.Instance?.ToggleComponent<Vignette>(value);
        }

        private void OnToggleLensDistortion(bool value) {
            GameSettings.SetBool("lensDistortion", value);
            VolumeController.Instance?.ToggleComponent<LensDistortion>(value);
        }

        private void OnButtonReturnClick()
        {
            ShowMainMenu();
        }

        private void ShowSettingsMenu()
        {
            _buttonLayout.gameObject.SetActive(false);

            _settingsLayout.gameObject.SetActive(true);

            var sequence = DOTween.Sequence();

            Transform buttons = _settingsLayout.Find("Buttons");
            _elementsLayout.gameObject.SetActive(false);

            for (int i = 0; i < buttons.childCount; i++)
            {
                Transform child = buttons.GetChild(i);
                child.localScale = new Vector3(1, 0, 1);
                sequence.Append(child.DOScale(new Vector3(1, 1, 1), .1f).SetEase(Ease.InCubic));
            }

            sequence.Play();
        }

        private void ShowMainMenu()
        {
            _settingsLayout.gameObject.SetActive(false);

            _buttonLayout.gameObject.SetActive(true);
            var sequence = DOTween.Sequence();

            for (int i = 0; i < _buttonLayout.childCount; i++)
            {
                Transform child = _buttonLayout.GetChild(i);
                child.localScale = new Vector3(1, 0, 1);
                sequence.Append(child.DOScale(new Vector3(1, 1, 1), .1f).SetEase(Ease.InCubic));
            }

            sequence.Play();
        }

        private void OnButtonExitClick()
        {
            Application.Quit();
        }

        private void LateUpdate()
        {
            if (_camera != null)
            {
                _camera.transform.position += _movementSpeed * Time.deltaTime;
            } else
            {
                _camera = FollowCamera.Instance;
            }
        }

        private void OnApplicationQuit()
        {
            GameSettings.Save();
        }
    }
}