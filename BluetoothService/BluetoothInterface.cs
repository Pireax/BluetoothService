using Windows.Devices.Enumeration;

namespace BluetoothService
{
    /// <summary>
    /// Wrapper class around the Bluetooth device API calls.
    /// </summary>
    public interface IBluetoothInterface
    {
        public Task<bool> Pair(string deviceId);
        public Task<bool> Unpair(string deviceId);
    }

    public class WindowsBluetoothInterface : IBluetoothInterface
    {
        public async Task<bool> Pair(string deviceId)
        {
            var info = await DeviceInformation.CreateFromIdAsync(deviceId);

            var pairing = info.Pairing.Custom;
            pairing.PairingRequested += Pairing_PairingRequested;
            var result = await pairing.PairAsync(DevicePairingKinds.ConfirmOnly);
            pairing.PairingRequested -= Pairing_PairingRequested;

            return result.Status == DevicePairingResultStatus.Paired;
        }

        public async Task<bool> Unpair(string deviceId)
        {
            var info = await DeviceInformation.CreateFromIdAsync(deviceId);
            var result = await info.Pairing.UnpairAsync();
            return result.Status == DeviceUnpairingResultStatus.Unpaired;
        }

        private void Pairing_PairingRequested(DeviceInformationCustomPairing sender, DevicePairingRequestedEventArgs args)
        {
            args.Accept();
        }
    }
}
