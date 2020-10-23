﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
	//[Route("api/[controller]")]
	[Route("api/v{version:apiVersion}/nationalParks")]
	[ApiController]
	//[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public class NationalParksController : Controller
	{
		private readonly INationalParkRepository _npRepo ;
		private readonly IMapper _mapper;


		public NationalParksController(INationalParkRepository npRepo, IMapper mapper)
		{
			_npRepo = npRepo;
			_mapper = mapper;
		}
		/// <summary>
		/// Get list of all national Parks.
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		
		[ProducesResponseType(200, Type = typeof(List<NationalParkDto>))]
		[ProducesResponseType(400)]
		//[ProducesResponseType(400)]
		public IActionResult GetNationalParks()
		{
			var objList = _npRepo.GetNationalParks();
			var objDto = new List<NationalParkDto>(); 

			foreach(var obj in objList)
			{
				objDto.Add(_mapper.Map<NationalParkDto>(obj));
			}
			return Ok(objDto);
		}
		/// <summary>
		/// Get individual national Park 
		/// </summary>
		/// <param name="nationalParkId"> The Id of the national Park</param>
		/// <returns></returns>

		[HttpGet("{nationalParkId:int}",Name = "GetNationalPark")]
		[ProducesResponseType(200, Type = typeof(NationalParkDto))]
		//[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		[ProducesDefaultResponseType]

		public IActionResult GetNationalPark(int nationalParkId)
		{
			var obj = _npRepo.GetNationalPark(nationalParkId);
			if(obj == null)
			{
				return NotFound();
			}
			var objDto = _mapper.Map<NationalParkDto>(obj);


			//WITH OUT AUTO MAPPER THIS IS THE CODE U'LL WRITE BELOW
			//var objDto = new NationalParkDto()
			//{
			//	Created = obj.Created,
			//	Id = obj.Id,
			//	Name = obj.Name,
			//	State = obj.State
			//};

			return Ok(objDto);
		}

		[HttpPost]
		[ProducesResponseType(201, Type = typeof(NationalParkDto))]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		

		public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
		{
			if (nationalParkDto == null)
			{
				return BadRequest(ModelState);
			}

			if (_npRepo.NationalParkExists(nationalParkDto.Name))
			{
				ModelState.AddModelError("","National Park Exists");
				return StatusCode(404, ModelState);
			}

			//if (!ModelState.IsValid)
			//{
			//	return BadRequest(ModelState);
			//}

			var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);

			if (!_npRepo.CreateNationalPark(nationalParkObj))
			{
				ModelState.AddModelError("", $"Something went wrong when saving the record {nationalParkObj.Name}");

				return StatusCode(500, ModelState);
			}
			return CreatedAtRoute("GetNationalPark", new { version = HttpContext.GetRequestedApiVersion().ToString(), nationalParkId = nationalParkObj.Id},nationalParkObj);
		}




		[HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]

		public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDto nationalParkDto)
		{
			if (nationalParkDto == null || nationalParkId != nationalParkDto.Id)
			{
				return BadRequest(ModelState);
			}

			var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);

			if (!_npRepo.UpdateNationalPark(nationalParkObj))
			{
				ModelState.AddModelError("", $"Something went wrong when updating the record {nationalParkObj.Name}");

				return StatusCode(500, ModelState);
			}
			return NoContent();
		}
		[HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult DeleteNationalPark(int nationalParkId)
		{
			if (!_npRepo.NationalParkExists(nationalParkId))
			{
				
				return NotFound();
			}

			var nationalParkObj = _npRepo.GetNationalPark(nationalParkId);

			if (!_npRepo.DeleteNationalPark(nationalParkObj))
			{
				ModelState.AddModelError("", $"Something went wrong when deleteing the record {nationalParkObj.Name}");

				return StatusCode(500, ModelState);
			}
			return NoContent();
		}
	}
}
