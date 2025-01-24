namespace MyMauiSamplesApp;

public class DeviceInfoService
{
    private readonly HashSet<string> notchedIPhones =
    [
        "iPhone10,3", "iPhone10,6", "iPhone11,2", "iPhone11,4", "iPhone11,6", "iPhone11,8",
        "iPhone12,1", "iPhone12,3", "iPhone12,5", "iPhone13,1", "iPhone13,2", "iPhone13,3",
        "iPhone13,4", "iPhone14,2", "iPhone14,3", "iPhone14,4", "iPhone14,5", "iPhone14,7",
        "iPhone14,8", "iPhone15,2", "iPhone15,3", "iPhone15,4", "iPhone15,5", "iPhone16,1", "iPhone16,2"
    ];

    private readonly HashSet<string> notchedIPads =
    [
        "iPad8,1", "iPad8,2", "iPad8,3", "iPad8,4", "iPad8,5", "iPad8,6", "iPad8,7", "iPad8,8",
        "iPad8,9", "iPad8,10", "iPad8,11", "iPad8,12", "iPad13,1", "iPad13,2", "iPad13,4", "iPad13,5",
        "iPad13,6", "iPad13,7", "iPad13,8", "iPad13,9", "iPad13,10", "iPad13,11", "iPad13,16",
        "iPad13,17", "iPad13,18", "iPad13,19", "iPad14,1", "iPad14,2", "iPad14,3", "iPad14,4",
        "iPad14,5", "iPad14,6"
    ];

    private readonly int assumeiPhoneAreNotchedFrom = 17;
    private readonly int assumeiPadAreNotchedFrom = 15;

    public static bool IsiOS => DeviceInfo.Current.Platform == DevicePlatform.iOS;
    public static bool IsAndroid => DeviceInfo.Current.Platform == DevicePlatform.Android;

    // Assume virtual devices are notched
    public static bool IsVirtual => DeviceInfo.Current.DeviceType switch
    {
        DeviceType.Physical => false,
        DeviceType.Virtual => true,
        _ => false
    };

    public bool IsNotchDevice()
    {
        string deviceModel = DeviceInfo.Current.Model;

        if (deviceModel.StartsWith("iPhone"))
        {
            int modelNumber = int.Parse(deviceModel[6..deviceModel.IndexOf(',')]);
            return notchedIPhones.Contains(deviceModel) || modelNumber >= assumeiPhoneAreNotchedFrom;
        }
        else if (deviceModel.StartsWith("iPad"))
        {
            int modelNumber = int.Parse(deviceModel[4..deviceModel.IndexOf(',')]);
            return notchedIPads.Contains(deviceModel) || modelNumber >= assumeiPadAreNotchedFrom;
        }

        return false;
    }

    public double GetTopNotchHeight(double scaleFactor)
    {
        return IsAndroid ? 20 : (IsiOS && (IsNotchDevice() || IsVirtual) ? 20 * scaleFactor : 0);
    }

    public double GetBottomNotchHeight(double scaleFactor)
    {
        return IsiOS ? (IsNotchDevice() || IsVirtual ? 16 * scaleFactor : 0) : 0;
    }
}