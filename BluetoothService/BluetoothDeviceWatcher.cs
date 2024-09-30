using System.Collections.Concurrent;
using Windows.Devices.Enumeration;

namespace BluetoothService
{
    public interface IBluetoothDeviceWatcher
    {
        IEnumerable<BluetoothDevice> Devices { get; }
        /// <summary>
        /// Finishes when the watcher has finishied initialization.
        /// </summary>
        Task InitializationTask { get; }
    }

    public class BluetoothDeviceWatcher : IBluetoothDeviceWatcher
    {
        const string aqsAllBluetoothDevices = "(System.Devices.Aep.ProtocolId:=\"{e0cbf06c-cd8b-4647-bb8a-263b43f0f974}\")";
        const string isConnectedProperty = "System.Devices.Aep.IsConnected";

        private readonly DeviceWatcher watcher;
        private readonly ConcurrentDictionary<string, BluetoothDevice> devices = [];
        private TaskCompletionSource watcherInitialized = new();

        public BluetoothDeviceWatcher()
        {
            watcher = DeviceInformation.CreateWatcher(aqsAllBluetoothDevices, [
                "System.Devices.Aep.DeviceAddress",
                isConnectedProperty
                ], DeviceInformationKind.AssociationEndpoint);

            watcher.Added += (s, e) => devices[e.Id] = BluetoothDeviceFromDeviceInformation(e);
            watcher.Updated += (s, e) =>
            {
                if (!e.Properties.ContainsKey(isConnectedProperty)) return;
                devices[e.Id] = devices[e.Id] with
                {
                    IsConnected = (bool)e.Properties[isConnectedProperty]
                };
            };
            watcher.Removed += (s, e) => devices.Remove(e.Id, out _);
            watcher.EnumerationCompleted += (s, e) => watcherInitialized.SetResult();

            watcher.Start();
        }

        public IEnumerable<BluetoothDevice> Devices => devices.Values;

        public Task InitializationTask => watcherInitialized.Task;

        private static BluetoothDevice BluetoothDeviceFromDeviceInformation(DeviceInformation x) => new(
            x.Name,
            x.Id,
            (string)x.Properties["System.Devices.Aep.DeviceAddress"],
            (bool)x.Properties["System.Devices.Aep.IsConnected"]
        );
    }
}
