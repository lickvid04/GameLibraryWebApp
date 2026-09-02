using System;
using BCrypt.Net;  
using System.ComponentModel.DataAnnotations;

namespace GameLibrary;

public class User {
    [Key]
    public int User_ID { get; set; }
    public string Nickname { get; set; }
    public string Mail { get; set; }
    public string Password { get; private set; }
    public void SetPassword(string password) =>
        Password = BCrypt.Net.BCrypt.HashPassword(password);
    public bool VerifyPassword(string password) => 
        BCrypt.Net.BCrypt.Verify(password, Password);
    public List<Games>? GamesList { get; set; }
    public string? Description { get; set; }
}