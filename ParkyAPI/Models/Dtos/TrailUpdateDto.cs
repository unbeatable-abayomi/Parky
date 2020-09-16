﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ParkyAPI.Models.Trail;

namespace ParkyAPI.Models.Dtos
{
	public class TrailUpdateDto
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }

		[Required]

		public double Distance { get; set; }

		public DifficultyType Difficulty { get; set; }
		[Display(Name = "National Park")]
		public int NationalParkId { get; set; }

		
	}
}
