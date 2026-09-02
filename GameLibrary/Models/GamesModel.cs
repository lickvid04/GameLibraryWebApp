using System;
using System.ComponentModel.DataAnnotations;

namespace GameLibrary;

public class Games {
    [Key]
    public int Game_ID { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public int Rating { get; set; }
        
    public string[]? Tags { get; set; }

    public List<User>? Users { get; set; } 
    
}