using VideoGrabberMVVMGUI.DI;

namespace VideoGrabberMVVMGUI.Models;

public partial class MainViewModel
{
    private readonly IBusinessService _businessService;

    public MainViewModel(IBusinessService businessService)
    {
        _businessService = businessService;
    }
}
