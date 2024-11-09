using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Data;

public class DataLoader {

    private readonly ApplicationDbContext _context;
    public DataLoader(ApplicationDbContext context) { _context = context; }

    /// <summary>
    /// Залить сущности в БД
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    /// <param name="data">Список сущностей</param>
    /// <returns></returns>
    public async Task UploadDataAsync<T>(IEnumerable<T> data) where T : class {
        if (data == null || !data.Any()) return;

        await _context.Set<T>().AddRangeAsync(data);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Загрузить данные из файла
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="file"></param>
    /// <returns></returns>
    public async Task UploadDataFromFileAsync<T>([FromForm] IFormFile file) where T : class {
        if (file == null || file.Length == 0) return;

        using (StreamReader reader = new StreamReader(file.OpenReadStream())) {
            string jsonContent = await reader.ReadToEndAsync() ?? string.Empty;
            IEnumerable<T> objects = JsonConvert.DeserializeObject<IEnumerable<T>>(jsonContent) ?? Enumerable.Empty<T>();
            await UploadDataAsync<T>(objects);
        }
    }


}