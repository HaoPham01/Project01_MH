using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ManagerDoctors.Data;

public partial class Doctor
{
	[StringLength(10, MinimumLength = 10, ErrorMessage = "Id is 10 characters")]
	[Required(ErrorMessage = "Required")]
	[Display(Name = "National ID")]
	public string Id { get; set; } = null!;

	[StringLength(200)]
	[Required(ErrorMessage = "Required")]
	[Display(Name = "Full Name")]
	public string FullName { get; set; } = null!;

	[StringLength(200)]
	[Required(ErrorMessage = "Required")]
	[EmailAddress(ErrorMessage = "Email not recorrect")]
	[Display(Name = "Email")]
	public string Email { get; set; } = null!;


	[Required(ErrorMessage = "Required")]
	[Display(Name = "Date of birth")]
	[DataType(DataType.Date)]
	public DateOnly Date { get; set; }


	[Required(ErrorMessage = "Required")]
	[Display(Name = "Gender")]
	public string Gender { get; set; } = null!;

	[Required(ErrorMessage = "Required")]
	[RegularExpression(@"^\d+$",
   	ErrorMessage = "Is number")]
	[StringLength(10, MinimumLength = 10, ErrorMessage = "{0} is 10 numbers")]
	[Display(Name = "Phone number")]
	public string Phone { get; set; } = null!;

	[StringLength(200)]
	[Required(ErrorMessage = "Required")]
	[Display(Name = "Address")]
	public string Address { get; set; } = null!;

	[Display(Name = "Avatar upload")]
	public string? Avatar { get; set; }

}
