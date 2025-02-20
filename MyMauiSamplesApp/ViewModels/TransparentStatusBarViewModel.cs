using CommunityToolkit.Mvvm.ComponentModel;

namespace MyMauiSamplesApp;

public partial class TransparentStatusBarViewModel : ObservableObject
{
    private float _hue, _saturation, _luminosity;
    private Color _color = Colors.Transparent;
    private readonly DeviceInfoService _deviceInfoService;
    private readonly double ContentHeight = 64;

    public TransparentStatusBarViewModel()
    {
        _deviceInfoService = new DeviceInfoService();
        UpdateNotchHeights();
    }

    public float Hue
    {
        get => _hue;
        set
        {
            if (_hue != value)
                Color = Color.FromHsla(value, _saturation, _luminosity);
        }
    }

    public float Saturation
    {
        get => _saturation;
        set
        {
            if (_saturation != value)
                Color = Color.FromHsla(_hue, value, _luminosity);
        }
    }

    public float Luminosity
    {
        get => _luminosity;
        set
        {
            if (_luminosity != value)
                Color = Color.FromHsla(_hue, _saturation, value);
        }
    }

    public Color Color
    {
        get => _color;
        set
        {
            if (_color != value)
            {
                _color = value;
                _hue = _color.GetHue();
                _saturation = _color.GetSaturation();
                _luminosity = _color.GetLuminosity();

                OnPropertyChanged(nameof(Hue));
                OnPropertyChanged(nameof(Saturation));
                OnPropertyChanged(nameof(Luminosity));
                OnPropertyChanged(); // reports this property
            }
        }
    }

    [ObservableProperty]
    private double topHeight = 64;

    [ObservableProperty]
    private double bottomHeight = 64;

    private void UpdateNotchHeights()
    {
        TopHeight = _deviceInfoService.GetTopNotchHeight() + ContentHeight;
        BottomHeight = _deviceInfoService.GetBottomNotchHeight() + ContentHeight;
    }
}
