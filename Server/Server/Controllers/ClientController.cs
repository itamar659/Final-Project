using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Dto;
using Server.Infrastructure;
using Server.Models;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientController : Controller
{
    private IClientRepository _clientRepo;
    private IRoomRepository _roomRepo;
    private RoomHubManager _hub;

    public ClientController(IClientRepository clientRepo, IRoomRepository roomRepo, RoomHubManager hub)
    {
        _clientRepo = clientRepo;
        _roomRepo = roomRepo;
        _hub = hub;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Get([Bind("Token")] JukeboxClient jukeboxClient)
    {
        var client = await _clientRepo.GetClientByTokenAsync(jukeboxClient.Token);

        if (client is null)
            return NotFound();

        return Ok(client);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login([Bind("Username")] JukeboxClient jukeboxClient)
    {
        // Also create if doesn't exists
        if (jukeboxClient.Username == string.Empty)
            return NotFound();

        var client = await _clientRepo.GetClientByUsernameAsync(jukeboxClient.Username, false);
        if (client is null)
        {
            // CREATE if email doesn't exists
            client = await _clientRepo.CreateAsync(jukeboxClient with { IsAnonymous = false });
            await _clientRepo.SaveAsync();
        }

        return Ok(client);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> AnonymousLogin([Bind("Username")] JukeboxClient jukeboxClient)
    {
        var newClient = await _clientRepo.CreateAsync(jukeboxClient);
        await _clientRepo.SaveAsync();

        return Ok(newClient);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> JoinRoom([Bind("Token,RoomId,PinCode")] JukeboxClientJoinRoomDto jukeboxClient)
    {
        var client = await _clientRepo.GetClientByTokenAsync(jukeboxClient.Token);
        if (client is null)
            return NotFound(false);

        var room = await _roomRepo.GetRoomByIdAsync(jukeboxClient.RoomId);
        if (room is null)
            return NotFound(false);

        if (jukeboxClient.PinCode != room.PinCode)
            return Unauthorized(false);

        try
        {
            if (client.RoomId != NumberGenerator.EmptyId)
            {
                //if (await _roomRepo.GetRoomByIdAsync(client.RoomId) != null)
                //    await _hub.LeaveRoom(client.RoomId);
                await leaveRoomAsync(client); // TODO: doesn't work? LOL

                // get the updated classes after leaving the room
                client = await _clientRepo.GetClientByTokenAsync(jukeboxClient.Token);
                room = await _roomRepo.GetRoomByIdAsync(jukeboxClient.RoomId);
            }


            if (room is null || client is null)
                return NotFound(false);

            await joinRoomAsync(client, room);

            //await _hub.JoinRoom(client.RoomId);

            return Ok(true);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await _clientRepo.GetClientByTokenAsync(jukeboxClient.Token) == null)
                return NotFound(false);

            if (await _roomRepo.GetRoomByIdAsync(jukeboxClient.RoomId) == null)
                return NotFound(false);

            throw;
        }
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> LeaveRoom([Bind("Token,ConnectionId")] JukeboxClientJoinRoomDto jukeboxClient)
    {
        var client = await _clientRepo.GetClientByTokenAsync(jukeboxClient.Token);
        if (client is null)
            return NotFound();

        if (client.RoomId == NumberGenerator.EmptyId)
            return Unauthorized();

        try
        {
            await leaveRoomAsync(client);
            return Ok();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await _clientRepo.GetClientByTokenAsync(jukeboxClient.Token) == null)
                return NotFound();

            throw;
        }
    }

    private async Task joinRoomAsync(JukeboxClient client, JukeboxRoom room)
    {
        // create updated records
        var newRoom = room with
        {
            OnlineUsers = room.OnlineUsers + 1
        };

        var newClient = client with
        {
            RoomId = newRoom.RoomId
        };

        // update and save in database
        _roomRepo.Update(room, newRoom);
        await _roomRepo.SaveAsync();

        _clientRepo.Update(client, newClient);
        await _clientRepo.SaveAsync();
    }

    private async Task leaveRoomAsync(JukeboxClient client)
    {
        if (client.RoomId == NumberGenerator.EmptyId)
            return;

        // get current room
        var room = await _roomRepo.GetRoomByIdAsync(client.RoomId);
        if (room is not null)
        {
            // leave current room if it still exists
            // create updated records
            var newRoom = room with
            {
                OnlineUsers = room.OnlineUsers - 1,
            };

            // update and save in database
            _roomRepo.Update(room, newRoom);
            await _roomRepo.SaveAsync();
        }

        // create updated records
        var newClient = client with
        {
            RoomId = NumberGenerator.EmptyId
        };

        // update and save in database
        _clientRepo.Update(client, newClient);
        await _clientRepo.SaveAsync();
    }






    [HttpPost("admin_only/[action]")]
    public async Task<IEnumerable<JukeboxClient>> All()
    {
        return await _clientRepo.GetAllClients();
    }

    [HttpPost("admin_only/[action]")]
    public async Task<ActionResult<JukeboxClient>> Create([Bind("Username")] JukeboxClient jukeboxClient)
    {
        if (jukeboxClient is null)
            return BadRequest();

        if (await _clientRepo.GetClientByUsernameAsync(jukeboxClient.Username, true) != null)
            return Unauthorized();

        var newClient = await _clientRepo.CreateAsync(jukeboxClient);
        await _clientRepo.SaveAsync();

        return CreatedAtAction(nameof(Get), new { Token = newClient?.Token }, newClient);
    }

    [HttpPost("admin_only/[action]")]
    public async Task<ActionResult> Delete([Bind("Username,IsAnonymous")] JukeboxClient jukeboxClient)
    {
        var client = await _clientRepo.GetClientByUsernameAsync(jukeboxClient.Username, jukeboxClient.IsAnonymous);
        if (client == null)
            return NotFound();

        _clientRepo.Remove(client);
        await _clientRepo.SaveAsync();

        return NoContent();
    }
}
