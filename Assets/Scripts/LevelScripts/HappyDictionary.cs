using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappyDictionary : EvilDictionary
{
    public override List<string> EvilNames()
    {
        return happyNames;
    }

    public List<string> happyNames = new List<string> {
        "Cheer",
        "Meme", "Mistery", "Gift",
        ":)", ":')", ":D", ":O",
        "Happiness", "Holiday", "Spirit", "Firework",
        "Joy", "Spark", "Christmas", "Candy", "Candy Cane", "Elf", "Santa", "Surprise",
        "Dude", "Haha", "Hoho", "Ooooooooof", "Doooood",
        "Snow", "Snowflakes", "Icicle", "Joke",
        "Game", "Playful", "Glittering", "Sparkling",
        "Friendly", "Friend", "Girlfriend", "Good boy", "Nice",
        "Warm", "Heart", "Green", "Stocking", "Deer", "Raindeer",
        "Sledge", "Snowball", "Meow", "Fireplace", "Mulled Wine",
        "Chocolate", "Cake", "Pie", "Red", "White", "Wrapper",
        "Confetti", "Celebration", "Eve", "Gathering", "Invitation",
        "Kiss", "Midnight", "Toast", "Twelve o'clock", "Dancing",
        "Balloons", "Countdown", "Hehe"
    };
}
