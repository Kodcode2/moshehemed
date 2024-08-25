﻿using System.Collections.Generic;
using FotTestApi.Data;
using FotTestApi.ModelDto;
using FotTestApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FotTestApi.Service;

public class AgentService : IAgenService
{
	private readonly ApplicationBDContext _context;
	private readonly IMissionsService _missionsService;


	public AgentService(ApplicationBDContext context, IMissionsService missionsService)
	{
		_context = context;
		_missionsService = missionsService;
	}

	private readonly Dictionary<string, (int x, int y)> stepsByDirection = new()
	{
		{"n", (x: 0,y: 1) },
		{"s", (x: 0,y: -1) },
		{"e", (x: 1,y: 0) },
		{"w", (x: -1,y: 0) },
		{"nw", (x: -1,y: 1) },
		{"ne", (x: 1,y: 1) },
		{"sw", (x: -1,y: -1) },
		{"se", (x: 1,y: -1) }
	};
	public async Task<AgentModel> CreateAsyncService(AgentDto agentDto)
	{
		try
		{
			AgentModel newAgent = new AgentModel()
			{
				Nickname = agentDto.Nickname,
				Image = agentDto.photoUrl,
			};
			await _context.Agents.AddAsync(newAgent);
			await _context.SaveChangesAsync();
			return newAgent;
		}
		catch (Exception ex)
		{
			throw new Exception($"{ex.Message}", ex);
		}
	}


	public async Task<AgentModel?> FindByIdService(int id)
	{
		AgentModel? agent = await _context.Agents.FindAsync(id);
		if (agent is null) { throw new Exception(ErrorMassage.errorNotFound); }
		return agent;
	}

	public async Task StartingPositionService(LocationDto location, int id)
	{
		try
		{
			TestLocation(location);
			AgentModel? agent = await FindByIdService(id);
			agent!.Location_x = location.x;
			agent.Location_y = location.y;
			await _context.SaveChangesAsync();
			await _missionsService.CheckIfCreateMission();
		}
		catch (Exception ex)
		{
			throw new Exception($"{ex.Message}", ex);
		}
	}


	public void TestLocation(LocationDto location)
	{
		if (location.x < 0 || location.x > 1000 || location.y < 0 || location.y > 1000)
		{
			throw new Exception(ErrorMassage.errorNumberHighOrLow);
		}
	}

	public async Task<AgentModel?> StepsAgent(string diraction, int id)
	{
		try
		{
			bool keyExists = stepsByDirection.ContainsKey(diraction);
			if (!keyExists)
			{
				throw new Exception(ErrorMassage.notContainsKey);
			}
			var (xForTuple, yForTuple) = stepsByDirection[diraction];
			AgentModel? agent = await FindByIdService(id);
			TestLocation(new()
			{
				x = xForTuple + agent!.Location_x,
				y = yForTuple + agent.Location_y
			});
			agent.Location_x += xForTuple;
			agent.Location_y += yForTuple;
			await _context.SaveChangesAsync();
			await _missionsService.CheckIfCreateMission();
			return agent;

		}
		catch (Exception ex)
		{
			throw new Exception($"{ex.Message}", ex);
		}

	}

	public async Task<List<AgentModel>?> GetAllAgent() =>
		await _context.Agents.ToListAsync();

}