﻿using MVCTest.Models;

namespace MVCTest.Services
{
	public interface IMissionService
	{
		Task<List<MissionModel>?> GetAllMissionsAsync();
	}
}
