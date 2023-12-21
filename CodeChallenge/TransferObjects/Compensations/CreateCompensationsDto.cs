using CodeChallenge.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;

namespace CodeChallenge.TransferObjects.Compensations
{
    public record CreateCompensationsDto
    {
        [Required]
        public Decimal? Salary { get; set; }

        [Required]
        public DateTime? EffectiveDate { get; set; }
    }
}
