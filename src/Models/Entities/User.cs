﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities;

public class User : IdentityUser
{
    [InverseProperty("User")]
    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();
}
