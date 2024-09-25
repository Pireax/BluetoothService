using Moq;

namespace BluetoothService.Tests
{
    public class BluetoothServiceTests
    {
        [Fact]
        public async void GetDevices_returns_devices()
        {
            var deviceId = "01:02:03:04:05:06";
            var device = new BluetoothDevice(
                "Test Device",
                deviceId,
                deviceId,
                false
            );
            var deviceWatcher = new FakeBluetoothDeviceWatcher([device]);
            var bluetoothInterface = new Mock<IBluetoothInterface>();
            var sut = new BluetoothService(deviceWatcher, bluetoothInterface.Object);

            var result = await sut.GetDevices();

            List<BluetoothDevice> expected = [device];
            Assert.Equivalent(expected, result);
        }

        [Fact]
        public async void UpdateConnectionStatus_can_connect()
        {
            var deviceId = "01:02:03:04:05:06";
            var device = new BluetoothDevice(
                "Test Device",
                deviceId,
                deviceId,
                false
            );
            var deviceWatcher = new FakeBluetoothDeviceWatcher([]);
            var bluetoothInterface = new Mock<IBluetoothInterface>();
            bluetoothInterface.Setup(x => x.Pair(deviceId)).Returns(Task.FromResult(true));
            var sut = new BluetoothService(deviceWatcher, bluetoothInterface.Object);

            var result = await sut.UpdateConnectionStatus(device, true);

            Assert.True(result);
            bluetoothInterface.Verify(m => m.Pair(deviceId), Times.Once());
            bluetoothInterface.VerifyNoOtherCalls();
        }

        [Fact]
        public async void UpdateConnectionStatus_can_disconnect()
        {
            var deviceId = "01:02:03:04:05:06";
            var device = new BluetoothDevice(
                "Test Device",
                deviceId,
                deviceId,
                true
            );
            var deviceWatcher = new FakeBluetoothDeviceWatcher([]);
            var bluetoothInterface = new Mock<IBluetoothInterface>();
            bluetoothInterface.Setup(x => x.Unpair(deviceId)).Returns(Task.FromResult(true));
            var sut = new BluetoothService(deviceWatcher, bluetoothInterface.Object);

            var result = await sut.UpdateConnectionStatus(device, false);

            Assert.True(result);
            bluetoothInterface.Verify(m => m.Unpair(deviceId), Times.Once());
            bluetoothInterface.VerifyNoOtherCalls();
        }
    }
}