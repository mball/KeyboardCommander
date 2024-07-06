using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using KeyboardCommander.Engine.IO;

namespace KeyboardCommander.States;

public enum PianoKeys
{
    C,
    CSharp,
    D,
    DSharp,
    E,
    F,
    FSharp,
    G,
    GSharp,
    A,
    ASharp,
    B,
}

[Serializable]
public record PlayerState
{
    [JsonConverter(typeof(BigIntegerConverter))]
    public BigInteger Inspiration { get; set; }

    [JsonIgnore]
    public BigInteger[] InspirationPerClickMultiplier { get; set; } = new BigInteger[12];
    
    // Unlocks
    public bool[] UnlockedPianoKeys { get; set; } = new bool[12];
    
    // Upgrades
    public int[] PianoKeyTuning { get; set; } = new int[12];
    
    [JsonIgnore]
    public bool IsSaving { get; set; }
    
    public PlayerState()
    {
        Inspiration = 0;
        for (var i = 0; i < InspirationPerClickMultiplier.Length; i++)
        {
            InspirationPerClickMultiplier[i] = 1;
        }
        UnlockedPianoKeys[0] = true;
        
        IsSaving = false;
    }
    
    public static PlayerState LoadOrCreate()
    {
        var state = new PlayerState();
        state.Load();
        return state;
    }
    
    public void Load()
    {
        using var dataFile = IsolatedStorageFile.GetUserStoreForApplication();
        
        if (!dataFile.FileExists("player_state.save"))
        {
            return;
        }

        using var saveFile = dataFile.OpenFile("player_state.save", FileMode.Open);

        var serializedState = new byte[saveFile.Length];
        var bytesRead = saveFile.Read(serializedState, 0, serializedState.Length);
        
        saveFile.Close();
        dataFile.Close();
        
        var base64 = Encoding.UTF8.GetString(serializedState.AsSpan(0, bytesRead));
        var json = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
        var loadedState = JsonSerializer.Deserialize<PlayerState>(json);

        Inspiration = loadedState.Inspiration;
        UnlockedPianoKeys = loadedState.UnlockedPianoKeys;
        PianoKeyTuning = loadedState.PianoKeyTuning;
    }
    
    public void Save()
    {
        IsSaving = true;
        
        var json = JsonSerializer.Serialize(this);
        var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        var serializedState = Encoding.UTF8.GetBytes(base64);
        
        using var dataFile = IsolatedStorageFile.GetUserStoreForApplication();
        using var saveFile = dataFile.OpenFile("player_state.save", FileMode.Create);
        
        saveFile.Write(serializedState, 0, serializedState.Length);
        
        saveFile.Close();
        dataFile.Close();
        
        IsSaving = false;
    }
    
    public void ManualSave(string path)
    {
        IsSaving = true;
        
        var json = JsonSerializer.Serialize(this);
        var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        var serializedState = Encoding.UTF8.GetBytes(base64);
        
        using var saveFile = File.OpenWrite(path);
        
        saveFile.Seek(0, SeekOrigin.Begin);
        saveFile.Write(serializedState, 0, serializedState.Length);
        saveFile.SetLength(serializedState.Length);
        
        saveFile.Close();
        
        IsSaving = false;
    }
    
    public void ManualLoad(string path)
    {
        using var saveFile = File.OpenRead(path);
        
        var serializedState = new byte[saveFile.Length];
        
        saveFile.Seek(0, SeekOrigin.Begin);
        var bytesRead = saveFile.Read(serializedState);
        
        saveFile.Close();
        
        var base64 = Encoding.UTF8.GetString(serializedState.AsSpan(0, bytesRead));
        var json = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
        var loadedState = JsonSerializer.Deserialize<PlayerState>(json);

        Inspiration = loadedState.Inspiration;
        UnlockedPianoKeys = loadedState.UnlockedPianoKeys;
        PianoKeyTuning = loadedState.PianoKeyTuning;
    }
}