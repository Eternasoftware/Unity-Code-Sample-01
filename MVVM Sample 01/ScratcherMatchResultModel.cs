using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Tools;
using Core.Services.Data;
using Core.Services.Data.Remote;
using Core.Services.Localization;
using Core.Services.Network;
using Core.UI;
using Cysharp.Threading.Tasks;
using Loxodon.Framework.Asynchronous;
using Loxodon.Log;
using Tools.Log;

public class ScratcherMatchResultModel
{
    private const string ScratcherKey = "menu_mini_game_scratcher";
    private const string ScratchAndMatchKey = "menu_mini_games_scratcher_message";
    private const int CardAmount = 12;
    private const int MatchCount = 3;
    private const int RemainingCount = 9;
    private const int RewardDelay = 1200;
    private const float RequestProfileDelay = 0.3f;
    private readonly ILog log = LogProvider.GetLogger(nameof(ScratcherMatchResultModel));
    private readonly LocalizationService localizationService;
    private readonly NetworkService networkService;
    private readonly AsyncResult rewardResult;
    private readonly RemoteConfigData remoteConfig;

    public ScratcherMatchResultModel(LocalizationService localizationService, NetworkService networkService, WindowService windowService, AsyncResult rewardResult, UserData user, RemoteConfigData remoteConfig)
    {
        this.localizationService = localizationService;
        this.networkService = networkService;
        this.rewardResult = rewardResult;
        this.remoteConfig = remoteConfig;
    }
    public string TxtScratcher => localizationService.GetString(ScratcherKey);
    public string TxtScratchAndMatch => localizationService.GetString(ScratchAndMatchKey);

    public ScratcherGameConfig GenerateConfig()
    {
        log.Debug("GenerateConfig");
        ScratcherGameConfig.Item selectedItem = SelectItemByWeight(remoteConfig.ScratcherItems.ToArray());
        ScratcherGameConfig.Item[] resultItems = new ScratcherGameConfig.Item[CardAmount];

        for (int i = 0; i < MatchCount; i++)
        {
            resultItems[i] = new ScratcherGameConfig.Item()
            {
                id = selectedItem.id,
                amount = selectedItem.amount,
                weight = selectedItem.weight
            };
        }

        ScratcherGameConfig.Item[] remainingItems = GetRandomUniqueItems(remoteConfig.ScratcherItems.ToArray(), selectedItem, RemainingCount);
        for (int i = 0; i < RemainingCount; i++)
        {
            resultItems[i + MatchCount] = remainingItems[i];
        }

        resultItems.Shuffle();

        ScratcherGameConfig result = new ScratcherGameConfig()
        {
            gameResultType = ScratcherGameConfig.GameResultType.Win,
            winItemId = selectedItem.id,
            items = resultItems
        };
        log.DebugFormat("GenerateConfig - winItemId: {0}", result.winItemId);
        return result;
    }

    private ScratcherGameConfig.Item SelectItemByWeight(ScratcherGameConfig.Item[] items)
    {
        log.DebugFormat("SelectItemByWeight - items count: {0}", items.Length);
        var validItems = items.Where(item => item.weight > 0).ToArray();

        if (validItems.Length == 0)
            return items[0];

        int maxWeight = validItems.Max(item => item.weight);
        float normalizationFactor = 100f / maxWeight;

        List<(ScratcherGameConfig.Item item, float normalizedWeight)> weightedItems = new List<(ScratcherGameConfig.Item, float)>();
        float totalNormalizedWeight = 0f;

        foreach (var item in validItems)
        {
            float normalizedWeight = item.weight * normalizationFactor;
            weightedItems.Add((item, normalizedWeight));
            totalNormalizedWeight += normalizedWeight;
        }

        System.Random random = new System.Random();
        float randomValue = (float)random.NextDouble() * totalNormalizedWeight;
        float currentWeight = 0f;

        foreach (var (item, weight) in weightedItems)
        {
            currentWeight += weight;
            if (randomValue < currentWeight)
            {
                log.DebugFormat("SelectItemByWeight - selected id: {0}, weight: {1}", item.id, item.weight);
                return item;
            }
        }

        log.DebugFormat("SelectItemByWeight - fallback id: {0}, weight: {1}", validItems[0].id, validItems[0].weight);
        return validItems[0];
    }

    private ScratcherGameConfig.Item[] GetRandomUniqueItems(ScratcherGameConfig.Item[] allItems, ScratcherGameConfig.Item excludedItem, int count)
    {
        log.DebugFormat("GetRandomUniqueItems - count: {0}, excluded: {1}", count, excludedItem?.id);
        List<ScratcherGameConfig.Item> availableItems = new List<ScratcherGameConfig.Item>(allItems);
        if (excludedItem != null) availableItems.Remove(excludedItem);

        System.Random random = new System.Random();
        for (int i = availableItems.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            var temp = availableItems[i];
            availableItems[i] = availableItems[j];
            availableItems[j] = temp;
        }

        return availableItems.Take(count).ToArray();
    }

    public async UniTaskVoid RequestPrizeFromServer(int result)
    {
        log.DebugFormat("RequestPrizeFromServer - result: {0}", result);
        if (result > 0)
        {
            rewardResult.SetResult();
            await UniTask.Delay(RewardDelay);
            networkService.GetMiniGameRewardSecretly(MiniGameType.Scratcher,
                                                     TokenType.Free,
                                                     MiniGamePrizeType.Chips,
                                                     result);
            networkService.UpdateProfile(RequestProfileDelay);
        }
    }
}