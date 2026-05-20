using Cysharp.Threading.Tasks;
using Loxodon.Framework.Asynchronous;
using Loxodon.Framework.Binding;
using Loxodon.Framework.Messaging;
using Loxodon.Framework.Views;
using PersistentData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScratcherMatchResultView : Window
{
    private const int ResultDelay = 1000;
    [SerializeField] private TextMeshProUGUI txtScratcher;
    [SerializeField] private TextMeshProUGUI txtScratchAndMatch;
    [SerializeField] private Button bttnClose;
    [SerializeField] private ScratcherGame scratcher;
    private IMessenger messenger;
    private AsyncResult<int> gameResult;

    protected override void OnCreate(IBundle bundle)
    {
        ScratcherMatchResultViewModel viewModel = bundle.Get<ScratcherMatchResultViewModel>("viewModel");
        messenger = viewModel.Messenger;

        Loxodon.Framework.Binding.Builder.BindingSet<ScratcherMatchResultView, ScratcherMatchResultViewModel> bindingSet
            = this.CreateBindingSet(viewModel);

        bindingSet.Bind(txtScratcher).For(v => v.text).To(vm => vm.TxtScratcher).OneTime();
        bindingSet.Bind(txtScratchAndMatch).For(v => v.text).To(vm => vm.TxtScratchAndMatch).OneTime();

        bindingSet.Build();

        bttnClose.onClick.AddListener(OnBttnCloseHandler);

        gameResult = new AsyncResult<int>(true);
        Proceed(viewModel).Forget();
    }

    private async UniTaskVoid Proceed(ScratcherMatchResultViewModel viewModel)
    {
        scratcher.Setup(viewModel.Config);
        scratcher.StartGame(gameResult).Forget();
        await new WaitUntil(() => gameResult.IsDone);
        await UniTask.Delay(ResultDelay);
        PublishGameResult();
    }

    private void PublishGameResult()
    {
        string channel = Constants.MessageChannel.Result;
        if (!gameResult.IsCancelled) messenger.Publish<int>(channel, gameResult.Result);
    }

    private void OnBttnCloseHandler()
    {
        if (gameResult != null && !gameResult.IsDone) gameResult.Cancel();
        Dismiss();
    }

    protected override void OnDismiss()
    {
        if (gameResult != null && !gameResult.IsDone) gameResult.Cancel();
        base.OnDismiss();
    }
}