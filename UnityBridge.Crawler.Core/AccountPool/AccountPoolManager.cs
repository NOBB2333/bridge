using System.Collections.Concurrent;
using UnityBridge.Core;

namespace UnityBridge.Crawler.Core.AccountPool;

/// <summary>
/// 账号池管理器，支持多账号轮询、失效标记和自动切换。
/// </summary>
public class AccountPoolManager
{
    private readonly IAccountStore? _accountStore;
    private readonly ProxyPoolManager? _proxyPool;
    private readonly ConcurrentDictionary<string, List<AccountInfo>> _accountsByPlatform = new();
    private readonly ConcurrentDictionary<string, int> _currentIndex = new();
    private readonly object _lock = new();

    /// <summary>
    /// 初始化账号池管理器。
    /// </summary>
    /// <param name="accountStore">账号存储（可选，用于持久化）。</param>
    /// <param name="proxyPool">代理池管理器（可选）。</param>
    public AccountPoolManager(IAccountStore? accountStore = null, ProxyPoolManager? proxyPool = null)
    {
        _accountStore = accountStore;
        _proxyPool = proxyPool;
    }

    /// <summary>
    /// 加载指定平台的账号。
    /// </summary>
    public async Task LoadAccountsAsync(string platform, CancellationToken ct = default)
    {
        if (_accountStore is null) return;

        var accounts = await _accountStore.GetAccountsAsync(platform, ct);
        _accountsByPlatform[platform] = accounts
            .Where(a => a.Status == AccountStatus.Active)
            .ToList();
        _currentIndex[platform] = 0;
    }

    /// <summary>
    /// 手动添加账号到内存池。
    /// </summary>
    public void AddAccount(AccountInfo account)
    {
        var platform = account.Platform;
        if (!_accountsByPlatform.TryGetValue(platform, out var list))
        {
            list = new List<AccountInfo>();
            _accountsByPlatform[platform] = list;
            _currentIndex[platform] = 0;
        }

        lock (_lock)
        {
            list.Add(account);
        }
    }

    /// <summary>
    /// 获取一个可用的账号（轮询策略）。
    /// </summary>
    public AccountInfo? GetNextAccount(string platform)
    {
        if (!_accountsByPlatform.TryGetValue(platform, out var accounts) || accounts.Count == 0)
            return null;

        lock (_lock)
        {
            var activeAccounts = accounts.Where(a => a.Status == AccountStatus.Active).ToList();
            if (activeAccounts.Count == 0) return null;

            if (!_currentIndex.TryGetValue(platform, out var index))
                index = 0;

            var account = activeAccounts[index % activeAccounts.Count];
            _currentIndex[platform] = (index + 1) % activeAccounts.Count;

            account.LastUsedAt = DateTimeOffset.Now;
            return account;
        }
    }

    /// <summary>
    /// 获取一个可用的账号，并关联代理 IP。
    /// </summary>
    public async Task<AccountInfo?> GetAccountWithProxyAsync(string platform, CancellationToken ct = default)
    {
        var account = GetNextAccount(platform);
        if (account is null) return null;

        // 如果账号没有关联代理，且代理池可用，则分配一个代理
        if (string.IsNullOrEmpty(account.ProxyUrl) && _proxyPool is not null)
        {
            var proxy = _proxyPool.GetNextProxy();
            if (proxy?.Address is not null)
            {
                account.ProxyUrl = proxy.Address.ToString();
            }
        }

        return account;
    }

    /// <summary>
    /// 标记账号为无效。
    /// </summary>
    public async Task MarkAccountInvalidAsync(AccountInfo account, CancellationToken ct = default)
    {
        lock (_lock)
        {
            account.Status = AccountStatus.Invalid;
            account.FailureCount++;
        }

        if (_accountStore is not null)
        {
            await _accountStore.UpdateAccountAsync(account, ct);
        }
    }

    /// <summary>
    /// 标记账号为限流状态。
    /// </summary>
    public void MarkAccountRateLimited(AccountInfo account)
    {
        lock (_lock)
        {
            account.Status = AccountStatus.RateLimited;
        }
    }

    /// <summary>
    /// 恢复限流账号为可用状态。
    /// </summary>
    public void RestoreRateLimitedAccounts(string platform)
    {
        if (!_accountsByPlatform.TryGetValue(platform, out var accounts)) return;

        lock (_lock)
        {
            foreach (var account in accounts.Where(a => a.Status == AccountStatus.RateLimited))
            {
                account.Status = AccountStatus.Active;
            }
        }
    }

    /// <summary>
    /// 获取指定平台可用账号数量。
    /// </summary>
    public int GetActiveAccountCount(string platform)
    {
        if (!_accountsByPlatform.TryGetValue(platform, out var accounts)) return 0;
        return accounts.Count(a => a.Status == AccountStatus.Active);
    }
}
