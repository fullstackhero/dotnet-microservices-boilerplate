namespace FSH.Core.Common;

public interface ISerializationService : ITransientService
{
    string Serialize<T>(T obj);

    string Serialize<T>(T obj, Type type);

    T Deserialize<T>(string text);
}
