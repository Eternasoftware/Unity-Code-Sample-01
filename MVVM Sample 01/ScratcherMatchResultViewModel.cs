using Loxodon.Framework.Messaging;
using Loxodon.Framework.ViewModels;
using PersistentData;

public class ScratcherMatchResultViewModel : ViewModelBase
{
    private readonly ScratcherMatchResultModel model;
    private readonly ScratcherGameConfig config;
    private readonly ISubscription<int> subscription;

    public ScratcherMatchResultViewModel(ScratcherMatchResultModel model)
    {
        this.model = model;
        Messenger = new Messenger();
        config = this.model.GenerateConfig();

        string channel = Constants.MessageChannel.Result;
        subscription = Messenger.Subscribe<int>(channel, OnResultHandler);
    }

    private void OnResultHandler(int result)
    {
        model.RequestPrizeFromServer(result).Forget();
    }

    public string TxtScratcher => model.TxtScratcher;
    public string TxtScratchAndMatch => model.TxtScratchAndMatch;

    public ScratcherGameConfig Config => config;

    protected override void Dispose(bool disposing)
    {
        subscription.Dispose();
        base.Dispose(disposing);
    }
}