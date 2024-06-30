using Microsoft.AspNetCore.Mvc;
using ScheduleManagementApi.models;
using ScheduleManagementApi.models.DTO;

namespace ScheduleManagementApi.controllers;


[ApiController]
[Route("api/[controller]")]
public class ConsumersController(ConsumerConfigurationService consumersService) : ControllerBase
{
    [HttpGet]
    public async Task<List<ConsumerConfiguration>> Get() =>
        await consumersService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<ConsumerConfiguration>> Get(string id)
    {
        var consumer = await consumersService.GetAsync(id);

        if (consumer is null)
        {
            return NotFound();
        }

        return consumer;
    }

    [HttpPost("wallbox")]
    public async Task<IActionResult> PostWallbox([FromBody] WallboxConfigurationInputDto inputDto)
    {
        // Validate input if necessary

        var wallboxConfig = new WallboxConfiguration
        {
            DeviceName = inputDto.DeviceName,
            // Set default values or leave other properties as defaults/null
        };

        await consumersService.CreateWallboxAsync(wallboxConfig);

        return CreatedAtAction(nameof(Get), new { id = wallboxConfig.Id }, wallboxConfig);
    }
    
    [HttpPost("{id}/schedule")]
    public async Task<IActionResult> AddSchedule(string id, [FromBody] ScheduleDto scheduleDto)
    {
        var existingWallboxConfig = await consumersService.GetAsync(id);

        if (existingWallboxConfig == null)
        {
            return NotFound();
        }

        var newScheduleEntry = new ScheduleEntry
        {
            StartTime = scheduleDto.StartTime,
            EndTime = scheduleDto.EndTime,
            Mode = scheduleDto.Mode,
            MinValue = scheduleDto.MinValue,
            MaxValue = scheduleDto.MaxValue,
            Priority = scheduleDto.Priority ?? Priority.Default
        };

        existingWallboxConfig.Schedule.Add(newScheduleEntry);

        await consumersService.UpdateAsync(id, existingWallboxConfig);

        return Ok(existingWallboxConfig);
    }
    
    [HttpPost("heatpump")]
    public async Task<IActionResult> Post(HeatpumpConfiguration heatpumpConfiguration)
    {
        await consumersService.CreateHeatpumpAsync(heatpumpConfiguration);

        return CreatedAtAction(nameof(Get), new { id = heatpumpConfiguration.Id }, heatpumpConfiguration);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, ConsumerConfiguration updatedConsumer)
    {
        var consumer = await consumersService.GetAsync(id);

        if (consumer is null)
        {
            return NotFound();
        }

        updatedConsumer.Id = consumer.Id;

        await consumersService.UpdateAsync(id, updatedConsumer);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var consumer = await consumersService.GetAsync(id);

        if (consumer is null)
        {
            return NotFound();
        }

        await consumersService.RemoveAsync(id);

        return NoContent();
    }
}