namespace Common.Models;


/// <summary>
/// Модель показателей.
/// </summary>
/// <param name="RAM">Оперативная память</param>
/// <param name="PSD">ПЗУ</param>
/// <param name="CpyPercent">Использование процессора в %</param>
public record MetricsData(MemoryData RAM, MemoryData PSD, int CpyPercent);