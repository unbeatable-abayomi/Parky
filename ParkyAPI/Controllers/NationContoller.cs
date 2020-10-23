using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.ParkyMapper;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkyAPI.Controllers
{
	public class NationContoller : Controller
	{
		public readonly IMapper _newParky;

		public readonly INationalParkRepository _npRepo;

		public NationContoller(IMapper newParky, INationalParkRepository nationalPark)
		{
			_newParky = newParky;
			_npRepo = nationalPark;
		}


		[HttpGet]
		[ProducesResponseType(200, Type = typeof(List<NationalParkDto>))]
		[ProducesResponseType(400)]
		public IActionResult GetAllNationalParks()
		{
			var objFromBD = _npRepo.GetNationalParks();

			var listOfNpRepo = new List<NationalParkDto>();


			foreach (var list in objFromBD)
			{
				listOfNpRepo.Add(_newParky.Map<NationalParkDto>(list));
			}
			return Ok();
		}

		[HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
		[ProducesResponseType(200, Type = typeof(NationalParkDto))]
		[ProducesResponseType(400)]
		[ProducesDefaultResponseType]

		public IActionResult GetNationalPark(int nationalParkId)
		{
			var objFromDb = _npRepo.GetNationalPark(nationalParkId);

			if (objFromDb == null)
			{
				return NotFound();
			}


			var returnObj = _newParky.Map<NationalParkDto>(objFromDb);
			return Ok(returnObj);
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
				ModelState.AddModelError("", "National Park Already Exits");
				return StatusCode(404, ModelState);
			}
			var objToSave = _newParky.Map<NationalPark>(nationalParkDto);
			if (!_npRepo.CreateNationalPark(objToSave))
			{
				ModelState.AddModelError("", $"Something went wrong when creating{objToSave.Name} ");
				return StatusCode(500, ModelState);
			}

			return CreatedAtRoute("GetNationalPark", new { nationalParkId = objToSave.Id }, objToSave);
		}

		[HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDto nationalParkDto)
		{
			if (nationalParkDto.Id != nationalParkId || nationalParkDto == null)
			{
				return BadRequest(ModelState);
			}


			var objToBd = _newParky.Map<NationalPark>(nationalParkDto);
			if (!_npRepo.UpdateNationalPark(objToBd))
			{
				ModelState.AddModelError("", $"Some went when updating{objToBd.Name} ");
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
			var objFromDb = _npRepo.GetNationalPark(nationalParkId);
			if (!_npRepo.DeleteNationalPark(objFromDb))
			{
				ModelState.AddModelError("", $" Error while deleting{objFromDb.Name}");
				return StatusCode(500, ModelState);
			}
			return NoContent();
		}

	}
}
