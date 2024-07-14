using Microsoft.AspNetCore.Mvc;
using ScheduleManagementApi.models;
using ScheduleManagementApi.models.DTO;

namespace ScheduleManagementApi.controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsumersController(ConsumerConfigurationRepository consumersRepository, ILogger<ConsumersController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<List<ConsumerConfiguration>> Get() =>
        await consumersRepository.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<ConsumerConfiguration>> Get(string id)
    {
        logger.LogInformation("Get request for {Id}", id);
        var consumer = await consumersRepository.GetAsync(id);

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

        await consumersRepository.CreateWallboxAsync(wallboxConfig);

        return CreatedAtAction(nameof(Get), new { id = wallboxConfig.Id }, wallboxConfig);
    }

    [HttpPost("{id}/schedule")]
    public async Task<IActionResult> AddSchedule(string id, [FromBody] ScheduleDto scheduleDto)
    {
        var existingWallboxConfig = await consumersRepository.GetAsync(id);

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

        await consumersRepository.UpdateAsync(id, existingWallboxConfig);

        return Ok(existingWallboxConfig);
    }

    [HttpPost("heatpump")]
    public async Task<IActionResult> Post(HeatpumpConfiguration heatpumpConfiguration)
    {
        await consumersRepository.CreateHeatpumpAsync(heatpumpConfiguration);

        return CreatedAtAction(nameof(Get), new { id = heatpumpConfiguration.Id }, heatpumpConfiguration);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, ConsumerConfiguration updatedConsumer)
    {
        var consumer = await consumersRepository.GetAsync(id);

        if (consumer is null)
        {
            return NotFound();
        }

        updatedConsumer.Id = consumer.Id;

        await consumersRepository.UpdateAsync(id, updatedConsumer);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var consumer = await consumersRepository.GetAsync(id);

        if (consumer is null)
        {
            return NotFound();
        }

        await consumersRepository.RemoveAsync(id);

        return NoContent();
    }
}