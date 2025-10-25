using System;
using System.Collections.Generic;
using System.Text.Json;

namespace EcoStationManagerApplication.Models.Entities
{
    public class User   
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        // Navigation properties
        public List<Role> Roles { get; set; } = new List<Role>();
        public List<Station> Stations { get; set; } = new List<Station>();
    }

    public class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public JsonDocument Permissions { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public DateTime AssignedDate { get; set; }
        public int? AssignedBy { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Role Role { get; set; }
        public User AssignedByUser { get; set; }
    }

    public class UserStation
    {
        public int UserId { get; set; }
        public int StationId { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Station Station { get; set; }
    }
}