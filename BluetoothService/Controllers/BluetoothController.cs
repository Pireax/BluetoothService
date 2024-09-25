using Microsoft.AspNetCore.Mvc;

namespace BluetoothService.Controllers;

[ApiController]
[Route("[controller]")]
public class BluetoothController : ControllerBase
{
    private readonly BluetoothService bluetoothService;

    public BluetoothController(BluetoothService bluetoothService)
    {
        this.bluetoothService = bluetoothService;
    }

    [HttpGet]
    public Task<IEnumerable<BluetoothDevice>> Get()
    {
        return bluetoothService.GetDevices();
    }

    [HttpPut]
    public async Task Put(BluetoothDevice device, bool connect)
    {
        await bluetoothService.UpdateConnectionStatus(device, connect);
    }
}
