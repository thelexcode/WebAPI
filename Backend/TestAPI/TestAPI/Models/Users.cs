﻿namespace TestAPI.Models
{
    public class Users
    {
 
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } 
        public string RoleDescription { get; set; }
        public int RoleId { get; set; }
    }
}
