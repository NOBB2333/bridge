namespace UnityBridge.Core.Models;

/// <summary>
/// SKIT.FlurlHttpClient 请求基类。
/// </summary>
public abstract class CommonRequestBase
{
    /// <inheritdoc/>
    [System.Text.Json.Serialization.JsonIgnore]
    public virtual int? Timeout { get; set; }
}