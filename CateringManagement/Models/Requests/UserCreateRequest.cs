﻿using DAL.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CateringManagement.Models.Requests
{
    public class UserCreateRequest
    {
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Status { get; set; }
        public int Sex { get; set; }
        public UserPosition Role { get; set; }
        

    }
}
