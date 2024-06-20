using UnityEngine.UI;
using UnityEngine;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

public class GamePanel : MonoBehaviour
{
    [Header("Bullets")]
    [SerializeField] private Text _currentBulletsText;
    [SerializeField] private Text _totalBulletsText;

    [Header("Health")]
    [SerializeField] private Text _healthText;
    [SerializeField] private Slider _healthBar;

    [Header("Money")]
    [SerializeField] private Text _moneyText;

    [Header("Icons")]
    [SerializeField] private Image[] _images;

    [SerializeField] private GameObject _speedometr;

    private EventBus _eventBus;
    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<SetCurrentBullets>(SetCurrentBulletsText, 1);
        _eventBus.Subscribe<SetTotalBullets>(SetTotalBulletsText, 1);
        _eventBus.Subscribe<UpdateCurrentBullets>(UpdateCurrentBulletsText, 1);
        _eventBus.Subscribe<UpdateTotalBullets>(UpdateTotalBulletsText, 1);
        _eventBus.Subscribe<UpdateHealth>(UpdateHealthText, 1);
        _eventBus.Subscribe<UpdateMoney>(UpdateMoneyText, 1);
        _eventBus.Subscribe<SetImage>(SetImage, 1);
        _eventBus.Subscribe<SetSpeedometer>(SetSpeedometer, 1);

        foreach (Image image in _images)
        {
            image.enabled = false;
        }
    }

    private void UpdateCurrentBulletsText(UpdateCurrentBullets currentBullets)
    {
        _currentBulletsText.text = currentBullets.Amount.ToString() + " /";
    }

    private void UpdateTotalBulletsText(UpdateTotalBullets totalBullets)
    {
        _totalBulletsText.text = totalBullets.Amount.ToString();
    }

    private void UpdateHealthText(UpdateHealth health)
    {
        _healthText.text = health.Amount.ToString();
        _healthBar.value = health.Amount;
    }

    private void UpdateMoneyText(UpdateMoney updateMoney)
    {
        _moneyText.text = updateMoney.Amount.ToString();
    }

    private void SetCurrentBulletsText(SetCurrentBullets state)
    {
        _currentBulletsText.enabled = state.State;
    }

    private void SetTotalBulletsText(SetTotalBullets state)
    {
        _totalBulletsText.enabled = state.State;
    }

    private void SetImage(SetImage images)
    {
        foreach (Image image in _images)
        {
            image.enabled = false;
        }

        if(images.Active == true)
        {
            _images[images.Index].enabled = true;
        }
    }

    private void SetSpeedometer(SetSpeedometer speedometer)
    {
        _speedometr.SetActive(speedometer.State);
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<SetCurrentBullets>(SetCurrentBulletsText);
        _eventBus.Unsubscribe<SetTotalBullets>(SetTotalBulletsText);
        _eventBus.Unsubscribe<UpdateCurrentBullets>(UpdateCurrentBulletsText);
        _eventBus.Unsubscribe<UpdateTotalBullets>(UpdateTotalBulletsText);
        _eventBus.Unsubscribe<UpdateHealth>(UpdateHealthText);
        _eventBus.Unsubscribe<UpdateMoney>(UpdateMoneyText);
        _eventBus.Unsubscribe<SetImage>(SetImage);
        _eventBus.Unsubscribe<SetSpeedometer>(SetSpeedometer);
    }
}
