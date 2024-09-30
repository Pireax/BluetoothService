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
    public async Task<IActionResult> Put(BluetoothDevice device, bool connect)
    {
        var result = await bluetoothService.UpdateConnectionStatus(device, connect);
        if (!result) return BadRequest(new { Message = "Failed to update connection status for device." });
        return Ok();
    }
}
