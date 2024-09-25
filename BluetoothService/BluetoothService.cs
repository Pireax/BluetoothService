namespace BluetoothService;

public class BluetoothService(IBluetoothDeviceWatcher bluetoothDeviceWatcher, IBluetoothInterface bluetoothInterface)
{
    private readonly IBluetoothDeviceWatcher bluetoothDeviceWatcher = bluetoothDeviceWatcher;
    private readonly IBluetoothInterface bluetoothInterface = bluetoothInterface;

    public async Task<IEnumerable<BluetoothDevice>> GetDevices()
    {
        await bluetoothDeviceWatcher.InitializationTask;
        return bluetoothDeviceWatcher.Devices;
    }

    /// <summary>
    /// Pairs (and connects) or unpairs the given device
    /// </summary>
    /// <returns>true if successful, false otherwise</returns>
    public Task<bool> UpdateConnectionStatus(BluetoothDevice device, bool connect)
    {
        if (connect)
        {
            return bluetoothInterface.Pair(device.Id);
        }
        else
        {
            return bluetoothInterface.Unpair(device.Id);
        }
    }
}
