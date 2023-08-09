namespace Common.Models;


/// <summary>
/// 
/// </summary>
/// <param name="Free">Свободная память в байтах</param>
/// <param name="All">Вся память в байтах</param>
public record MemoryData(long Free, long All);