namespace UnityBridge.Crawler.Core.AccountPool;

/// <summary>
/// 账号存储接口。
/// </summary>
public interface IAccountStore
{
    /// <summary>
    /// 获取指定平台的所有账号。
    /// </summary>
    Task<IReadOnlyList<AccountInfo>> GetAccountsAsync(string platform, CancellationToken ct = default);

    /// <summary>
    /// 更新账号信息。
    /// </summary>
    Task UpdateAccountAsync(AccountInfo account, CancellationToken ct = default);

    /// <summary>
    /// 添加账号。
    /// </summary>
    Task AddAccountAsync(AccountInfo account, CancellationToken ct = default);

    /// <summary>
    /// 删除账号。
    /// </summary>
    Task RemoveAccountAsync(string platform, string accountName, CancellationToken ct = default);
}
