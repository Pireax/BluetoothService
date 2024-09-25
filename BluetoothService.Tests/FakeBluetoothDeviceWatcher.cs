namespace BluetoothService.Tests
{
    internal class FakeBluetoothDeviceWatcher : IBluetoothDeviceWatcher
    {
        private readonly List<BluetoothDevice> devices;

        public FakeBluetoothDeviceWatcher(IEnumerable<BluetoothDevice> devices)
        {
            this.devices = devices.ToList();
        }

        public IEnumerable<BluetoothDevice> Devices => devices;

        public Task InitializationTask => Task.CompletedTask;
    }
}
